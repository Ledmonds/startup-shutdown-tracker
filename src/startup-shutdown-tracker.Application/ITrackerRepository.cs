using startup_shutdown_tracker.Domain;
using System.Threading.Tasks;

namespace startup_shutdown_tracker.Application;

public interface ITrackerRepository
{
	Task<Tracker> GetTrackerAsync();

	Task SaveTrackerAsync(Tracker tracker);
}