using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;

class ClientConnection : MonoBehaviour
{
    public string ServerAddress = "127.0.0.1";
    public int ServerPort = 8888;
    public int ClientPort = 4444;

    private HostTopology _hostTopology;
    private int _reliableChannelId;
    private int _unreliableChannelId;
    private int _hostId;
    private int _serverConnectionId;

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
        _hostId = NetworkTransport.AddHost(_hostTopology, ClientPort);
    }

    private void Shutdown()
    {
        NetworkTransport.Shutdown();
    }

    [UsedImplicitly]
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ConnectTo(ServerAddress, ServerPort);
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            var message = Encoding.UTF8.GetBytes("Hello! :D");

            Send(message);
        }

    }

    public void ConnectTo(string address, int port)
    {
        // Send connection request:
        byte error;
        _serverConnectionId = NetworkTransport.Connect(_hostId, address, port, 0, out error);
        Debug.LogError("ConnectionID: " + _serverConnectionId + ", error: " + error);

        // Receive connection response:
        {
            // Create buffer to accommodate data received:
            var buffer = new byte[1024];

            // Receive data from any host:
            int hostId, connectionId, channelId, dataSize;
            var networkEvent = NetworkTransport.Receive(out hostId, out connectionId, out channelId, buffer, buffer.Length, out dataSize, out error);
            if (networkEvent == NetworkEventType.ConnectEvent)
            {
                if (connectionId == _serverConnectionId)
                {
                    Debug.LogError("Connected to server. Error: " + error);
                }
            }
        }
    }

    public void Disconnect()
    {
        // Send disconnect request:
        byte error;
        NetworkTransport.Disconnect(_hostId, _serverConnectionId, out error);
    }

    public void Send(byte[] buffer, bool reliable = true)
    {
        // Choose channel ID:
        var channelId = reliable ? _reliableChannelId : _unreliableChannelId;

        // Send buffer:
        byte error;
        NetworkTransport.Send(_hostId, _serverConnectionId, channelId, buffer, buffer.Length, out error);
    }
}
