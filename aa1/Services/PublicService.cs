using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aa1.Services
{
    public class PublicService
    {
        JsonService _jsonService = new JsonService();

        public int PublicMenu()
        {
            var specilasitsString = _jsonService.GetListFromFile("specialist");
            var specialist = _jsonService.DeserializeSpecialistsJsonFile(specilasitsString);

            specialist.ForEach(e =>
            {
                Console.WriteLine($" - {e.Speciality}: {e.Name} {e.LastName}");
            });

            return 0;
        }
    }
}
