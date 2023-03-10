using aa1.Models;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aa1.Services
{
    public class SpecialistService
    {
        JsonService _jsonService = new JsonService();
        ILogger _logger = LoggerFactory.Create(builder => builder.AddNLog()).CreateLogger<Program>();


        public int SpecialistMenu() 
        {
            try
            {
                var specialistLogged = SignIn();
                var menu = LoggedSpecialistMenu(specialistLogged);
                while (menu != 0)
                {
                    menu = LoggedSpecialistMenu(specialistLogged);
                }
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message);
                return 0;
            }
        }

        private Specialist SignIn()
        {

            var specilasitsString = _jsonService.GetListFromFile("specialist");
            var specialists = _jsonService.DeserializeSpecialistsJsonFile(specilasitsString);
            Console.WriteLine("Please login using your credentials\n");

            Console.WriteLine("Username: ");
            var inputUsername = Console.ReadLine();
            var existingSpecialist = specialists.Find(x => x.UserName == inputUsername);
            while (existingSpecialist == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("User not found, try a different username");
                Console.ResetColor();
                Console.WriteLine("Username: ");
                inputUsername = Console.ReadLine();
                existingSpecialist = specialists.Find(x => x.UserName == inputUsername);
            }
            Console.WriteLine("Password: ");
            var inputPassword = Console.ReadLine();
            while (inputPassword != existingSpecialist.Password)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wrong password. Try again.");
                Console.ResetColor();
                Console.WriteLine("Password: ");
                inputPassword = Console.ReadLine();
                existingSpecialist = specialists.Find(x => x.UserName == inputUsername);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nHi {existingSpecialist.Name} {existingSpecialist.LastName}");
            Console.ResetColor();
            _logger.LogInformation($"Specialist {existingSpecialist.Name} {existingSpecialist.LastName} logged in");
            return existingSpecialist;
        }

        private int LoggedSpecialistMenu(Specialist specialist)
        {
            var appointmentString = _jsonService.GetListFromFile("appointment");
            var appointments = _jsonService.DeserializeAppointmentJsonFile(appointmentString);

            Console.WriteLine("\nPlease write one of the following numbers:");
            Console.WriteLine(" - 1: List my appointments");
            Console.WriteLine(" - 2: Search appointment");
            Console.WriteLine(" - 3: Modify my appointments");
            Console.WriteLine(" - 4: Back");
            string loggedSpecialistAction = Console.ReadLine();

            while (loggedSpecialistAction != "1" && loggedSpecialistAction != "2" && loggedSpecialistAction != "3" && loggedSpecialistAction != "4")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wrong number, try again");
                Console.ResetColor();
                loggedSpecialistAction = Console.ReadLine();
            }

            switch (loggedSpecialistAction)
            {
                case "1":
                    ListAppointments(appointments, specialist);
                    return 1;
                case "2":
                    SearchAppointment(appointments, specialist);
                    return 1;
                case "3":
                    ModifyAppointment(appointments, specialist);
                    return 1;
                default:
                    return 0;
            }
        }

        private void ListAppointments(List<Appointment> appointments, Specialist specialist)
        {
            var specialistAppointments = appointments.FindAll(e => e.Specialist.UserName == specialist.UserName).ToList();
            if(specialistAppointments.Count < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nYou do not have any appointments");
                Console.ResetColor();
            }
            else
            {
                string currEnv = Environment.GetEnvironmentVariable("currency");
                var currency = currEnv == "eu" ? "EUR" : "$";

                var table = new Table();
                table.BorderColor(Color.SkyBlue2);
                table.AddColumn(new TableColumn("Id"));
                table.AddColumn(new TableColumn("Status"));
                table.AddColumn(new TableColumn("Name"));
                table.AddColumn(new TableColumn("Patient"));
                table.AddColumn(new TableColumn($"Price"));
                table.AddColumn(new TableColumn("Comments"));

                specialistAppointments.ForEach(e =>
                {
                    string status = e.IsCompleted == true ? "finished" : "active";
                    string price = e.Price == null ? "Pending" : $"{e.Price} {currency}";
                    string comments = e.SpecialistComment == null ? "Pending" : $"{e.SpecialistComment}";
                    table.AddRow($"{e.Id}", $"{status}",$"{e.Name}", $"{e.Patient.Name}{e.Patient.LastName}", $"{price}", $"{comments}");
                });
                AnsiConsole.Write(table);
            }

        }
        private int ModifyAppointment(List<Appointment> appointments, Specialist specialist)
        {
            var specialistAppointments = appointments.FindAll(e => e.Specialist.UserName == specialist.UserName && e.IsCompleted == false).ToList();
            if(specialistAppointments.Count < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nYou do not have any appointments");
                Console.ResetColor();
                return 1;
            }
            else
            {
                Console.WriteLine("Please write the ID number of the appointment you want to edit");
                string appointmentInput = Console.ReadLine();
                int appointmentToModifylInt;
                bool appointmentToModifyBool = int.TryParse(appointmentInput, out appointmentToModifylInt);

                appointmentToModifylInt = appointmentToModifyBool == true ? Int32.Parse(appointmentInput) : 0;

                var appointmentSelected = specialistAppointments.Find(e => e.Id == appointmentToModifylInt);

                while (appointmentSelected == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Wrong number id, try again\n");
                    Console.ResetColor();
                    specialistAppointments.ForEach(e =>
                    {
                        Console.WriteLine($" - Id: {e.Id}: {e.Name}");
                    });
                    appointmentInput = Console.ReadLine();
                    appointmentToModifyBool = int.TryParse(appointmentInput, out appointmentToModifylInt);
                    appointmentToModifylInt = appointmentToModifyBool == true ? Int32.Parse(appointmentInput) : 0;
                    appointmentSelected = specialistAppointments.Find(e => e.Id == appointmentToModifylInt);
                }

                Console.WriteLine("Appointment selected. Choose what you want to modify:");
                Console.WriteLine(" - 1: Price appointment");
                Console.WriteLine(" - 2: Comment appointment");
                Console.WriteLine(" - 3: Complete appointment");
                Console.WriteLine(" - 4: Back");
                string apointmentModification = Console.ReadLine();

                while (apointmentModification != "1" && apointmentModification != "2" && apointmentModification != "3" && apointmentModification != "4")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Wrong number, try again\n");
                    Console.ResetColor();
                    apointmentModification = Console.ReadLine();
                }

                switch (apointmentModification)
                {
                    case "1":
                        if (appointmentSelected.Price != 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("This appointment already has a price\n");
                            Console.ResetColor();
                            return 0;
                        }
                        else
                        {
                            Console.WriteLine("Add price:");
                            string newPriceString = Console.ReadLine();
                            decimal newPriceDec;
                            var isPriceCorrect = decimal.TryParse(newPriceString, out newPriceDec);
                            while (isPriceCorrect == false)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Wrong price, try again");
                                Console.ResetColor();
                                newPriceString = Console.ReadLine();
                                isPriceCorrect = decimal.TryParse(newPriceString, out newPriceDec);
                            }
                            appointmentSelected.Price = decimal.Parse(newPriceString);

                            int index1 = appointments.FindIndex(e => e.Id == appointmentSelected.Id);
                            appointments[index1] = appointmentSelected;
                            _jsonService.SerealizeAppointmentJsonFile(appointments); //modifiying appointment file

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nPrice changed\n");
                            Console.ResetColor();
                        }
                        return 1;

                    case "2":
                        if (appointmentSelected.SpecialistComment != null)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("This appointment already has a comment\n");
                            Console.ResetColor();
                            return 0;
                        }
                        else
                        {
                            Console.WriteLine("Add comment:");
                            string newCommentString = Console.ReadLine();
                            while (newCommentString == null)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Wrong comment, try again");
                                Console.ResetColor();
                                newCommentString = Console.ReadLine();
                            }
                            appointmentSelected.SpecialistComment = newCommentString;

                            int index2 = appointments.FindIndex(e => e.Id == appointmentSelected.Id);
                            appointments[index2] = appointmentSelected;
                            _jsonService.SerealizeAppointmentJsonFile(appointments); //modifiying appointment file

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nComment changed\n");
                            Console.ResetColor();
                        }
                        return 1;

                    case "3":
                        appointmentSelected.IsCompleted = true;
                        int index = appointments.FindIndex(e => e.Id == appointmentSelected.Id);
                        appointments[index] = appointmentSelected;
                        _jsonService.SerealizeAppointmentJsonFile(appointments); //modifiying appointment file

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nAppointment competed!\n");
                        Console.ResetColor();
                        return 1;
                    default:
                        return 1;
                }
            }
            
        }

        private int SearchAppointment(List<Appointment> appointments, Specialist specialist)
        {
            var specialistAppointments = appointments.FindAll(e => e.Specialist.UserName == specialist.UserName).ToList();
            Console.WriteLine("Introduce the name of the patient to find their appointments");
            string patientNameInput = Console.ReadLine();

            var patientAppointments = new List<Appointment>();
            specialistAppointments.ForEach(e =>
            {
                if (e.Patient.Name.Contains(patientNameInput))
                {
                    patientAppointments.Add(e);
                }
            });
            if(patientAppointments.Count < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Appointments not found\n");
                Console.ResetColor();
            }
            else
            {
                patientAppointments.ForEach(e =>
                {
                    Console.WriteLine($" - Id: #{e.Id}");
                    Console.WriteLine($" - Name: {e.Name}");
                    Console.WriteLine($" - Completed: #{e.IsCompleted}\n");
                });
            }
            return 1;
        }
    }
}
