using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CliArchitecture
{
	[Verb("verb", HelpText = "Application verb for performing specfic commands.")]
	public class ApplicationVerbOptions
	{
		[Option('f', "flag", Required = false, HelpText = "A CLI flag to control specific aspects of the command being run.")]
		public bool Flag { get; set; }
	}
	public class ApplicationVerb : IHostedService, IDisposable
	{
		private readonly ApplicationVerbOptions _opts;
		private readonly ILogger<ApplicationVerb> _logger;

		public ApplicationVerb(ApplicationVerbOptions opts, ILogger<ApplicationVerb> logger)
		{
			_opts = opts;
			_logger = logger;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("ApplicationVerb.StartAsync()");
			_logger.LogInformation($"ApplicationVerb.Flag: {_opts.Flag}");

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("ApplicationVerb.StopAsync()");
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_logger.LogTrace("ApplicationVerb.Dispose()");
		}
	}
}
