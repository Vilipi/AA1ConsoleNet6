using aa1.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace aa1.Services
{
    public class PatientService
    {

        public static string _path = $@"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName}\Utils\specialists.json";

        //private static string _path = @"C:\Users\Vicentury\source\repos\aa1\aa1\Utils\specialists.json";

        public static List<Patient> patients = new List<Patient>
        {
            new Patient{Name = "Demo", LastName = "Demo", UserName = "demo", Password= "demo"} //usar de ejemplo
        };
        
        public static List<Appointment> appointments = new List<Appointment>();

        public int PatientMenu()
        {
            Console.WriteLine("Please write a number to choose an action:");
            Console.WriteLine(" - 1: Sign up");
            Console.WriteLine(" - 2: Sign in");
            Console.WriteLine(" - 3: Back");
            string login = Console.ReadLine();
            while(login != "1" && login != "2" && login != "3")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wrong number, try again");
                Console.ResetColor();
                login = Console.ReadLine();

            }

            if (login == "1")
            {
               return SignUp();
            }
            else if(login == "2")
            {
               var userlogged = SignIn();
               var menu = LoggedPatientMenu(userlogged);
               while (menu != 0)
               {
                    menu = LoggedPatientMenu(userlogged);
               }
                return 0;
            }
            else
            {
                return 0;
            }
        }

        private int SignUp()
        {
            var newUser = new Patient();
            Console.WriteLine("Name:");
            newUser.Name = Console.ReadLine();
            Console.WriteLine("LastName:");
            newUser.LastName = Console.ReadLine();
            Console.WriteLine("Gender:");
            newUser.Gender = Console.ReadLine();
            Console.WriteLine("Date of birth (dd/MM/yyyy):");
            //var db = DateTime.Parse(Console.ReadLine());
            var db = Console.ReadLine();
            DateTime i;
            bool result = DateTime.TryParse(db, out i);
            while (!result)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wrong date format\n");
                Console.ResetColor();
                Console.WriteLine("Please repeat your date of birth:");
                db = Console.ReadLine();
                result = DateTime.TryParse(db, out i);
            }
            newUser.BirthDate = i;

            Console.WriteLine("Email:");
            newUser.Email = Console.ReadLine();
            Console.WriteLine("UserName:");
            newUser.UserName = Console.ReadLine();

            Console.WriteLine("Password:");
            newUser.Password = Console.ReadLine();

            Console.WriteLine("Please repeat your password:");
            var passwordRepeated = Console.ReadLine();

            while (passwordRepeated != newUser.Password)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nYour passwords do not match!\n");
                Console.ResetColor();
                Console.WriteLine("Please repeat your password:");
                passwordRepeated = Console.ReadLine();
            }

            patients.Add(newUser);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nUser {newUser.UserName} created !\n");
            //Console.WriteLine($"Your bd is {newUser.BirthDate.ToString("dd/MM/yyyy")}");
            Console.WriteLine("Next time login using your username and password\n");
            Console.ResetColor();
            return 0;
        }
        private Patient SignIn()
        {
            Console.WriteLine("Username: ");
            var inputUsername = Console.ReadLine();
            var existingUser = patients.Find(x => x.UserName == inputUsername);
            while(existingUser == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("User not found, try a different username");
                Console.ResetColor();
                Console.WriteLine("Username: ");
                inputUsername = Console.ReadLine();
                existingUser = patients.Find(x => x.UserName == inputUsername);
            }
            Console.WriteLine("Password: ");
            var inputPassword = Console.ReadLine();
            while(inputPassword != existingUser.Password)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wrong password. Try again.");
                Console.ResetColor();
                Console.WriteLine("Password: ");
                inputPassword = Console.ReadLine();
                existingUser = patients.Find(x => x.UserName == inputUsername);
            }
            Console.WriteLine($"\nHi {existingUser.Name} {existingUser.LastName}");

            return existingUser;
        }
        private int LoggedPatientMenu(Patient patient) 
        {
            Console.WriteLine("Please write one of the following numbers:");
            Console.WriteLine(" - 1: Book new appointment");
            Console.WriteLine(" - 2: View my appointments");
            Console.WriteLine(" - 3: Back");
            string loggedPatientAction = Console.ReadLine();

            while (loggedPatientAction != "1" && loggedPatientAction != "2" && loggedPatientAction != "3")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wrong number, try again");
                Console.ResetColor();
                loggedPatientAction = Console.ReadLine();

            }
            if (loggedPatientAction == "1")
            {
                Console.WriteLine("Please choose one of the following specialities:");
                Console.WriteLine(" - 1: General");
                Console.WriteLine(" - 2: Dentist"); 
                Console.WriteLine(" - 3: Ophthalmologist");
                Console.WriteLine(" - 4: Dermatologist");

                string specialistElection = Console.ReadLine();

                while (specialistElection != "1" && specialistElection != "2" && specialistElection != "3" && specialistElection != "4")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Wrong number, try again");
                    Console.ResetColor();
                    specialistElection = Console.ReadLine();
                }
                var specilasitsString = GetSpecialistListFromFile();
                var specialiast = DeserializeJsonFile(specilasitsString);

                var newAppointment = new Appointment(); // creando nuevo appointment
                newAppointment.Id = appointments.Count + 1;
                newAppointment.Patient = patient;
                newAppointment.IsCompleted = false;
                newAppointment.Price = 0;
                newAppointment.SpecialistComment = null;


                switch (specialistElection)
                {
                    case "1":
                        newAppointment.Name = $"{patient.UserName}-General-Book";
                        newAppointment.Specialist = specialiast.Find(e => e.Speciality == "General");
                        newAppointment.AppointmentCreationDate = DateTime.Now;
                        Console.WriteLine("General appointment created");
                        Console.WriteLine("You'll be receiving an email shortly with further details");
                        break;

                    case "2":
                        newAppointment.Name = $"{patient.UserName}-Dentist-Book";
                        newAppointment.Specialist = specialiast.Find(e => e.Speciality == "Dentist");
                        newAppointment.AppointmentCreationDate = DateTime.Now;
                        Console.WriteLine("Dentist appointment created");
                        Console.WriteLine("You'll be receiving an email shortly with further details");
                        break;

                    case "3":
                        newAppointment.Name = $"{patient.UserName}-Ophthalmologist-Book";
                        newAppointment.Specialist = specialiast.Find(e => e.Speciality == "Ophthalmologist");
                        newAppointment.AppointmentCreationDate = DateTime.Now;
                        Console.WriteLine("Ophthalmologist appointment created");
                        Console.WriteLine("You'll be receiving an email shortly with further details");
                        break;
                    case "4":
                        newAppointment.Name = $"{patient.UserName}-Dermatologist-Book";
                        newAppointment.Specialist = specialiast.Find(e => e.Speciality == "Dermatologist");
                        newAppointment.AppointmentCreationDate = DateTime.Now;
                        Console.WriteLine("Dermatologist appointment created");
                        Console.WriteLine("You'll be receiving an email shortly with further details");
                        break;
                }
                appointments.Add(newAppointment); // adding modified appointment
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Appointment booked!\n");
                Console.ResetColor();
                return 1;
            }
            else if(loggedPatientAction == "2")
            {
                var patientAppointments = appointments.FindAll(e => e.Patient == patient && e.IsCompleted == false).ToList();
                if (patientAppointments.Count == 0 ) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No appointments booked\n");
                    Console.ResetColor();
                }
                else
                {
                    patientAppointments.ForEach(e => Console.WriteLine($"- {e.Id} - {e.Name}"));
                    Console.WriteLine("");
                    Console.WriteLine("Would you like to cancel an appointment?");
                    Console.WriteLine(" - 1: Yes");
                    Console.WriteLine(" - 2: No");

                    string cancelRespone = Console.ReadLine();
                    while (cancelRespone != "1" && cancelRespone != "2")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Wrong number, try again");
                        Console.ResetColor();
                        cancelRespone = Console.ReadLine();
                    }
                    if (cancelRespone == "1")
                    {
                        Console.WriteLine("Choose the appointment you want to cancel");
                         

                        for (int i = 0; i < patientAppointments.Count; i++)
                        {
                            Console.WriteLine($" - {i}: {patientAppointments[i].Name}");
                        }
                        var appointmentToCancel = Console.ReadLine();
                        int appointmentToCancelInt;
                        bool appointmentToCancelBool = int.TryParse(appointmentToCancel, out appointmentToCancelInt);
                        //&& Int32.Parse(appointmentToCancel) >= 0 && Int32.Parse(appointmentToCancel) < patientAppointments.Count
                        while (appointmentToCancelBool == false)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Wrong number, try again");
                            Console.ResetColor();
                            appointmentToCancel = Console.ReadLine();
                            appointmentToCancelBool = int.TryParse(appointmentToCancel, out appointmentToCancelInt);
                        }
                        var appointmentToCancelIndex = Int32.Parse(appointmentToCancel);    
                        while (appointmentToCancelIndex < 0 || appointmentToCancelIndex > patientAppointments.Count -1)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Wrong number, try again");
                            Console.ResetColor();
                            appointmentToCancelIndex = Int32.Parse(Console.ReadLine());
                        }
                        patientAppointments[appointmentToCancelIndex].IsCompleted = true;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Appointment cancelled");
                        Console.ResetColor();
                    }
                }
                return 1;
            }
            else
            {
                return 0;
            }
        }

        // Info from JSON
        private string GetSpecialistListFromFile()
        {
            string specilistsJsonFromFile;
            using (var reader = new StreamReader(_path))
            {
                specilistsJsonFromFile = reader.ReadToEnd();
            }
            return specilistsJsonFromFile;
        }
        private List<Specialist> DeserializeJsonFile(string specilistsJsonFromFile)
        {
            return JsonConvert.DeserializeObject<List<Specialist>>(specilistsJsonFromFile);
        }
    }
}
