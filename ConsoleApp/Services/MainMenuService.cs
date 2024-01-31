using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Factories;
using Infrastructure.Respositories;
using Infrastructure.Respositories.ProductRepositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.IdentityModel.Tokens;

namespace ConsoleApp.Services;

public class MainMenuService(UserService userService, ProductService productService, MenuService menuService, UserRepository userRepository, VerificationRepository verificationRepository, ProfileRepository profileRepository, ProductRepository productRepository, OrderRowRepository orderRowRepository, OrderRepository orderRepository, UserFactories userFactories, ProductFactories productFactories) 
{
    private readonly UserService _userService = userService;
    private readonly ProductService _productService = productService;
    private readonly MenuService _menuService = menuService;

    private readonly UserRepository _userRepository = userRepository;
    private readonly VerificationRepository _verificationRepository = verificationRepository;
    private readonly ProfileRepository _profileRepository = profileRepository;

    private readonly ProductRepository _productRepository = productRepository;
    private readonly OrderRowRepository _orderRowRepository = orderRowRepository;

    private readonly UserFactories _userFactories = userFactories;
    private readonly ProductFactories _productFactories = productFactories;

    public void ShowMainMenu()
    {
        Console.Clear();
        Console.WriteLine("*** MAIN MENU ***\n\n");
        Console.WriteLine("Please, choose an option:");

        Console.WriteLine("\n\n----------User Options------------\n");
        Console.WriteLine("1.\t Add a new user");
        Console.WriteLine("2.\t Show and inspect users");

        Console.WriteLine("\n\n----------Product Options------------\n");
        Console.WriteLine("3.\t Admin menu");
        Console.WriteLine("4.\t Browse products & orders");

        Console.WriteLine("\n\n----------Menu Options------------\n");
        Console.WriteLine("0.\t Exit program.\n");

        var input = Console.ReadLine();

        switch (input)
        {
            case "1":
                ShowAddUserMenu();
                break;

            case "2":
                ShowAllUsersMenu();
                break;

            case "3":
                ShowAdminProductMenu();
                break;

            case "4":
                ShowVerifyEmailMenu();
                break;

            case "0":
                Environment.Exit(0);
                break;

            default:
                Console.WriteLine("Invalid choice, press any key to continue.");
                Console.ReadKey();
                ShowMainMenu();
                break;

        }
    }

    public void ShowAddUserMenu()
    {
        var userReg = new UserRegistrationForm();
        bool emailCheck = true;
        Console.Clear();

        _menuService.SetUserProperties(value => userReg.FirstName = value, "Enter first name: ", "First name may not be empty!");

        _menuService.SetUserProperties(value => userReg.LastName = value, "Enter surname: ", "Surname may not be empty!");

        while (emailCheck)
        {
            Console.Write("Enter email: ");
            var emailInput = Console.ReadLine()!;

            if (!string.IsNullOrEmpty(emailInput))
            {
                if (!_verificationRepository.Exists(x => x.Email == emailInput))
                {
                    userReg.Email = emailInput;
                    emailCheck = false;
                }
                else
                {
                    Console.WriteLine("\nA user with that E-mail already exists!");
                    ReturnToMainMenu("Press any key to try again, or press 0 to return to the main menu.");
                }
            }
            else
                Console.WriteLine("Email may not be empty!");
        }

        _menuService.SetUserProperties(value => userReg.Password = value, "Enter password: ", "Password may not be empty!");

        _menuService.SetUserProperties(value => userReg.Street = value, "Enter streetname: ", "Streetname may not be empty!");

        _menuService.SetUserProperties(value => userReg.PostalCode = value, "Enter postal code: ", "Postal code may not be empty!");

        _menuService.SetUserProperties(value => userReg.City = value, "Enter city: ", "City may not be empty!");

        var result = _userService.CreateUser(userReg);

        Console.Clear();
        if (result)
            Console.WriteLine("User added successfully!");
        else
            Console.WriteLine("Something went wrong, please try again.");

        Console.ReadKey();
        ShowMainMenu();
    }

    public void ShowAllUsersMenu()
    {
        var userList = _userService.GetAllUsers();
        Console.Clear();

        Console.WriteLine("Displaying all users:\n\n ");
        foreach (var (user, index) in userList.Select((u, i) => (u, i)))
        {
            Console.WriteLine("---------------------------");
            Console.WriteLine($"User: \t\t{index + 1}");
            Console.WriteLine($"First Name: \t{user.FirstName}");
            Console.WriteLine($"Last Name: \t{user.LastName}");
            Console.WriteLine("---------------------------\n");
        }
        Console.WriteLine("Enter the number of which user you would like to inspect, or enter 0 to return to the main menu.");
        var result = int.Parse(Console.ReadLine()!);

        if (result == 0)
            ShowMainMenu();
        else
        {
            UserDto selectedUser = userList.ElementAt(result - 1);
            VerificationEntity e = _verificationRepository.GetOne(x => x.Email == selectedUser.Email);
            ProfileEntity entity = _profileRepository.GetOne(x => x.UserId == e.UserId);

            Console.Clear();
            ShowEditUsersMenu(entity);
        }
    }

    public void ShowEditUsersMenu(ProfileEntity entity)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("-----------------------------------");
            Console.WriteLine($"Value");
            Console.WriteLine($"1  -  First Name: \t{entity.User.FirstName}");
            Console.WriteLine($"2  -  Last Name: \t{entity.User.LastName}");
            Console.WriteLine($"3  -  Email: \t\t{entity.User.Verification.Email}");
            Console.WriteLine("4  -  ADRESS");
            Console.WriteLine($"\tCity: \t\t{entity.Address.City}");
            Console.WriteLine($"\tStreet: \t{entity.Address.Street}");
            Console.WriteLine($"\tPostal Code: \t{entity.Address.PostalCode}");
            Console.WriteLine("-----------------------------------\n\n");
            Console.WriteLine("*\t Enter the value of the property you would like to update.");
            Console.WriteLine("**\t Enter 9 to delete the user.");
            Console.WriteLine("***\t Enter 0 to return to user menu.");
            var result = Console.ReadLine();

            switch (result)
            {
                case "1":

                    Console.WriteLine("Enter a new first name:");
                    var firstName = Console.ReadLine()!;

                    if (!firstName.IsNullOrEmpty())
                    {
                        UserEntity entityToUpdate = _userRepository.GetOne(x => x.Id == entity.UserId);
                        entityToUpdate.FirstName = firstName;
                        _userRepository.Update(entityToUpdate, x => x.Id == entityToUpdate.Id);

                        _menuService.InputValidation("Update successful!");
                    }
                    break;

                case "2":

                    Console.WriteLine("Enter a new last name:");
                    var lastName = Console.ReadLine()!;

                    if (!lastName.IsNullOrEmpty())
                    {
                        UserEntity entityToUpdate = _userRepository.GetOne(x => x.Id == entity.UserId);
                        entityToUpdate.LastName = lastName;
                        _userRepository.Update(entityToUpdate, x => x.Id == entityToUpdate.Id);

                        _menuService.InputValidation("Update successful!");
                    }
                    break;

                case "3":

                    Console.WriteLine("Enter a new e-mail:");
                    var email = Console.ReadLine()!;

                    if (!email.IsNullOrEmpty())
                    {
                        if (!_verificationRepository.Exists(x => x.Email == email))
                        {
                            VerificationEntity entityToUpdate = _verificationRepository.GetOne(x => x.UserId == entity.UserId);
                            entityToUpdate.Email = email;
                            _verificationRepository.Update(entityToUpdate, x => x.UserId == entityToUpdate.UserId);

                            _menuService.InputValidation("Update successful!");
                        }
                        else
                        {
                            _menuService.InputValidation("\nA user with that E-mail already exists.\nPress any key to try again.");
                        }
                    }
                    else
                    {
                        _menuService.InputValidation("\nEmail may not be empty.\nPress any key to try again.");
                    }
                    break;


                case "4":

                    UpdateAddress(entity);
                    break;

                case "9":

                    _verificationRepository.Delete(x => x.UserId == entity.UserId);
                    _userRepository.Delete(x => x.Id == entity.UserId);
                    _profileRepository.Delete(x => x.UserId == entity.UserId);

                    _menuService.InputValidation("Successfully deleted the user!");
                    ShowMainMenu();
                    break;

                case "0":
                    ShowMainMenu();
                    break;

                default:
                    Console.WriteLine("Invalid option, please try again.");
                    break;
            }
        }
    }

    public void ShowAdminProductMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.Write("Please enter your E-mail to verify your role: ");
            var input = Console.ReadLine()!;
            if (!string.IsNullOrEmpty(input))
            {
                var verificationEntity = _verificationRepository.GetOne(x => x.Email == input);
                var profileEntity = _profileRepository.GetOne(x => x.UserId == verificationEntity.UserId);

                if(profileEntity != null && profileEntity.RoleId == 1)
                {
                    Console.Clear();
                    Console.WriteLine("Choose an option:\n");
                    Console.WriteLine("1.\t Add a new product.");
                    Console.WriteLine("2.\t Inspect and delete current products.");
                    Console.WriteLine("3.\t Return to main menu");
                    var choice = Console.ReadLine();

                    switch(choice) 
                    {
                        case "1":
                            ShowAddProductMenu();
                            break;

                        case "2":
                            ShowDeleteProductsMenu();
                            break;

                        case "3":
                            ShowMainMenu();
                            break;

                        default:
                            Console.WriteLine("Invalid choice.\nPress any key to continue.");
                            Console.ReadKey();
                            break;
                    }
                }
                else
                {
                    _menuService.InputValidation("\nAdmin authenticity not found.\nPress any key to return to the main menu.");
                    ShowMainMenu();
                }
            }
            else
            {
                _menuService.InputValidation("\nYou must enter an e-mail.\nPress any key to try again.");
            }
        }
    }

    public void ShowAddProductMenu()
    {
        var product = new ProductDto();
        Console.Clear();

        _menuService.SetProductProperties(value => product.Title = value, "Enter title: ", "Title may not be empty!");

        _menuService.SetProductProperties(value => product.Description = value, "Enter description: ", "Description may not be empty!");

        _menuService.SetPrice(value => product.Price = value, "Enter price: ", "Price may not be empty!");

        _menuService.SetProductProperties(value => product.CategoryName = value, "Enter category: ", "Category may not be empty!");

        _menuService.SetProductProperties(value => product.ManufacturerName = value, "Enter manufacturer: ", "Manufacturer may not be empty!");

        var result = _productService.CreateProduct(product);

        Console.Clear();
        if (result)
            Console.WriteLine("Product added successfully!");
        else
            Console.WriteLine("Something went wrong, please try again.");

        Console.ReadKey();
        ShowMainMenu();
    }

    public void ShowDeleteProductsMenu()
    {
        var productList = _productService.GetAllProducts();

        while(true)
        {
            Console.Clear();

            foreach (var product in productList)
            {
                Console.WriteLine($"------------------------------------");
                Console.WriteLine($"Product ID: {product.Id}");
                Console.WriteLine($"Title: {product.Title}");
                Console.WriteLine($"------------------------------------\n");
            }

            Console.Write("Enter the ID of the product you would like to inspect: ");

            var result = Console.ReadLine();
            if (!string.IsNullOrEmpty(result))
            {
                var parsedResult = Int32.Parse(result!);
                var entity = _productRepository.GetOne(x => x.Id == parsedResult);

                if(_productRepository.Exists(x => x.Id == entity.Id))
                {
                    Console.Clear();
                    Console.WriteLine($"Title: \t{entity.Title}");
                    Console.WriteLine($"Description: \t{entity.Description}");
                    Console.WriteLine($"Price: \t\t{entity.Price}");
                    Console.WriteLine($"Category: \t{entity.Category.CategoryName}");
                    Console.WriteLine($"Manufacturer: \t{entity.Manufacturer.ManufacturerName}");
                    Console.WriteLine("\nPress 9 to delete this product, or press any key to return to the main menu.");

                    var input = Console.ReadLine()!;

                    if (input == "9")
                    {
                        var deleteResult = _productRepository.Delete(x => x.Id == entity.Id);
                        if (deleteResult)
                        {
                            _menuService.InputValidation("Product deleted successfully, press any key to return.");
                            ShowDeleteProductsMenu();
                        }
                        else
                        {
                            _menuService.InputValidation("Something went wrong, press any key to return.");
                            ShowDeleteProductsMenu();
                        }
                    }
                    else
                        ShowMainMenu();
                }
                else
                {
                    _menuService.InputValidation("A product with that ID does not exist, please try again.");
                }
            }
            else
            {
                _menuService.InputValidation("You have to enter an ID, please try again.");
            }
        }
    }

    public void ShowVerifyEmailMenu()
    {
        bool emailCheck = true;

        while(emailCheck)
        {
            Console.Clear();
            Console.Write("Please enter your e-mail to begin browsing: ");
            var userEmail = Console.ReadLine();

            if (!string.IsNullOrEmpty(userEmail))
            {
                if (_verificationRepository.Exists(x => x.Email == userEmail))
                {
                    emailCheck = false;

                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("Choose an option:");
                        Console.WriteLine("1.\tBrowse and purchase products");
                        Console.WriteLine("2.\tInspect your orders");
                        Console.WriteLine("0.\tReturn to main menu");
                        var choice = Console.ReadLine();

                        switch (choice)
                        {
                            case "1":
                                ShowBrowseProductsMenu(userEmail);
                                break;

                            case "2":
                                ShowOrderMenu(userEmail);
                                break;

                            case "0":
                                ShowMainMenu();
                                break;

                            default:
                                Console.WriteLine("Invalid choice, please try again.");
                                break;
                        }
                    }  
                }
                else
                {
                    _menuService.InputValidation("That e-mail could not be found, press any key to try again.");
                }
            }
            else
            {
                _menuService.InputValidation("You must enter an e-mail, press any key to try again.");
            }
        }
    }

    public void ShowBrowseProductsMenu(string userEmail)
    {
        var productList = _productService.GetAllProducts();

        ProfileEntity shoppingUser = _profileRepository.GetOne(x => x.User.Verification.Email == userEmail);
        Order order = _productFactories.CreateOrderEntity(shoppingUser.UserId);

        while (true)
        {
            Console.Clear();

            foreach (var product in productList)
            {
                Console.WriteLine($"------------------------------------------");
                Console.WriteLine($"Id:\t\t{product.Id}");
                Console.WriteLine($"Title:\t\t{product.Title}");
                Console.WriteLine($"Description:\t{product.Description}");
                Console.WriteLine($"Price:\t\t{product.Price}");
                Console.WriteLine($"Manufacturer:\t{product.ManufacturerName}");
                Console.WriteLine($"Category:\t{product.CategoryName}");
                Console.WriteLine($"\n------------------------------------------\n");
            }

            Console.Write("To place an order, please enter the Id of the item you would like to add to cart: ");
            var inputId = Console.ReadLine();

            if (!string.IsNullOrEmpty(inputId) && int.TryParse(inputId, out var selectedId))
            {

                if (_productRepository.Exists(x => x.Id == selectedId))
                {
                    Console.Clear();
                    Console.Write("How many of the selected item do you wish to purchase? ");
                    var inputAmount = Console.ReadLine();

                    if (!string.IsNullOrEmpty(inputAmount) && int.TryParse(inputAmount, out var selectedAmount))
                    {

                        OrderRow orderRow = _productFactories.CreateOrderRowEntity(order.Id, selectedId, selectedAmount);

                        if (orderRow != null)
                        {
                            ReturnToMainMenu("Order created successfully!\n\nPress any key to continue browsing.\nPress 0 to return to main menu.\n");
                        }
                        else
                        {
                            ReturnToMainMenu("Something went wrong with your order.\nPress 0 to return to main menu.");
                        }
                    }
                    else
                    {
                        _menuService.InputValidation("Please enter a valid amount.");
                    }
                }
                else
                {
                    _menuService.InputValidation("That Id could not be found, please try again.");
                }
            }
            else
            {
                _menuService.InputValidation("You must enter a valid Id, please try again.");
            }
        }
    }

    public void ShowOrderMenu(string userEmail)
    {
        VerificationEntity e = _verificationRepository.GetOne(x => x.Email == userEmail);
        UserEntity entity = _userRepository.GetOne(x => x.Id == e.UserId);

        var orderList = _orderRowRepository.GetAll();
        Console.Clear();

        foreach(var orderRow in orderList.Where(o => o.Order.UserId == entity.Id))
        {
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"Orderer:\t\t\t{entity.FirstName} {entity.LastName}");
            Console.WriteLine($"Order ID:\t\t\t{orderRow.OrderId}");
            Console.WriteLine($"Product Name:\t\t\t{orderRow.Product.Title}");
            Console.WriteLine($"Product Description\t\t{orderRow.Product.Description}");
            Console.WriteLine($"Product Price & Quantity\t{orderRow.Product.Price} x {orderRow.Amount}");
            Console.WriteLine($"Total Order Price\t\t{orderRow.Product.Price * orderRow.Amount}");
            Console.WriteLine("--------------------------------------------------");
        }
        Console.WriteLine("\nPress any key to return to product browsing.");
        Console.ReadKey();
    }

    private void UpdateAddress(ProfileEntity entity)
    {
        while (true)
        {
            Console.WriteLine("Enter a new city:");
            var city = Console.ReadLine()!;

            if (!city.IsNullOrEmpty())
            {
                Console.WriteLine("Enter a new street:");
                var street = Console.ReadLine()!;

                if (!street.IsNullOrEmpty())
                {
                    Console.WriteLine("Enter a postal code:");
                    var postalCode = Console.ReadLine()!;
                    if (!postalCode.IsNullOrEmpty())
                    {
                        AddressEntity entityToUpdate = _userFactories.GetOrCreateAddressEntity(street, postalCode, city);
                        entity.AddressId = entityToUpdate.Id;
                        _profileRepository.Update(entity, x => x.UserId == entity.UserId);

                        _menuService.InputValidation("Update successful!");
                        ShowEditUsersMenu(entity);
                    }
                }
            }
        }
    }

    public void ReturnToMainMenu(string message)
    {
        Console.Clear();
        Console.WriteLine(message);
        var choice = Console.ReadKey();
        if (choice.KeyChar == '0')
            ShowMainMenu();
    }

}
