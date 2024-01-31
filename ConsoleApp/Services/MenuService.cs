namespace ConsoleApp.Services;

public class MenuService
{
    public void SetUserProperties(Action<string> setProperty, string message, string errorMessage)
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

    public void SetProductProperties(Action<string> setProperty, string message, string errorMessage)
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

    public void SetPrice(Action<decimal> setProperty, string message, string errorMessage)
    {
        while (true)
        {
            Console.Write($"{message}");
            var input = Console.ReadLine()!;
            if (!string.IsNullOrEmpty(input))
            {
                setProperty(Decimal.Parse(input));
                break;
            }
            else
            {
                Console.WriteLine($"{errorMessage}\n");
            }
        }
    }

    public void InputValidation(string message)
    {
        Console.Clear();
        Console.WriteLine(message);
        Console.ReadKey();
    }

}
