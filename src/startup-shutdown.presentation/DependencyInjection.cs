using Microsoft.Extensions.DependencyInjection;
using startup_shutdown_tracker.Application;
using startup_shutdown_tracker.Application.Calculators.Day;
using startup_shutdown_tracker.Application.Calculators.Hour;
using startup_shutdown_tracker.Application.Calculators.Presentation;
using startup_shutdown_tracker.Common;
using startup_shutdown_tracker.Infrastructure;
using startup_shutdown_tracker.Infrastructure.Repositories;
using startup_shutdown_tracker.Tracking.Application;

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