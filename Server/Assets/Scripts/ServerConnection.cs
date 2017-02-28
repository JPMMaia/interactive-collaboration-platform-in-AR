using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;

public class ServerConnection : MonoBehaviour
{
    public string Address = "127.0.0.1";
    public int Port = 8888;

    private struct ClientData
    {
        public int HostId { get; set; }
        public int ConnectionId { get; set; }
    }

    private HostTopology _hostTopology;
    private int _reliableChannelId;
    private int _unreliableChannelId;
    private int _hostId;
    private List<ClientData> _connectedHosts = new List<ClientData>(1);

    [UsedImplicitly]
    private void Awake()
    {
        Initialize();
    }

    [UsedImplicitly]
    private void OnApplicationQuit()
    {
        Shutdown();
    }

    private void Initialize()
    {
        // Initialize transport layer:
        NetworkTransport.Init();

        // Configure connection between peers:
        var connectionConfiguration = new ConnectionConfig();
        _reliableChannelId = connectionConfiguration.AddChannel(QosType.Reliable);
        _unreliableChannelId = connectionConfiguration.AddChannel(QosType.Unreliable);

        // Define network topology and create host:
        _hostTopology = new HostTopology(connectionConfiguration, 2);
        _hostId = NetworkTransport.AddHost(_hostTopology, Port);
        NetworkTransport.AddHost(_hostTopology, Port, Address);
    }

    private void Shutdown()
    {
        NetworkTransport.Shutdown();
    }

    [UsedImplicitly]
    private void Update()
    {
        // Create buffer to accommodate data received:
        var buffer = new byte[1024];

        // Receive data from any host:
        int hostId, connectionId, channelId, dataSize;
        byte error;
        var networkEvent = NetworkTransport.Receive(out hostId, out connectionId, out channelId, buffer, buffer.Length, out dataSize, out error);

        if (error != 0)
            Debug.LogError("Error: " + error);

        // Handle received data:
        switch (networkEvent)
        {
            case NetworkEventType.ConnectEvent:
                HandleHostConnect(hostId, connectionId, channelId, buffer, dataSize);
                break;

            case NetworkEventType.DisconnectEvent:
                HandleHostDisconnect(hostId, connectionId, channelId, buffer, dataSize);
                break;

            case NetworkEventType.DataEvent:
                HandleDataReceived(hostId, connectionId, channelId, buffer, dataSize);
                break;

            default:
                break;
        }
    }

    private void HandleHostConnect(int hostId, int connectionId, int channelId, byte[] buffer, int dataSize)
    {
        // Save host data:
        var host = new ClientData
        {
            HostId = hostId,
            ConnectionId = connectionId
        };
        _connectedHosts.Add(host);

        Debug.LogError("Connection received: " + hostId + ", " + connectionId);
    }

    private void HandleHostDisconnect(int hostId, int connectionId, int channelId, byte[] buffer, int dataSize)
    {
        // Remove connected host from list
        _connectedHosts.RemoveAll(data => data.ConnectionId == connectionId);

        Debug.LogError("Connection lost: " + hostId + ", " + connectionId);
    }

    private void HandleDataReceived(int hostId, int connectionId, int channelId, byte[] buffer, int dataSize)
    {
        Debug.LogError(Encoding.UTF8.GetString(buffer, 0, dataSize));
    }
}
