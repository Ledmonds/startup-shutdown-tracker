// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using startup_shutdown_tracker.Application;
using startup_shutdown_tracker.Infrastructure;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection InjectDependencies(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<ITimeProvider, TimeProvider>();
		serviceCollection.AddSingleton<ITrackerRepository, TrackerRepository>();
		serviceCollection.AddSingleton<TrackerService>();

		return serviceCollection;
	}
}