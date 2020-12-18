using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DependencyInjectionVerificationTestExample;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit;

namespace DependencyInjectionVerificationTestExampleTests
{
	public sealed class Tests
	{
		private const RegexOptions CompiledAndMultiline = RegexOptions.Compiled | RegexOptions.Multiline;

		private static readonly Regex FailedDependencyResolutionMessageRegex =
			new(@"Unable to resolve service for type '([^']+)' while attempting to activate '([^']+)'.",
				CompiledAndMultiline);

		private static readonly Regex GenericOpenRegex = new(@"\`([0-9]+)\[", CompiledAndMultiline);

		private static readonly ImmutableHashSet<Type> ExcludedTypes = new[]
			{
				typeof(ILogger),
				typeof(ILogger<>),
				typeof(IOptions<>),
				typeof(IOptionsSnapshot<>),
				typeof(IOptionsMonitor<>),
				typeof(IOptionsFactory<>),
				typeof(IOptionsMonitorCache<>),
				typeof(Lazy<>)
			}
			.ToImmutableHashSet();

		[Fact]
		public Task Resolve_GivenHealthyRegistrar_ReturnsAnyRegisteredService()
		{
			return RunResolveTest<SomeHealthyRegistrar>();
		}

		[Fact]
		public Task Resolve_GivenUnHealthyRegistrar_ReturnsAnyRegisteredService()
		{
			return RunResolveTest<SomeUnhealthyRegistrar>();
		}

		private static async Task RunResolveTest<TRegistrar>() where TRegistrar : IRegistrar, new()
		{
			IServiceCollection serviceCollection = new ServiceCollection();

			serviceCollection = new TRegistrar().RegisterAll(serviceCollection);

			var serviceTypes = serviceCollection
				.Select(descriptor => descriptor.ServiceType)
				.Where(type => ExcludedTypes.All(excludedType =>
				{
					if (!excludedType.IsAssignableFrom(type))
					{
						return true;
					}

					try
					{
						var genericTypeDefinition = type.GetGenericTypeDefinition();

						return !excludedType.IsAssignableFrom(genericTypeDefinition);
					}
					catch
					{
						return false;
					}
				}));

			await using (var serviceProvider = serviceCollection.BuildServiceProvider())
			{
				var failedToResolveDependency = false;
				
				var errorMessage = serviceTypes
					.Select(serviceType =>
					{
						var (success, serviceName) = TryToResolveType(serviceProvider, serviceType);

						failedToResolveDependency = !success;

						return serviceName;
					})
					.Where(typeName => !string.IsNullOrWhiteSpace(typeName))
					.Aggregate(new StringBuilder($@"Could not resolve the following services:{Environment.NewLine}"),
						(stringBuilder, missingTypeName) => stringBuilder
							.AppendLine(missingTypeName)
							.AppendLine(string.Empty.PadLeft(25, '-')))
					.ToString();

				Assert.False(failedToResolveDependency, errorMessage);
			}
		}

		private static (bool Success, string? ServiceName) TryToResolveType(IServiceProvider serviceProvider,
			Type serviceType)
		{
			try
			{
				serviceProvider.GetService(serviceType);

				return (true, null);
			}
			catch (InvalidOperationException exception)
			{
				var serviceNames = FailedDependencyResolutionMessageRegex
					.Matches(exception.Message)
					.First()
					.Groups
					.Values
					.Skip(1)
					.Select(typeNameCapture =>
					{
						var unformattedTypeName = typeNameCapture.Value.Replace(@"]", @">");

						return GenericOpenRegex.Replace(unformattedTypeName, @"<");
					})
					.ToArray();

				var serviceName = $@"{serviceNames[0]} for type {serviceNames[1]}";

				return (false, serviceName);
			}
		}
	}
}