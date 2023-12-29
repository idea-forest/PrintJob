using Microsoft.AspNet.SignalR.Client;
using System;

namespace PrintLoc.Helper
{
    public class SignalRManager
    {
        private HubConnection hubConnection;
        private IHubProxy hubProxy;

        public event EventHandler<ConnectivityStatusChangedEventArgs> ConnectivityStatusChanged;

        public SignalRManager(string signalRServerUrl, string hubName)
        {
            hubConnection = new HubConnection(signalRServerUrl);
            hubProxy = hubConnection.CreateHubProxy(hubName);

            hubProxy.On<bool, string>("UpdateConnectivityStatus", (isConnected, deviceId) =>
            {
                ConnectivityStatusChanged?.Invoke(this, new ConnectivityStatusChangedEventArgs(isConnected, deviceId));
            });

            hubConnection.Start().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine("SignalR connection error: " + task.Exception.GetBaseException());
                }
                else
                {
                    Console.WriteLine("SignalR connected");
                }
            });
        }

        public void StopConnection()
        {
            if (hubConnection != null)
            {
                hubConnection.Stop();
                hubConnection.Dispose();
            }
        }
    }

    public class ConnectivityStatusChangedEventArgs : EventArgs
    {
        public bool IsConnected { get; }
        public string DeviceId { get; }

        public ConnectivityStatusChangedEventArgs(bool isConnected, string deviceId)
        {
            IsConnected = isConnected;
            DeviceId = deviceId;
        }
    }
}
