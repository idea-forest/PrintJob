using ProjectLoc.Models;

namespace ProjectLoc.Services
{
    public interface PrintJobHubClient
    {
        Task ReceivePrintJobs(PrintJob job);
        Task OnDisconnectedAsync();
    }
}
