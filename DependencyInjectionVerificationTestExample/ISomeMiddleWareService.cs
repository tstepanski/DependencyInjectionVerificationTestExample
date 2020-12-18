using System.Threading;
using System.Threading.Tasks;

namespace DependencyInjectionVerificationTestExample
{
	internal interface ISomeMiddleWareService
	{
		public Task DomeSomeInternalThingAsync(CancellationToken cancellationToken = default);
	}
}