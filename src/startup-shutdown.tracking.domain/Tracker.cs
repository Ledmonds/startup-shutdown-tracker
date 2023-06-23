using System.Collections.Generic;

namespace startup_shutdown_tracker.Domain;

public class Tracker
{
	public IList<TrackerEntry> Entries { get; init; } = new List<TrackerEntry>();
}