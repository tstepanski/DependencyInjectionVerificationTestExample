namespace DependencyInjectionVerificationTestExample
{
	internal sealed class SomeDoomedService : ISomeDoomedService
	{
		private readonly ISomeUnResolvableRepository _someUnResolvableRepository;

		public SomeDoomedService(ISomeUnResolvableRepository someUnResolvableRepository)
		{
			_someUnResolvableRepository = someUnResolvableRepository;
		}

		public decimal Calculate()
		{
			var leftValue = _someUnResolvableRepository.GetRandomValue();
			var rightValue = _someUnResolvableRepository.GetRandomValue();

			return leftValue + rightValue;
		}
	}
}