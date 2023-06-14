using startup_shutdown_tracker.Domain;
using System.Linq;
using System.Threading.Tasks;

namespace startup_shutdown_tracker.Application;

public class TrackerService
{
	private readonly ITrackerRepository _repository;
	private readonly ITimeProvider _timeProvider;

	public TrackerService(ITrackerRepository repository, ITimeProvider timeProvider)
	{
		_repository = repository;
		_timeProvider = timeProvider;
	}

	public async Task AddStartupEntry()
	{
		var tracker = await _repository.GetTrackerAsync();
		var entry = new TrackerEntry()
		{
			Date = _timeProvider.Date,
			StartedAt = _timeProvider.LocalTime,
			EndedAt = null,
		};

		tracker.Entries.Add(entry);
		await _repository.SaveTrackerAsync(tracker);
	}

	public async Task AddShutdownEntry()
	{
		var tracker = await _repository.GetTrackerAsync();
		var lastStartupEntry = tracker.Entries.Last();

		// If the last entry already has shutdown entry, we do not want to overwrite it.
		if (lastStartupEntry.EndedAt.HasValue)
		{
			var shutdownOnlyEntry = new TrackerEntry()
			{
				Date = _timeProvider.Date,
				StartedAt = null,
				EndedAt = _timeProvider.LocalTime,
			};

			tracker.Entries.Add(shutdownOnlyEntry);
		}
		else
		{
			lastStartupEntry.EndedAt = _timeProvider.LocalTime;
		}

		await _repository.SaveTrackerAsync(tracker);
	}
}