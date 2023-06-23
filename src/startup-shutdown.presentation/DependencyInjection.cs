using Microsoft.Extensions.DependencyInjection;
using startup_shutdown.calculation.application;
using startup_shutdown.calculation.application.Calculators.Day;
using startup_shutdown.calculation.application.Calculators.Hour;
using startup_shutdown.calculation.application.Calculators.Presentation;
using startup_shutdown.common;
using startup_shutdown.infrastructure;
using startup_shutdown.infrastructure.Repositories;
using startup_shutdown.tracking.application;

namespace startup_shutdown.presentation;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection InjectDependencies(this IServiceCollection serviceCollection)
	{
		// Common
		serviceCollection.AddTransient<ITimeProvider, TimeProvider>();

		// Tracking
		serviceCollection.AddTransient<ITrackerRepository, TrackerRepository>();
		serviceCollection.AddTransient<TrackerService>();

		// Calculation
		serviceCollection.AddTransient<IDayEntryRepository, DayEntryRepository>();
		serviceCollection.AddTransient<HourCalculator>();
		serviceCollection.AddTransient<DayCalculator>();
		serviceCollection.AddTransient<Calculator>();

		return serviceCollection;
	}
}