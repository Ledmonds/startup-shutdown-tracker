using FluentAssertions;
using Moq;
using startup_shutdown_tracker.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace startup_shutdown_tracker.Application.Test;

public class TrackerServiceTest
{
	private readonly Mock<ITrackerRepository> _repository = new();
	private readonly Mock<ITimeProvider> _timeProvider = new();
	private readonly TrackerService _trackerService;

	public TrackerServiceTest()
	{
		_trackerService = new TrackerService(_repository.Object, _timeProvider.Object);
	}

	[Fact]
	public async Task Adds_a_startup_entry_to_the_tracker_repository()
	{
		// Arrange
		var existingEntry = new TrackerEntry()
		{
			Date = new DateOnly(2021, 10, 10),
			StartedAt = new TimeOnly(10, 10, 10),
			EndedAt = new TimeOnly(10, 10, 10),
		};

		var tracker = new Tracker() { Entries = new List<TrackerEntry>() { existingEntry } };
		_repository.Setup(x => x.GetTrackerAsync()).ReturnsAsync(tracker);

		var startupDate = new DateOnly(2023, 03, 12);
		var startupTime = new TimeOnly(11, 03, 22);
		_timeProvider.Setup(x => x.Date).Returns(startupDate);
		_timeProvider.Setup(x => x.Time).Returns(startupTime);

		_repository
			.Setup(x => x.SaveTrackerAsync(tracker))
			.Callback<Tracker>(x =>
			{
				x.Entries.Should().HaveCount(2);
				x.Entries[0].Should().BeSameAs(existingEntry);
				x.Entries[1]
					.Should()
					.BeEquivalentTo(
						new TrackerEntry()
						{
							Date = startupDate,
							StartedAt = startupTime,
							EndedAt = null,
						}
					);
			});

		// Act
		await _trackerService.AddStartupEntry();

		// Assert
		_repository.VerifyAll();
	}

	[Fact]
	public async Task Adds_a_should_entry_to_the_tracker_repository()
	{
		// Arrange
		var entryToBeLeftUnmodified = new TrackerEntry()
		{
			Date = new DateOnly(2021, 08, 05),
			StartedAt = new TimeOnly(10, 10, 10),
			EndedAt = new TimeOnly(17, 00, 00),
		};
		var entryForShutdown = new TrackerEntry()
		{
			Date = new DateOnly(2021, 10, 15),
			StartedAt = new TimeOnly(09, 00, 00),
			EndedAt = null,
		};

		var tracker = new Tracker()
		{
			Entries = new[] { entryToBeLeftUnmodified, entryForShutdown }
		};
		_repository.Setup(x => x.GetTrackerAsync()).ReturnsAsync(tracker);

		var shutdownTime = new TimeOnly(18, 00, 00);
		_timeProvider.Setup(x => x.Time).Returns(shutdownTime);

		_repository
			.Setup(x => x.SaveTrackerAsync(tracker))
			.Callback<Tracker>(x =>
			{
				x.Entries.Should().HaveCount(2);
				x.Entries[0].Should().BeSameAs(entryToBeLeftUnmodified);
				x.Entries[1]
					.Should()
					.BeEquivalentTo(
						new TrackerEntry()
						{
							Date = entryForShutdown.Date,
							StartedAt = entryForShutdown.StartedAt,
							EndedAt = new TimeOnly(18, 00, 01),
						}
					);
			});

		// Act
		await _trackerService.AddShutdownEntry();

		// Assert
		_repository.VerifyAll();
	}
}