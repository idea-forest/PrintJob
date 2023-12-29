using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProjectLoc.Models;
using ProjectLoc.Data;
using System.Collections.Concurrent;

namespace ProjectLoc.Services
{
    public class ConnectionStatusHub : Hub
    {
        private readonly ApiDbContext _context;
        private readonly ConcurrentDictionary<string, string> connectedClients = new ConcurrentDictionary<string, string>();
        private readonly TimeSpan CheckInterval = TimeSpan.FromSeconds(30); // Change this to your desired check interval
        private Timer checkTimer;

        public ConnectionStatusHub(ApiDbContext dbContext)
        {
            _context = dbContext;
            checkTimer = new Timer(CheckDeviceConnections, null, CheckInterval, CheckInterval);
        }

        private async void CheckDeviceConnections(object state)
        {
            foreach (var deviceId in connectedClients.Values)
            {
                bool isConnected = IsClientConnected(deviceId); // Check if the client is connected

                await UpdateDeviceStatusAsync(deviceId, isConnected);
            }
        }

        private bool IsClientConnected(string deviceId)
        {
            // Get the connection ID associated with the deviceId
            var connectionId = connectedClients.FirstOrDefault(x => x.Value == deviceId).Key;

            if (connectionId != null)
            {
                var connection = Clients.Client(connectionId);
                if (connection != null)
                {
                    // Check if the client is still connected by invoking a client method
                    try
                    {
                        connection.SendAsync("Ping").Wait(TimeSpan.FromSeconds(5)); // Assuming a method named "Ping" exists on the client
                        return true; // If the client responds within the timeout, consider it connected
                    }
                    catch (Exception)
                    {
                        return false; // If an exception occurs (client didn't respond), consider it disconnected
                    }
                }
            }

            return false; // If connectionId or connection is null, consider it disconnected
        }

        private async Task UpdateDeviceStatusAsync(string deviceId, bool isConnected)
        {
            Device device = _context.Devices.FirstOrDefault(d => d.DeviceId == deviceId);

            if (device != null && device.DeviceStatus != isConnected)
            {
                device.DeviceStatus = isConnected;
                await _context.SaveChangesAsync(); // Save changes to the database asynchronously
            }
        }

        public async Task ReceiveHeartbeat(string deviceId)
        {
            Console.WriteLine($"Received heartbeat: {deviceId}");

            if (connectedClients.TryGetValue(Context.ConnectionId, out string storedDeviceId) && storedDeviceId == deviceId)
            {
                await Clients.All.SendAsync("SendHeartbeatToClients", deviceId);
                await UpdateDeviceStatusAsync(deviceId, isConnected: true); // Update device status as connected
            }
            else
            {
                // Device is not connected; handle accordingly
                Console.WriteLine($"Device {deviceId} is not connected or unauthorized.");
            }
        }

        public override async Task OnConnectedAsync()
        {
            // Add connected client to the connectedClients dictionary
            string deviceId = Context.GetHttpContext().Request.Query["deviceId"];
            connectedClients.TryAdd(Context.ConnectionId, deviceId);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (connectedClients.TryRemove(Context.ConnectionId, out string deviceId))
            {
                await UpdateDeviceStatusAsync(deviceId, isConnected: false); // Update device status in the database asynchronously
            }
            await base.OnDisconnectedAsync(exception);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                checkTimer?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
