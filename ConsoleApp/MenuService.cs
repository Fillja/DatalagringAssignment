using Infrastructure.Dtos;
using Infrastructure.Services;

namespace ConsoleApp;

public class MenuService(UserService userService)
{
    private readonly UserService _userService = userService;

    public void ShowUserMenu()
    {
        //var UserReg = new UserRegistrationForm();

        //Console.Clear();


        //Console.Write("First Name: ");
        //UserReg.FirstName = Console.ReadLine()!;

        //Console.Write("Last Name: ");
        //UserReg.LastName = Console.ReadLine()!;

        //Console.Write("Email: ");
        //UserReg.Email = Console.ReadLine()!;

        //Console.Write("Password: ");
        //UserReg.Password = Console.ReadLine()!;

        //Console.Write("Street: ");
        //UserReg.Street = Console.ReadLine()!;

        //Console.Write("City: ");
        //UserReg.City = Console.ReadLine()!;

        //Console.Write("Postal Code: ");
        //UserReg.PostalCode = Console.ReadLine()!;

        //_userService.CreateUser(UserReg);

        //Console.ReadKey();

        

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
}
