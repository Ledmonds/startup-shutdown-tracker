// See https://aka.ms/new-console-template for more information

using Fclp;
using Microsoft.Extensions.DependencyInjection;
using startup_shutdown_tracker.Application;
using startup_shutdown_tracker.Application.Calculators.Presentation;
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
				await trackerService.Startup();
				break;

			case ApplicationAction.Shutdown:
				await trackerService.Shutdown();
				break;

			case ApplicationAction.Present:
				var hoursCalculator = serviceProvider.GetRequiredService<Calculator>();
				var calculation = await hoursCalculator.Calculate();

				var sb = new System.Text.StringBuilder();

				sb.AppendLine(
					$"Total: {calculation.Total.Worked.ToString()} : {calculation.Total.Overtime.ToString()}"
				);
				sb.AppendLine(
					$"Monthly: {calculation.Monthly.Worked.ToString()} : {calculation.Monthly.Overtime.ToString()}"
				);
				sb.AppendLine(
					$"Weekly: {calculation.Weekly.Worked.ToString()} : {calculation.Weekly.Overtime.ToString()}"
				);

				await Console.Out.WriteLineAsync(sb.ToString());

				break;
		}
	}
}