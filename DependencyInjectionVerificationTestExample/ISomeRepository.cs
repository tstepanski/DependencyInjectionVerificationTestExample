using System.Threading;
using System.Threading.Tasks;

namespace DependencyInjectionVerificationTestExample
{
	internal interface ISomeRepository
	{
		Task<int> GetDelayAsync(CancellationToken cancellationToken = default);
	}
}