using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Scannit.ViewModels;
using System;
using System.IO;
using System.Reflection;
using Xamarin.Essentials;

namespace Scannit
{
    public static class Startup
    {
        public static IServiceProvider ServiceProvider { get; set; }
        public static void Init(Action<HostBuilderContext, IServiceCollection> nativeConfigureServices)
        {
            var configFilePath = ExtractResource("Scannit.appsettings.json", FileSystem.AppDataDirectory);

            var host = new HostBuilder()
                .ConfigureHostConfiguration(c =>
                {
                    c.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" });
                    c.AddJsonFile(configFilePath);
                })
                .ConfigureServices((c, x) =>
                {
                    nativeConfigureServices(c, x);
                    ConfigureServices(c, x);
                })
                .ConfigureLogging(l => l.AddConsole(o =>
                {
                    o.DisableColors = true;
                }))
                .Build();

            ServiceProvider = host.Services;
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            if (context.HostingEnvironment.IsDevelopment())
            {
                // Mock services go here
            }
            else
            {
                // Non-mocks here
            }

            services.AddSingleton<MainViewModel>();
            services.AddSingleton<SettingsViewModel>();
        }

        private static string ExtractResource(string filename, string location)
        {
            Assembly a = Assembly.GetExecutingAssembly();
            using (Stream resFilestream = a.GetManifestResourceStream(filename))
            {
                if (resFilestream != null)
                {
                    string full = Path.Combine(location, filename);
                    using (FileStream stream = File.Create(full))
                    {
                        resFilestream.CopyTo(stream);
                    }
                }
            }

            return Path.Combine(location, filename);
        }
    }
}
