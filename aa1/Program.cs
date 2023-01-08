using aa1.Services;
using System.Globalization;

int exit = 0;

PatientService patientService = new PatientService();

while (exit == 0)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("\nAA1 Medical Center");
    Console.ResetColor();
    Console.WriteLine("Please, type a number to choose an option");
    Console.WriteLine(" - 1: patient");
    Console.WriteLine(" - 2: specialist");
    Console.WriteLine(" - 3: exit");
    string option = Console.ReadLine();
    
    if (option == "1")
    {
        var patientMenu = patientService.PatientMenu();
        exit = patientMenu;
    }
    else if (option == "2")
    {
        Console.WriteLine("Hi Specialist");
    }
    else if (option == "3")
    {
        Console.WriteLine("Bye!");
        exit = 1;
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Please try again\n");
        Console.ResetColor();

    }
};
