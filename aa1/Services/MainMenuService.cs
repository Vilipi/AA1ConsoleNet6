using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Spectre.Console.Color;

namespace aa1.Services
{
    public class MainMenuService
    {
        public void MainMenu()
        {
            var _logger = LoggerFactory.Create(builder => builder.AddNLog()).CreateLogger<Program>();

            int exit = 0;

            PatientService patientService = new PatientService();
            SpecialistService specialistService = new SpecialistService();
            PublicService publicService = new PublicService();

            _logger.LogInformation("App started");

            AnsiConsole.Write(new FigletText("Medical Center").Color(Color.Salmon1));

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
            }
        }
    }
}
