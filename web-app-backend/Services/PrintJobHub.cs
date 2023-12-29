using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectLoc.Models;
using ProjectLoc.Data;

public class PrintJobHub : Hub
{
    private static List<PrintJob> printJobs = new List<PrintJob>();

    private readonly ApiDbContext _context;

    public PrintJobHub(ApiDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task GetPrintJobsByDeviceId(string deviceId)
    {
        List<PrintJob> printJobs = _context.PrintJobs
                .Where(p => p.DeviceId == deviceId)
                .Where(p => p.Status == "Pending")
                .OrderByDescending(p => p.CreatedAt)
                .ToList();
        await Clients.Caller.SendAsync("ReceivePrintJobs", printJobs);
    }
}
