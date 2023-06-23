using Microsoft.Extensions.DependencyInjection;
using startup_shutdown_tracker.Application;
using startup_shutdown_tracker.Application.Calculators.Day;
using startup_shutdown_tracker.Application.Calculators.Hour;
using startup_shutdown_tracker.Application.Calculators.Presentation;
using startup_shutdown_tracker.Infrastructure;
using startup_shutdown_tracker.Infrastructure.Repositories;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection InjectDependencies(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ITimeProvider, TimeProvider>();
        serviceCollection.AddSingleton<TrackerRepository>();
        serviceCollection.AddSingleton<IDayEntryRepository, DayEntryRepository>();
        serviceCollection.AddSingleton<TrackerService>();
        serviceCollection.AddSingleton<HourCalculator>();
        serviceCollection.AddSingleton<DayCalculator>();
        serviceCollection.AddSingleton<Calculator>();

        return serviceCollection;
    }
}
