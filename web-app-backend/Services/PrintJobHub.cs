using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectLoc.Models;
using ProjectLoc.Data;
using ProjectLoc.Services;
using System.Diagnostics;

public class PrintJobHub : Hub<PrintJobHubClient>
{
    private readonly ApiDbContext _context;

    public PrintJobHub(ApiDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task OnConnected()
    {
        Console.WriteLine(Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public async Task OnDisconnected(Exception exception)
    {
        Console.WriteLine(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task OnReconnected()
    {

    }

    public async Task ReceivePrintJobs(PrintJob job)
    {
        await Clients.All.ReceivePrintJobs(job);
    }
}
