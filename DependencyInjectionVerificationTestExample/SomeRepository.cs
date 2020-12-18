using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace DependencyInjectionVerificationTestExample
{
	internal sealed class SomeRepository : ISomeRepository
	{
		private readonly IOptions<SomeConfiguration> _options;

		public SomeRepository(IOptions<SomeConfiguration> options)
		{
			_options = options;
		}

		public Task<int> GetDelayAsync(CancellationToken cancellationToken = default)
		{
			var length = _options.Value.ConnectionString.Length;
			
			return Task.FromResult(length);
		}
	}
}