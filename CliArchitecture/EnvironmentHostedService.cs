using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CliArchitecture
{
	/// <summary>
	/// An implementation of <see cref="IHostEnvironment"/> is registed automatically and can be used to get information about
	/// the following settings:
	/// <list type="bullet">
	/// <item>ApplicationName</item>
	/// <item>EnvironmentName</item>
	/// <item>ContentRootPath</item>
	/// </list>
	/// </summary>
	public class EnvironmentHostedService : IHostedService
	{
		private readonly IHostEnvironment _environment;
		private readonly ILogger<EnvironmentHostedService> _logger;

		public EnvironmentHostedService(IHostEnvironment environment, ILogger<EnvironmentHostedService> logger)
		{
			_environment = environment;
			_logger = logger;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation(_environment.ApplicationName);
			_logger.LogInformation(_environment.EnvironmentName);
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}
