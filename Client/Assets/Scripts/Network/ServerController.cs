using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using CollaborationEngine.Objects;
using UnityEngine;
using UnityEngine.Networking;

namespace CollaborationEngine.Network
{
    public class ServerController : NetworkBehaviour
    {
        public static short InitializeSceneDataOnClientHandle = MsgType.Highest + 1;
        public static short AddSceneObjectDataOnServerHandle = MsgType.Highest + 1;
        public static short AddSceneObjectDataOnClientHandle = MsgType.Highest + 1;

        public void Awake()
        {
            NetworkServer.RegisterHandler(AddSceneObjectDataOnServerHandle, OnAddSceneObjectData);
        }

        public void OnServerConnect(NetworkConnection clientConnection)
        {
            Debug.LogError("Client connected!");
        }

        private void OnAddSceneObjectData(NetworkMessage netMsg)
        {
            var data = netMsg.ReadMessage<SceneObject2.Data>();

            lock (_sceneData)
            {
                _sceneData.Add(data);
            }

            // Send to all clients
            NetworkServer.SendToAll(AddSceneObjectDataOnClientHandle, data);
        }

        /*public void CmdRequestSceneObjectsData()
        {
            // Create a memory stream:
            var memoryStream = new MemoryStream();

            // Save scene data on the memory stream:
            var binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(memoryStream, _sceneData);

            // Convert data to a string:
            var data = Convert.ToBase64String(memoryStream.GetBuffer());

            // Send data to clients:
            ClientController.Instance.RpcAddSceneObjectsData(data);
        }*/

        private ServerController()
        {
        }

        private static ServerController _instance;
        private readonly List<SceneObject2.Data> _sceneData = new List<SceneObject2.Data>();
    }
}
