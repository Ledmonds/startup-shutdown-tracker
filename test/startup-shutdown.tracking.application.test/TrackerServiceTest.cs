using FluentAssertions;
using Moq;
using startup_shutdown_tracker.Common;
using startup_shutdown_tracker.Domain;
using startup_shutdown_tracker.Tracking.Application;
using Xunit;

namespace startup_shutdown_tracker.Application.Test;

public class TrackerServiceTest
{
	private readonly Mock<ITrackerRepository> _repository = new();
	private readonly Mock<ITimeProvider> _timeProvider = new();
	private readonly TrackerService _trackerService;
	private static readonly DateOnly _todaysDate = new DateOnly(2022, 01, 01);

	public TrackerServiceTest()
	{
		_trackerService = new TrackerService(_repository.Object, _timeProvider.Object);

		_timeProvider.Setup(x => x.Date).Returns(_todaysDate);
	}

	[Fact]
	public async Task Adds_open_startup_entry()
	{
		// Arrange
		var today = new TrackerEntry()
		{
			Id = Guid.NewGuid(),
			Date = _todaysDate,
			StartedAt = new TimeOnly(10, 10, 10),
			EndedAt = new TimeOnly(10, 10, 10),
		};

		var tracker = new Tracker() { Entries = new List<TrackerEntry>() { today } };
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
				x.Entries[0].Should().BeSameAs(today);
				x.Entries[1]
					.Should()
					.BeEquivalentTo(
						new TrackerEntry()
						{
							Id = Guid.NewGuid(),
							Date = startupDate,
							StartedAt = startupTime,
							EndedAt = null,
						},
						opts => opts.Excluding(arg => arg.Id)
					);
			});

		// Act
		await _trackerService.Startup();

		// Assert
		_repository.VerifyAll();
	}

	[Fact]
	public async Task Modifies_pre_existing_open_entry_for_current_day()
	{
		// Arrange
		var yesterday = new TrackerEntry()
		{
			Id = Guid.NewGuid(),
			Date = _todaysDate.AddDays(-1),
			StartedAt = new TimeOnly(10, 10, 10),
			EndedAt = new TimeOnly(17, 00, 00),
		};
		var earlierToday = new TrackerEntry()
		{
			Id = Guid.NewGuid(),
			Date = _todaysDate,
			StartedAt = new TimeOnly(06, 00, 00),
			EndedAt = new TimeOnly(07, 00, 00),
		};
		var today = new TrackerEntry()
		{
			Id = Guid.NewGuid(),
			Date = _todaysDate,
			StartedAt = new TimeOnly(08, 00, 00),
			EndedAt = null,
		};

		var tracker = new Tracker()
		{
			Entries = new List<TrackerEntry>() { yesterday, earlierToday, today }
		};
		_repository.Setup(x => x.GetTrackerAsync()).ReturnsAsync(tracker);

		var shutdownTime = new TimeOnly(18, 00, 00);
		_timeProvider.Setup(x => x.Time).Returns(shutdownTime);

		_repository
			.Setup(x => x.SaveTrackerAsync(tracker))
			.Callback<Tracker>(x =>
			{
				x.Entries.Should().HaveCount(3);
				x.Entries[0].Should().BeSameAs(yesterday);
				x.Entries[1].Should().BeSameAs(earlierToday);
				x.Entries[2]
					.Should()
					.BeEquivalentTo(
						new TrackerEntry()
						{
							Id = Guid.NewGuid(),
							Date = today.Date,
							StartedAt = today.StartedAt,
							EndedAt = shutdownTime,
						},
						opts => opts.Excluding(arg => arg.Id)
					);
			});

		// Act
		await _trackerService.Shutdown();

		// Assert
		_repository.VerifyAll();
	}

	[Fact]
	public async Task Adds_new_shutdown_entry_if_pre_existinig_entry_for_current_day_is_closed()
	{
		// Arrange
		var yesterday = new TrackerEntry()
		{
			Id = Guid.NewGuid(),
			Date = _todaysDate.AddDays(-1),
			StartedAt = new TimeOnly(10, 10, 10),
			EndedAt = new TimeOnly(17, 00, 00),
		};
		var today = new TrackerEntry()
		{
			Id = Guid.NewGuid(),
			Date = _todaysDate,
			StartedAt = new TimeOnly(08, 00, 00),
			EndedAt = new TimeOnly(17, 00, 00),
		};

		var tracker = new Tracker()
		{
			Entries = new List<TrackerEntry>() { yesterday, today }
		};
		_repository.Setup(x => x.GetTrackerAsync()).ReturnsAsync(tracker);

		var shutdownTime = new TimeOnly(18, 00, 00);
		_timeProvider.Setup(x => x.Time).Returns(shutdownTime);

		_repository
			.Setup(x => x.SaveTrackerAsync(tracker))
			.Callback<Tracker>(x =>
			{
				x.Entries.Should().HaveCount(3);
				x.Entries[0].Should().BeSameAs(yesterday);
				x.Entries[1].Should().BeSameAs(today);
				x.Entries[2]
					.Should()
					.BeEquivalentTo(
						new TrackerEntry()
						{
							Id = Guid.NewGuid(),
							Date = _todaysDate,
							StartedAt = null,
							EndedAt = shutdownTime,
						},
						opts => opts.Excluding(arg => arg.Id)
					);
			});

		// Act
		await _trackerService.Shutdown();

		// Assert
		_repository.VerifyAll();
	}
}