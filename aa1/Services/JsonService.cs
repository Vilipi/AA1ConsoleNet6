using aa1.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace aa1.Services
{
    public class JsonService
    {
        public static string _pathSpecialists = $@"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName}\Resource\specialists.json";
        public static string _pathPatients = $@"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName}\Resource\patients.json";
        public static string _pathAppointments = $@"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName}\Resource\appointments.json";
        ILogger _logger = LoggerFactory.Create(builder => builder.AddNLog()).CreateLogger<Program>();

        public string GetListFromFile(string fileClass)
        {
            try
            {
                string dataJsonFromFile;
                string path;

                if (fileClass == "specialist")
                    path = _pathSpecialists;
                else if (fileClass == "appointment")
                    path = _pathAppointments;
                else
                    path = _pathPatients;

                using (var reader = new StreamReader(path))
                {
                    dataJsonFromFile = reader.ReadToEnd();
                }
                _logger.LogInformation($"File {path} succesfully found");

                return dataJsonFromFile;
            }
            
            catch(Exception ex)
            {
                _logger.LogDebug(ex.ToString(), "Error - Path not found");    
                return fileClass;
            }
        }
        public List<Specialist> DeserializeSpecialistsJsonFile(string specilistsJsonFromFile)
        {
            return JsonConvert.DeserializeObject<List<Specialist>>(specilistsJsonFromFile);
        }

        public List<Patient> DeserializePatientsJsonFile(string patientsJsonFromFile)
        {
            return JsonConvert.DeserializeObject<List<Patient>>(patientsJsonFromFile);
        }

        public void SerealizePatientsJsonFile(List<Patient> patients)
        {
            string patientsJson = JsonConvert.SerializeObject(patients.ToArray(), Formatting.Indented);
            File.WriteAllText(_pathPatients, patientsJson);
        }

        public List<Appointment> DeserializeAppointmentJsonFile(string appointmentJsonFromFile)
        {
            return JsonConvert.DeserializeObject<List<Appointment>>(appointmentJsonFromFile);
        }

        public void SerealizeAppointmentJsonFile(List<Appointment> appointments)
        {
            string appointmentsJson = JsonConvert.SerializeObject(appointments.ToArray(), Formatting.Indented);
            File.WriteAllText(_pathAppointments, appointmentsJson);
        }
    }
}

