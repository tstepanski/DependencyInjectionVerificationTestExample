using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionVerificationTestExample
{
	public sealed class SomeUnhealthyRegistrar : IRegistrar
	{
		public IServiceCollection RegisterAll(IServiceCollection serviceCollection)
		{
			return serviceCollection.AddSingleton<ISomeDoomedService, SomeDoomedService>();
		}
	}
}