using aa1.Models;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Spectre.Console;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Xml.Linq;


namespace aa1.Services
{
    public class PatientService
    {
        JsonService _jsonService = new JsonService();
        ILogger _logger = LoggerFactory.Create(builder => builder.AddNLog()).CreateLogger<Program>();

        public int PatientMenu()
        {
            try
            {
                Console.WriteLine("Please write a number to choose an action:");
                Console.WriteLine(" - 1: Sign up");
                Console.WriteLine(" - 2: Sign in");
                Console.WriteLine(" - 3: Back");
                string login = Console.ReadLine();
                while (login != "1" && login != "2" && login != "3")
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
                else if (login == "2")
                {
                    var userLogged = SignIn();
                    var menu = LoggedPatientMenu(userLogged);
                    while (menu != 0)
                    {
                        menu = LoggedPatientMenu(userLogged);
                    }
                    return 0;
                }
                else
                {
                    return 0;
                }
            }
            catch(Exception ex)
            {
                _logger.LogDebug(ex.Message);
                return 0;
            }
           
        }
        private int SignUp()
        {
            var patientsString = _jsonService.GetListFromFile("patients");
            var patients = _jsonService.DeserializePatientsJsonFile(patientsString);

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

            var today = DateTime.Today;
            var age = today.Year - i.Year;
            if (i.Date > today.AddYears(-age)) age--;  
            newUser.IsUnderage = age >= 18 ? false: true;

            Console.WriteLine("Email:");
            var email = Console.ReadLine();

            while (!new EmailAddressAttribute().IsValid(email))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wrong email format, try again");
                Console.ResetColor();
                email = Console.ReadLine();
            }
            newUser.Email = email;

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
            newUser.Appointments = new List<int>();
            patients.Add(newUser);
            _jsonService.SerealizePatientsJsonFile(patients);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nUser {newUser.UserName} created !\n");
            //Console.WriteLine($"Your bd is {newUser.BirthDate.ToString("dd/MM/yyyy")}");
            Console.WriteLine("Next time login using your username and password\n");
            Console.ResetColor();
            return 0;
        }
        private Patient SignIn()
        {
            var patientsString = _jsonService.GetListFromFile("patients");
            var patients = _jsonService.DeserializePatientsJsonFile(patientsString);

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
            _logger.LogInformation($"Patient {existingUser.Name} {existingUser.LastName} logged in");
            return existingUser;
        }
        private int LoggedPatientMenu(Patient patient) 
        {
            var patientsString = _jsonService.GetListFromFile("patients");
            var patients = _jsonService.DeserializePatientsJsonFile(patientsString);

            var appointmentString = _jsonService.GetListFromFile("appointment");
            var appointments = _jsonService.DeserializeAppointmentJsonFile(appointmentString);


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
                string specialistElection;
                if (!patient.IsUnderage)
                {
                    Console.WriteLine("Please choose one of the following specialities:");
                    Console.WriteLine(" - 1: General");
                    Console.WriteLine(" - 2: Dentist");
                    Console.WriteLine(" - 3: Ophthalmologist");
                    Console.WriteLine(" - 4: Dermatologist");

                    specialistElection = Console.ReadLine();

                    while (specialistElection != "1" && specialistElection != "2" && specialistElection != "3" && specialistElection != "4")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Wrong number, try again");
                        Console.ResetColor();
                        specialistElection = Console.ReadLine();
                    }
                }
                else
                {
                    Console.WriteLine("Please choose one of the following specialities:");
                    Console.WriteLine(" - 1: General");
                    Console.WriteLine(" - 2: Dentist");
                    Console.WriteLine(" - 3: Ophthalmologist");
                    Console.WriteLine(" - 4: Dermatologist");
                    Console.WriteLine(" - 5: Pediatrician");

                    specialistElection = Console.ReadLine();

                    while (specialistElection != "1" && specialistElection != "2" && specialistElection != "3" && specialistElection != "4" && specialistElection != "5")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Wrong number, try again");
                        Console.ResetColor();
                        specialistElection = Console.ReadLine();
                    }
                }
           
                var specilasitsString = _jsonService.GetListFromFile("specialist");
                var specialist = _jsonService.DeserializeSpecialistsJsonFile(specilasitsString);

                var newAppointment = new Appointment(); // creando nuevo appointment
                newAppointment.Id = appointments.Count + 1;
                newAppointment.Patient = patient;
                newAppointment.IsCompleted = false;
                newAppointment.Price = 0;
                newAppointment.SpecialistComment = null;

                switch (specialistElection)
                {
                    case "1":
                        newAppointment.Name = $"{patient.UserName}-General-Book-{newAppointment.Id}";
                        newAppointment.Specialist = specialist.Find(e => e.Speciality == "General");
                        newAppointment.AppointmentCreationDate = DateTime.Now;
                        Console.WriteLine("General appointment created");
                        Console.WriteLine("You'll be receiving an email shortly with further details");
                        break;

                    case "2":
                        newAppointment.Name = $"{patient.UserName}-Dentist-Book-{newAppointment.Id}";
                        newAppointment.Specialist = specialist.Find(e => e.Speciality == "Dentist");
                        newAppointment.AppointmentCreationDate = DateTime.Now;
                        Console.WriteLine("Dentist appointment created");
                        Console.WriteLine("You'll be receiving an email shortly with further details");
                        break;

                    case "3":
                        newAppointment.Name = $"{patient.UserName}-Ophthalmologist-Book-{newAppointment.Id}";
                        newAppointment.Specialist = specialist.Find(e => e.Speciality == "Ophthalmologist");
                        newAppointment.AppointmentCreationDate = DateTime.Now;
                        Console.WriteLine("Ophthalmologist appointment created");
                        Console.WriteLine("You'll be receiving an email shortly with further details");
                        break;
                    case "4":
                        newAppointment.Name = $"{patient.UserName}-Dermatologist-Book-{newAppointment.Id}";
                        newAppointment.Specialist = specialist.Find(e => e.Speciality == "Dermatologist");
                        newAppointment.AppointmentCreationDate = DateTime.Now;
                        Console.WriteLine("Dermatologist appointment created");
                        Console.WriteLine("You'll be receiving an email shortly with further details");
                        break;
                    case "5":
                        newAppointment.Name = $"{patient.UserName}-Pediatrician-Book-{newAppointment.Id}";
                        newAppointment.Specialist = specialist.Find(e => e.Speciality == "Pediatrician");
                        newAppointment.AppointmentCreationDate = DateTime.Now;
                        Console.WriteLine("Pediatrician appointment created");
                        Console.WriteLine("You'll be receiving an email shortly with further details");
                        break;
                }
                patient.Appointments.Add(newAppointment.Id);
                int index = patients.FindIndex(e => e.Email == patient.Email);
                patients[index] = patient;
                _jsonService.SerealizePatientsJsonFile(patients); //modifiying patient file


                appointments.Add(newAppointment); 
                _jsonService.SerealizeAppointmentJsonFile(appointments); //adding to file
                appointmentString = _jsonService.GetListFromFile("appointments");
                appointments = _jsonService.DeserializeAppointmentJsonFile(appointmentString);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Appointment booked!\n");
                Console.ResetColor();
                return 1;
            }
            else if(loggedPatientAction == "2")
            {
                var patientAppointments = appointments.FindAll(e => e.Patient.Email == patient.Email && e.IsCompleted == false).ToList();
                if (patientAppointments.Count == 0 ) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No appointments booked\n");
                    Console.ResetColor();
                }
                else
                {
                    var table = new Table();
                    table.BorderColor(Color.SkyBlue2);
                    table.AddColumn(new TableColumn("Id"));
                    table.AddColumn(new TableColumn("Apointment Name"));
                    table.AddColumn(new TableColumn("Comments"));
                    table.AddColumn(new TableColumn("Total amount"));
                    patientAppointments.ForEach(e => 
                    {
                        string currEnv = Environment.GetEnvironmentVariable("currency");
                        var currency = currEnv == "eu" ? "EUR" : "$";
                        var price = e.Price == 0 ? "Pending" : $"{e.Price.ToString("F")} {currency}";
                        var comments = e.SpecialistComment == null ? "Pending" : e.SpecialistComment;
                        table.AddRow($"{e.Id}", $"{e.Name}", $"{comments}", $"{price}");
                    });
                    AnsiConsole.Write(table);

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
                        var tableCancel = new Table();
                        tableCancel.BorderColor(Color.SkyBlue2);

                        tableCancel.AddColumn(new TableColumn("Id"));
                        tableCancel.AddColumn(new TableColumn("Apointment Name"));

                        for (int i = 0; i < patientAppointments.Count; i++)
                        {
                            tableCancel.AddRow($"{patientAppointments[i].Id}", $"{patientAppointments[i].Name}");

                            Console.WriteLine($" - Id:{patientAppointments[i].Id}: {patientAppointments[i].Name}");
                        }
                        AnsiConsole.Write(tableCancel);

                        var appointmentToCancel = Console.ReadLine();
                        int appointmentToCancelInt;
                        bool appointmentToCancelBool = int.TryParse(appointmentToCancel, out appointmentToCancelInt);

                        appointmentToCancelInt = appointmentToCancelBool == true ? Int32.Parse(appointmentToCancel) : 0;

                        var appointmentSelected = patientAppointments.Find(e => e.Id == appointmentToCancelInt);

                        while (appointmentSelected == null)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Wrong number, try again");
                            Console.ResetColor();
                            appointmentToCancel = Console.ReadLine();
                            appointmentToCancelBool = int.TryParse(appointmentToCancel, out appointmentToCancelInt);
                            appointmentToCancelInt = appointmentToCancelBool == true ? Int32.Parse(appointmentToCancel) : 0;
                            appointmentSelected = patientAppointments.Find(e => e.Id == appointmentToCancelInt);
                        }
                        var appointmentToCancelIndex = Int32.Parse(appointmentToCancel);

                        appointmentSelected.IsCompleted = true;

                        int index = appointments.FindIndex(e => e.Id == appointmentSelected.Id);
                        appointments[index] = appointmentSelected;
                        _jsonService.SerealizeAppointmentJsonFile(appointments); //modifiying patient file

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
    }
}
