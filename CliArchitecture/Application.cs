using CommandLine;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CliArchitecture
{
	[Verb("run")]
	public class ApplicationOptions
	{
	}

	public class Application : IHostedService, IDisposable
	{
		private readonly ILogger<Application> _logger;

		public Application(ApplicationOptions opts, ILogger<Application> logger)
		{
			_logger = logger;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Application.StartAsync()");

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Application.StopAsync()");
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_logger.LogTrace("Application.Dispose()");
		}
	}
}
