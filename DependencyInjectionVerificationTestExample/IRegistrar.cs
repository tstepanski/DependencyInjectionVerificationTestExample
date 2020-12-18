using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionVerificationTestExample
{
	public interface IRegistrar
	{
		IServiceCollection RegisterAll(IServiceCollection serviceCollection);
	}
}