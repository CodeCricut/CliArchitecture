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
	/// The <see cref="IHostApplicationLifetime"/> service is registered automatically and is used to handle lifetime events.
	/// You can registerd three main lifetime event handlers:
	/// <list type="bullet">
	///	<item><code>ApplicationStarted</code></item>
	///	<item><code>ApplicatoinStopping</code></item>
	///	<item><code>ApplicationStopped</code></item>
	/// </list>
	/// </summary>
	public class LifetimeEventsHostedService : IHostedService
	{
		private readonly IHostApplicationLifetime _appLifetime;
		private readonly ILogger<LifetimeEventsHostedService> _logger;

		public LifetimeEventsHostedService(IHostApplicationLifetime appLifetime, ILogger<LifetimeEventsHostedService> logger)
		{
			_appLifetime = appLifetime;
			_logger = logger;
		}

		/// <summary>
		/// Register the three lifetime handlers.
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogDebug("LifetimeEventsHostedService.StartAsync");

			_appLifetime.ApplicationStarted.Register(OnStarted);
			_appLifetime.ApplicationStopping.Register(OnStopping);
			_appLifetime.ApplicationStopped.Register(OnStopped);
			return Task.CompletedTask;
		}

		/// <summary>
		/// Even this StopAsync method is called before OnStopped, but after OnStopping.
		/// </summary>
		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogDebug("LifetimeEventsHostedService.StopAsync");
			return Task.CompletedTask;
		}

		/// <summary>
		/// Will be called after other hosted services' StartAsync methods called
		/// </summary>
		private void OnStarted()
		{
			_logger.LogInformation("OnStarted has been called.");

			// Perform post-startup activities here
		}

		/// <summary>
		/// Will be called before other hosted services' StopAsync methods called
		/// </summary>
		private void OnStopping()
		{
			_logger.LogInformation("OnStopping has been called.");

			// Perform on-stopping activities here
		}

		/// <summary>
		/// This is called after all other hosted services' (including this one) StopAsync is called.
		/// </summary>
		private void OnStopped()
		{
			_logger.LogInformation("OnStopped has been called.");

			// Perform post-stopped activities here
		}
	}
}
