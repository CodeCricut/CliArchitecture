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
	/// The <see cref="IHostApplicationLifetime"/> is registered automatically and can be used to gracefully stop the application.
	/// This hosted service stops the application after 5 seconds.
	/// The <seealso cref="IHostLifetime"/> service is registered automatically and is used internally to control the graceful shutdown of
	/// the application. 
	/// </summary>
	public class LifetimeHostedService : IHostedService
	{
		private readonly IHostApplicationLifetime _appLifetime;
		private readonly ILogger<LifetimeHostedService> _logger;

		public LifetimeHostedService(IHostApplicationLifetime appLifetime, ILogger<LifetimeHostedService> logger)
		{
			_appLifetime = appLifetime;
			_logger = logger;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogDebug("Lifetime.StartAsync");
			await StopAfterMillisAsync(5000, cancellationToken);
		}

		private async Task StopAfterMillisAsync(int millis, CancellationToken cancellationToken)
		{
			_logger.LogDebug($"Stopping in {millis} millis.");
			await Task.Delay(millis);
			_logger.LogDebug($"Stopping program.");
			_appLifetime.StopApplication();
			_logger.LogDebug($"Application requested to stop.");
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogDebug("Lifetime.StopAsync");
			return Task.CompletedTask;
		}
	}
}
