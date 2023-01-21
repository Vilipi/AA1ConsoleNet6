using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aa1.Models
{
    public class Specialist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool IsRetired { get; set; }
        public decimal Rating { get; set; }
        public DateTime BirthDate { get; set; }
        public string Speciality { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }        
    }
}
