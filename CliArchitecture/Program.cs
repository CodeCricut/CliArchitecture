using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CliArchitecture
{
	class Program
	{
		static async Task<int> Main(string[] args)
		{
			/* The host being built encapsulates app resources such as:
			 *	- services
			 *	- logging
			 *	- configuration
			 *	- hosted services (where hosted services are tasks that have an entry and exit procedure)
			 */
			var builder = new HostBuilder()
				// Set up the configuration for the host
				.ConfigureAppConfiguration(configBuilder =>
				{
					configBuilder.SetBasePath(Directory.GetCurrentDirectory())
					   .AddJsonFile(AppDomain.CurrentDomain.BaseDirectory + "appsettings.json", optional: false, reloadOnChange: true)
					   .AddEnvironmentVariables();
				})
				// Register services for the host
				.ConfigureServices((hostContext, services) =>
				{
					// The host has access to the configuration
					var configuration = hostContext.Configuration;

					services.AddSerilogLogger(configuration);

					services.AddProgramService<Application, ApplicationOptions>(args);
					services.AddProgramService<ApplicationVerb, ApplicationVerbOptions>(args);
				});

			try
			{
				// Builds and starts the host, but will end when CTRL+C is entered.
				// If the CTRL+C to shutdown process is not desired, then use builder.StartAsync()
				await builder.RunConsoleAsync();
			}
			catch (OperationCanceledException) { }
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return 1;
			}
			
			return 0;
		}

		private static async Task ManualDIAndHost()
		{
			// =============== MANUALLY CREATING DI PROVIDER =============== //
			// Create service collection
			var services = new ServiceCollection();

			// Services could be registered at this point

			// Configuration
			var configurationBuilder = new ConfigurationBuilder();

			// // Add default config file
			configurationBuilder.AddJsonFile("appsettings.json", optional: true);

			// // Build Configuration
			IConfiguration configuration = configurationBuilder.Build();

			// // Inject configuration into DI system
			services.AddSingleton(configuration);

			// Build Service Provider
			var provider = services.BuildServiceProvider();

			// One (nonstandard) way of making this provider globally accessible is to have a public static IProvider property somewhere.
			// The standard way is constructor injection

			// At this point, the DI system is set up
			var myConfig = provider.GetService<IConfiguration>();
		}
	}
}
