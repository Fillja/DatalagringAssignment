using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Respositories;
using Infrastructure.Services;
using Microsoft.IdentityModel.Tokens;

namespace ConsoleApp.Services;

public class MainMenuService(UserService userService, UserRepository userRepository, VerificationRepository verificationRepository, ProfileRepository profileRepository, UserFactories userFactories) 
{
    private readonly UserService _userService = userService;

    private readonly UserRepository _userRepository = userRepository;
    private readonly VerificationRepository _verificationRepository = verificationRepository;
    private readonly ProfileRepository _profileRepository = profileRepository;

    private readonly UserFactories _userFactories = userFactories;

    public void ShowMainMenu()
    {
        Console.Clear();
        Console.WriteLine("-----------------------");
        Console.WriteLine("MAIN MENU");
        Console.WriteLine("-----------------------");
        Console.WriteLine("Please, choose an option:");
        Console.WriteLine("1.\t Add a user");
        Console.WriteLine("2.\t Show and inspect users");
        Console.WriteLine("0.\t Exit program.");

        var input = Console.ReadLine();

        switch (input)
        {
            case "1":
                ShowAddUserMenu();
                break;

            case "2":
                ShowAllUsersMenu();
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
        while (true)
        {
            var userReg = new UserRegistrationForm();
            bool emailCheck = true;
            Console.Clear();

            SetUserProperties(userReg, () => userReg.FirstName, value => userReg.FirstName = value, "Enter first name: ", "First name may not be empty!");

            SetUserProperties(userReg, () => userReg.LastName, value => userReg.LastName = value, "Enter surname: ", "Surname may not be empty!");

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

            SetUserProperties(userReg, () => userReg.Password, value => userReg.Password = value, "Enter password: ", "Password may not be empty!");

            SetUserProperties(userReg, () => userReg.Street, value => userReg.Street = value, "Enter streetname: ", "Streetname may not be empty!");

            SetUserProperties(userReg, () => userReg.PostalCode, value => userReg.PostalCode = value, "Enter postal code: ", "Postal code may not be empty!");

            SetUserProperties(userReg, () => userReg.City, value => userReg.City = value, "Enter city: ", "City may not be empty!");

            var result = _userService.CreateUser(userReg);

            Console.Clear();
            if (result)
                Console.WriteLine("User added successfully!");
            else
                Console.WriteLine("Something went wrong, please try again.");

            Console.ReadKey();
            ShowMainMenu();
            break;
        }
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
            Console.WriteLine("---------------------------\n\n");
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
            ShowEditAndDeleteMenu(entity);
        }
    }

    public void ShowEditAndDeleteMenu(ProfileEntity entity)
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

                        Console.Clear();
                        Console.WriteLine("Update successful!");
                        Console.ReadKey();
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

                        Console.Clear();
                        Console.WriteLine("Update successful!");
                        Console.ReadKey();
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

                            Console.Clear();
                            Console.WriteLine("Update successful!");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("\nA user with that E-mail already exists.\nPress any key to try again.");
                            Console.ReadKey();
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nEmail may not be empty.\nPress any key to try again.");
                        Console.ReadKey();
                    }
                    break;


                case "4":

                    UpdateAddress(entity);
                    break;

                case "9":

                    _verificationRepository.Delete(x => x.UserId == entity.UserId);
                    _userRepository.Delete(x => x.Id == entity.UserId);
                    _profileRepository.Delete(x => x.UserId == entity.UserId);

                    Console.Clear();
                    Console.WriteLine("Successfully deleted the user!");
                    Console.ReadKey();
                    ShowMainMenu();
                    break;

                case "0":
                    ShowMainMenu();
                    break;
            }
        }
    }

    public void ReturnToMainMenu(string message)
    {
        Console.WriteLine(message);
        var choice = Console.ReadKey();
        if (choice.KeyChar == '0')
            ShowMainMenu();
    }

    public void SetUserProperties(UserRegistrationForm user, Func<string> getProperty, Action<string> setProperty, string message, string errorMessage)
    {
        while (true)
        {
            Console.Write($"{message}");
            var input = Console.ReadLine()!;
            if (!string.IsNullOrEmpty(input))
            {
                setProperty(input);
                break;
            }
            else
            {
                Console.WriteLine($"{errorMessage}\n");
            }
        }
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
                        AddressEntity entityToUpdate = _userFactories.GetOrCreateAddressEntity(street, city, postalCode);
                        entity.AddressId = entityToUpdate.Id;
                        _profileRepository.Update(entity, x => x.UserId == entity.UserId);

                        Console.Clear();
                        Console.WriteLine("Update successful!");
                        Console.ReadKey();
                        ShowEditAndDeleteMenu(entity);
                    }
                }
            }
        }
    }


}
