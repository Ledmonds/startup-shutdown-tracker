using System.Collections.Generic;

namespace startup_shutdown.tracking.domain;

public class Tracker
{
    public IList<TrackerEntry> Entries { get; init; } = new List<TrackerEntry>();
}
