using NLog.Web;
using NLog;
using sshBackend1;
using sshBackend1;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure; //per me bo raporte pdf
using System.ComponentModel;

//NOTE: Add dependencies/services in StartupExtensions.cs and keep this file as-is

var builder = WebApplication.CreateBuilder(args);
var logger = LogManager.Setup().GetCurrentClassLogger();

try
{
    logger.Debug("init main");
    builder.Host.UseNLog(new NLogAspNetCoreOptions() { RemoveLoggerFactoryFilter = false });

    QuestPDF.Settings.License = LicenseType.Community;

    var app = builder.ConfigureServices().ConfigurePipeline();
    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}

public partial class Program { }

