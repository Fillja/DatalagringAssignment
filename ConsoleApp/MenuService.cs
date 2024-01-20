using Infrastructure.Dtos;
using Infrastructure.Respositories;
using Infrastructure.Services;

namespace ConsoleApp;

public class MenuService(UserService userService, VerificationRepository verificationRepository)
{
    private readonly UserService _userService = userService;
    private readonly VerificationRepository _verificationRepository = verificationRepository;

    public void ShowMainMenu()
    {
        Console.Clear();
        Console.WriteLine("-----------------------");
        Console.WriteLine("Hello!");
        Console.WriteLine("-----------------------");
        Console.WriteLine("Please, choose an option:");
        Console.WriteLine("1.\t Add a user \n2.\t Show all users.");

        var input = Console.ReadLine();

        switch(input)
        {
            case "1":
                ShowAddUserMenu();
                break;

            case "2":
                ShowAllUsersMenu();
                break;

        }
    }
    public void ShowAddUserMenu()
    {
        var userReg = new UserRegistrationForm();
        bool emailCheck = true;
        Console.Clear();


        Console.Write("First Name: ");
        userReg.FirstName = Console.ReadLine()!;

        Console.Write("Last Name: ");
        userReg.LastName = Console.ReadLine()!;

        Console.Write("Email: ");
        userReg.Email = Console.ReadLine()!;

        while (emailCheck)
        {
            Console.Write("Email: ");
            userReg.Email = Console.ReadLine()!;
            if (!_verificationRepository.Exists(x => x.Email == userReg.Email))
            {
                emailCheck = false;
            }
            //else
            //{
            //    ReturnToMainMenu("En användare med den e-postadressen existerar redan! \nTryck på valfri knapp för att försöka igen. \nTryck på 0 för att återgå till huvudmenyn.");
            //}
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
    }

    public void ShowAllUsersMenu()
    {
        var userList = _userService.GetAllUsers();
        Console.Clear();

        Console.WriteLine("DISPLAYING ALL USERS:\n\n ");
        foreach (var user in userList)
        {
            Console.WriteLine(user.FirstName);
            Console.WriteLine(user.LastName);
            Console.WriteLine(user.Email);
            Console.WriteLine(user.City);
            Console.WriteLine(user.Street);
            Console.WriteLine(user.PostalCode + "\n\n");
        }
        Console.ReadKey();
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
