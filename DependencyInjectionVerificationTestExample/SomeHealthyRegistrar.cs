using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionVerificationTestExample
{
	public sealed class SomeHealthyRegistrar : IRegistrar
	{
		public IServiceCollection RegisterAll(IServiceCollection serviceCollection)
		{
			return serviceCollection
				.AddSingleton<ISomeHighLevelService, SomeHighLevelService>()
				.AddSingleton<ISomeMiddleWareService, SomeMiddlewareService>()
				.AddSingleton<ISomeRepository, SomeRepository>()
				.AddLogging()
				.AddOptions()
				.AddOptions<SomeConfiguration>()
				.Configure(configuration => configuration.ConnectionString = @"Boom")
				.Services;
		}
	}
}