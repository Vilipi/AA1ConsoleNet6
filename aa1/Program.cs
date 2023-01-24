using aa1.Services;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System.Globalization;

var _logger = LoggerFactory.Create(builder => builder.AddNLog()).CreateLogger<Program>();
MainMenuService MainMenuService = new MainMenuService();

try
{
    MainMenuService.MainMenu();
}
catch(Exception ex)
{
    _logger.LogDebug(ex.Message);
}
   