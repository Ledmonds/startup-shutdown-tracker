// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using startup_shutdown_tracker.Application;
using System;

var serviceProvider = new ServiceCollection().InjectDependencies().BuildServiceProvider();

var trackerService = serviceProvider.GetRequiredService<TrackerService>();

await trackerService.AddStartupEntry();
await trackerService.AddShutdownEntry();

Console.WriteLine("testing");