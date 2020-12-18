using System.Threading;
using System.Threading.Tasks;

namespace DependencyInjectionVerificationTestExample
{
	internal sealed class SomeMiddlewareService : ISomeMiddleWareService
	{
		private readonly ISomeRepository _someRepository;

		public SomeMiddlewareService(ISomeRepository someRepository)
		{
			_someRepository = someRepository;
		}

		public async Task DomeSomeInternalThingAsync(CancellationToken cancellationToken = default)
		{
			var delay = await _someRepository.GetDelayAsync(cancellationToken);
			
			await Task.Delay(delay, cancellationToken);
		}
	}
}