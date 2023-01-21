using aa1.Services;
using System.Globalization;

int exit = 0;

PatientService patientService = new PatientService();
SpecialistService specialistService = new SpecialistService();
PublicService publicService = new PublicService();

while (exit == 0)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("\nAA1 Medical Center");
    Console.ResetColor();
    Console.WriteLine("Please, type a number to choose an option");
    Console.WriteLine(" - 1: Patient");
    Console.WriteLine(" - 2: Specialist");
    Console.WriteLine(" - 3: Our team");
    Console.WriteLine(" - 4: Exit");
    string option = Console.ReadLine();
    
    if (option == "1")
    {
        var patientMenu = patientService.PatientMenu();
        exit = patientMenu;
    }
    else if (option == "2")
    {
        var specialistMenu = specialistService.SpecialistMenu();
        exit = specialistMenu;
    }
    else if (option == "3")
    {
        var publicMenu = publicService.PublicMenu();
        exit = publicMenu;
    }
    else if (option == "4")
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
