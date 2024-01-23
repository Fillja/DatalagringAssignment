using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Respositories;
using Infrastructure.Services;
using Microsoft.IdentityModel.Tokens;

namespace ConsoleApp;

public class MenuService(UserService userService, UserRepository userRepository, RoleRepository roleRepository, AddressRepository addressRepository, VerificationRepository verificationRepository, ProfileRepository profileRepository, UserFactories userFactories)
{
    private readonly UserService _userService = userService;
    private readonly UserRepository _userRepository = userRepository;
    private readonly RoleRepository _roleRepository = roleRepository;
    private readonly AddressRepository _addressRepository = addressRepository;
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
        Console.WriteLine("0.\t Exit Program.");

        var input = Console.ReadLine();

        switch(input)
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
        while(true)
        {
            var userReg = new UserRegistrationForm();
            bool emailCheck = true;
            Console.Clear();


            Console.Write("First Name: ");
            userReg.FirstName = Console.ReadLine()!;

            Console.Write("Last Name: ");
            userReg.LastName = Console.ReadLine()!;

            while (emailCheck)
            {
                Console.Write("Email: ");
                userReg.Email = Console.ReadLine()!;
                if (!_verificationRepository.Exists(x => x.Email == userReg.Email))
                {
                    emailCheck = false;
                }
                else
                {
                    Console.WriteLine("\nA user with that E-mail already exists!");
                    ReturnToMainMenu("Press any key to try again, or press 0 to return to the main menu.");
                }
            }

            Console.Write("Password: ");
            userReg.Password = Console.ReadLine()!;

            Console.Write("Street: ");
            userReg.Street = Console.ReadLine()!;

            Console.Write("Postal Code: ");
            userReg.PostalCode = Console.ReadLine()!;

            Console.Write("City: ");
            userReg.City = Console.ReadLine()!;

            var result = _userService.CreateUser(userReg);

            Console.Clear();
            if (result)
                Console.WriteLine("SUCCESS!");
            else
                Console.WriteLine("FAIL!");

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
            User selectedUser = userList.ElementAt(result - 1);
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
            Console.WriteLine("Enter the value of the property you would like to update.");
            Console.WriteLine("Enter 9 to delete the user.");
            Console.WriteLine("Enter 0 to return to main menu.");
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
                        VerificationEntity entityToUpdate = _verificationRepository.GetOne(x => x.UserId == entity.UserId);
                        entityToUpdate.Email = email;
                        _verificationRepository.Update(entityToUpdate, x => x.UserId == entityToUpdate.UserId);

                        Console.Clear();
                        Console.WriteLine("Update successful!");
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
