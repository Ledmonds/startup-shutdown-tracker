using startup_shutdown.tracking.domain;
using System.Threading.Tasks;

namespace startup_shutdown.tracking.application;

public interface ITrackerRepository
{
	Task<Tracker> GetTrackerAsync();

	Task SaveTrackerAsync(Tracker tracker);
}