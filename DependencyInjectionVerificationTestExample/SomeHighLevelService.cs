using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DependencyInjectionVerificationTestExample
{
	internal sealed class SomeHighLevelService : ISomeHighLevelService 
	{
		private readonly ISomeMiddleWareService _someMiddleWareService;
		private readonly ILogger<SomeHighLevelService> _logger;

		public SomeHighLevelService(ISomeMiddleWareService someMiddleWareService, ILogger<SomeHighLevelService> logger)
		{
			_someMiddleWareService = someMiddleWareService;
			_logger = logger;
		}

		public async Task DoSomethingAsync(CancellationToken cancellationToken = default)
		{
			_logger.LogDebug(@"Starting work...");
			
			var internalThingTask = _someMiddleWareService.DomeSomeInternalThingAsync(cancellationToken);
			var thisWorkTask = Task.Delay(1, cancellationToken);

			await Task.WhenAll(internalThingTask, thisWorkTask);
			
			_logger.LogDebug(@"Work done.");
		}
	}
}