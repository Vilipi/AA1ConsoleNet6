using aa1.Models;
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

        public int SpecialistMenu() 
        {
            var specialistLogged = SignIn();
            var menu = LoggedSpecialistMenu(specialistLogged);
            while (menu != 0)
            {
                menu = LoggedSpecialistMenu(specialistLogged);
            }
            return 0;

        }

        private Specialist SignIn()
        {
            var specilasitsString = _jsonService.GetListFromFile("specialist");
            var specialists = _jsonService.DeserializeSpecialistsJsonFile(specilasitsString);
            Console.WriteLine("Pelase login using your credentials\n");

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
            Console.WriteLine($"\nHi {existingSpecialist.Name} {existingSpecialist.LastName}");

            return existingSpecialist;
        }

        private int LoggedSpecialistMenu(Specialist specialist)
        {
            var appointmentString = _jsonService.GetListFromFile("appointment");
            var appointments = _jsonService.DeserializeAppointmentJsonFile(appointmentString);

            Console.WriteLine("\nPlease write one of the following numbers:");
            Console.WriteLine(" - 1: List my appointments");
            Console.WriteLine(" - 2: Modify my appointments");
            Console.WriteLine(" - 3: Back");
            string loggedSpecialistAction = Console.ReadLine();

            while (loggedSpecialistAction != "1" && loggedSpecialistAction != "2" && loggedSpecialistAction != "3")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wrong number, try again");
                Console.ResetColor();
                loggedSpecialistAction = Console.ReadLine();
            }

            switch (loggedSpecialistAction)
            {
                case "1":
                    listAppointments(appointments, specialist);
                    return 1;
                    break;
                case "2":
                    modifyAppointment(appointments, specialist);
                    return 1;
                    break;
                case "3":
                    return 0;
                    break;
                default:
                    return 0;
            }
        }

        private void listAppointments(List<Appointment> appointments, Specialist specialist)
        {
            var specialistAppointments = appointments.FindAll(e => e.Specialist.UserName == specialist.UserName).ToList();
            int count = 1;
            specialistAppointments.ForEach(e =>
            {
                Console.WriteLine($"\n· {count}:");
                Console.WriteLine($" - Id: {e.Id}");
                string status = e.IsCompleted == true ? "finished": "active";
                Console.WriteLine($" - Status: {status}");
                Console.WriteLine($" - Name: {e.Name}");
                Console.WriteLine($" - Patient: {e.Patient.Name} {e.Patient.LastName}");
                string price = e.Price == null ? "Pending" : $"{e.Price}";
                Console.WriteLine($" - Price: {price}");
                string comments = e.SpecialistComment == null ? "Pending" : $"{e.SpecialistComment}";
                Console.WriteLine($" - Comments: {comments}");
                count++;
            });
        }

        private int modifyAppointment(List<Appointment> appointments, Specialist specialist)
        {
            var specialistAppointments = appointments.FindAll(e => e.Specialist.UserName == specialist.UserName && e.IsCompleted == false).ToList();

            Console.WriteLine("Please write the ID number of the appointment you want to edit");
            string appointmentInput = Console.ReadLine();
            int appointmentToModifylInt;
            bool appointmentToModifyBool = int.TryParse(appointmentInput, out appointmentToModifylInt);

            appointmentToModifylInt = appointmentToModifyBool == true ? Int32.Parse(appointmentInput) : 0;

            var appointmentSelected = specialistAppointments.Find(e => e.Id == appointmentToModifylInt);

            while (appointmentSelected ==  null)
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
            string  apointmentModification = Console.ReadLine();

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

                        int index = appointments.FindIndex(e => e.Id == appointmentSelected.Id);
                        appointments[index] = appointmentSelected;
                        _jsonService.SerealizeAppointmentJsonFile(appointments); //modifiying appointment file

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nPrice changed\n");
                        Console.ResetColor();
                    }
                    return 1;
                    break;
                case "2":
                    return 1;
                    break;
                case "3":
                    return 1;
                    break;
                default:
                    return 1;
                    break;
            }
        }
    }
}
