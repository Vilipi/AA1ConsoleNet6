using Microsoft.Extensions.Logging;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Extensions.Logging;

namespace aa1.Services
{
    public class PublicService
    {
        JsonService _jsonService = new JsonService();

        public int PublicMenu()
        {
            var _logger = LoggerFactory.Create(builder => builder.AddNLog()).CreateLogger<Program>();

            try
            {
                var specilasitsString = _jsonService.GetListFromFile("specialist");
                var specialist = _jsonService.DeserializeSpecialistsJsonFile(specilasitsString);

                var table = new Table();
                table.BorderColor(Color.SkyBlue2);
                table.Width(100);
                table.AddColumn(new TableColumn("Speciality"));
                table.AddColumn(new TableColumn("Name"));

                specialist.ForEach(e =>
                {
                    table.AddRow($"{e.Speciality}", $"{e.Name} {e.LastName}");
                });
                AnsiConsole.Write(table);

                return 0;
            }
            catch(Exception ex)
            {
                _logger.LogDebug(ex.Message);
                return 0;
            }
           
        }
    }
}
