// See https://aka.ms/new-console-template for more information

using Fclp;
using Microsoft.Extensions.DependencyInjection;
using startup_shutdown_tracker.Application;
using startup_shutdown_tracker.Application.Calculation;
using System;
using System.Linq;
using System.Threading.Tasks;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var serviceProvider = new ServiceCollection().InjectDependencies().BuildServiceProvider();

        var applicaitonArguments = new FluentCommandLineParser<ApplicationArguments>();

        applicaitonArguments
            .Setup(x => x.Action)
            .As('a', "action")
            .SetDefault(ApplicationAction.Present);

        applicaitonArguments.Parse(args.ToArray());

        var trackerService = serviceProvider.GetRequiredService<TrackerService>();

        switch (applicaitonArguments.Object.Action)
        {
            case ApplicationAction.Startup:
                await trackerService.AddStartupEntry();
                break;

            case ApplicationAction.Shutdown:
                await trackerService.AddShutdownEntry();
                break;

            case ApplicationAction.Present:
                var hoursCalculator = serviceProvider.GetRequiredService<HoursCalculator>();
                var calculation = await hoursCalculator.Calculate();
                await Console.Out.WriteLineAsync(calculation.ToString());
                break;
        }
    }
}
