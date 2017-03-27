using System.Collections.Generic;
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

            var data = new SceneObject.DataCollection
            {
                DataEnumerable = _sceneData
            };
            clientConnection.Send(InitializeSceneDataOnClientHandle, data);
        }

        private void OnAddSceneObjectData(NetworkMessage networkMessage)
        {
            var data = networkMessage.ReadMessage<SceneObject.Data>();

            lock (_sceneData)
            {
                _sceneData.Add(data);
            }

            // Send to all clients
            NetworkServer.SendToAll(AddSceneObjectDataOnClientHandle, data);
        }

        private ServerController()
        {
        }

        private static ServerController _instance;
        private readonly List<SceneObject.Data> _sceneData = new List<SceneObject.Data>();
    }
}
