using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliArchitecture
{
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// If an instance of <typeparamref name="TProgramOptions"/> could be constructed from the <paramref name="args"/>, then register the <typeparamref name="TProgramEntry"/>
		/// which is dependant upon the <typeparamref name="TProgramOptions"/> as a hosted service.
		/// </summary>
		/// <typeparam name="TProgramEntry"></typeparam>
		/// <typeparam name="TProgramOptions"></typeparam>
		/// <param name="services"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static IServiceCollection AddProgramService<TProgramEntry, TProgramOptions>(this IServiceCollection services, string[] args)
			where TProgramEntry : class, IHostedService
			where TProgramOptions : class
		{
			if (services.TryAddProgramOption<TProgramOptions>(args))
			{
				return services.AddHostedService<TProgramEntry>();
			}
			else return services;
		}


		/// <summary>
		/// Try to parse the <paramref name="args"/> to construct the relevant <typeparamref name="TOption"/> and register it to the service collection.
		/// If the options couldn't be constructed, then the service collection will not be modified.
		/// <see cref="AddProgramService{TProgramEntry, TProgramOptions}(IServiceCollection, string[])"/> is preferable.
		/// </summary>
		/// <typeparam name="TOption">The type of option to try to register.</typeparam>
		/// <param name="services">The service collection to add to.</param>
		/// <param name="args">The CLI arguments to parse.</param>
		/// <returns>The <paramref name="args"/> could be parsed and the <typeparamref name="TOption"/> was registerd with the service collection.</returns>
		public static bool TryAddProgramOption<TOption>(this IServiceCollection services, string[] args)
			where TOption : class 
		{
			bool added = false;

			new Parser().ParseArguments(args, new [] { typeof(TOption) })
				.WithParsed(opt => {
					services.AddSingleton<TOption>((TOption) opt);
					added = true;
					})
				.WithNotParsed(opt => added = false);

			return added;
		}

		/// <summary>
		/// Register the Serilog <see cref="ILogger{TCategoryName}"/> dependency and configure it using the <paramref name="configuration"/>.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="configuration"></param>
		/// <returns>The service collection for chaining.</returns>
		public static IServiceCollection AddSerilogLogger(this IServiceCollection services, IConfiguration configuration)
		{
			var logger = new LoggerConfiguration()
						.ReadFrom.Configuration(configuration)
						.Enrich.FromLogContext()
						.CreateLogger();

			return services.AddLogging(config =>
			{
				config.ClearProviders();
				config.AddProvider(new SerilogLoggerProvider(logger));
				string minimumLevel = configuration.GetSection("Serilog:MinimumLevel")?.Value;

				if (!string.IsNullOrEmpty(minimumLevel))
				{
					config.SetMinimumLevel(Enum.Parse<LogLevel>(minimumLevel));
				}
			});
		}
	}
}
