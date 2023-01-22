using aa1.Services;
using Microsoft.Extensions.Logging;
using NLog;
using System.Globalization;


//var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
//logger.Debug("init main"); // si aparece esto texto,  sginfica que logger esta funcionando bien


MainMenuService MainMenuService = new MainMenuService();

MainMenuService.MainMenu();
   