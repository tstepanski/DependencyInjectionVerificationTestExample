using System.Threading;
using System.Threading.Tasks;

namespace DependencyInjectionVerificationTestExample
{
	internal interface ISomeHighLevelService
	{
		Task DoSomethingAsync(CancellationToken cancellationToken = default);
	}
}