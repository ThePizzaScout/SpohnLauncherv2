using Microsoft.Extensions.DependencyInjection;
using SpohnStory.Services;
using SpohnStory.Configuration;
using SpohnStory.Forms;

namespace SpohnStory
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Set up dependency injection
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            // Configure application settings
            ApplicationConfiguration.Initialize();

            // Get the main form from DI and run it
            var mainForm = serviceProvider.GetRequiredService<Form1>();
            Application.Run(mainForm);
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            // Register services
            services.AddSingleton<ILoggingService, LoggingService>();
            services.AddSingleton<IConfigurationService, ConfigurationService>();
            services.AddSingleton<IClientLocatorService, ClientLocatorService>();
            services.AddSingleton<IApiClient, ApiClient>();
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<ILaunchService, LaunchService>();

            // Register forms
            services.AddSingleton<Form1>();
            services.AddTransient<SettingsForm>();
            services.AddTransient<DiagnosticsForm>();
        }
    }
}