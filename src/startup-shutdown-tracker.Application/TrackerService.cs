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
			StartedAt = _timeProvider.Time,
			EndedAt = null,
		};

		tracker.Entries.Add(entry);
		await _repository.SaveTrackerAsync(tracker);
	}

	public async Task AddShutdownEntry()
	{
		var tracker = await _repository.GetTrackerAsync();

		var lastStartupEntry = tracker.Entries
			.Where(entry => entry.Date == _timeProvider.Date && entry.StartedAt.HasValue)
			.LastOrDefault();

		if (lastStartupEntry is not null && !lastStartupEntry.EndedAt.HasValue)
		{
			lastStartupEntry.EndedAt = _timeProvider.Time;

			await _repository.SaveTrackerAsync(tracker);

			return;
		}

		// If there is no pre-existing entry for the given day, or if the last entry has already been ended, add a new entry
		var shutdownOnlyEntry = new TrackerEntry()
		{
			Date = _timeProvider.Date,
			StartedAt = null,
			EndedAt = _timeProvider.Time,
		};

		tracker.Entries.Add(shutdownOnlyEntry);

		await _repository.SaveTrackerAsync(tracker);

		return;
	}
}