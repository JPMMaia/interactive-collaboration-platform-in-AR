using System.Collections.Generic;
using CollaborationEngine.Objects;
using UnityEngine;
using UnityEngine.Networking;

namespace CollaborationEngine.Network
{
    public class ServerController : NetworkBehaviour
    {
        public static short InitializeSceneDataOnClientHandle = MsgType.Highest + 1;
        public static short AddSceneObjectDataOnServerHandle = MsgType.Highest + 2;
        public static short AddSceneObjectDataOnClientHandle = MsgType.Highest + 3;
        public static short RemoveSceneObjectDataOnServerHandle = MsgType.Highest + 4;
        public static short RemoveSceneObjectDataOnClientHandle = MsgType.Highest + 5;
        public static short UpdateSceneObjectDataOnServerHandle = MsgType.Highest + 6;
        public static short UpdateSceneObjectDataOnClientHandle = MsgType.Highest + 7;

        public void Awake()
        {
            NetworkServer.RegisterHandler(AddSceneObjectDataOnServerHandle, OnAddSceneObjectData);
            NetworkServer.RegisterHandler(RemoveSceneObjectDataOnServerHandle, OnRemoveSceneObjectData);
            NetworkServer.RegisterHandler(UpdateSceneObjectDataOnServerHandle, OnUpdateSceneObjectData);
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

        private ServerController()
        {
        }

        private void OnAddSceneObjectData(NetworkMessage networkMessage)
        {
            var data = networkMessage.ReadMessage<SceneObject.Data>();

            // Assign a network ID to the entity:
            data.ID = ++SceneObject.Data.SceneObjectCount;

            lock (_sceneData)
            {
                _sceneData.Add(data);
            }

            // Send to all clients
            NetworkServer.SendToAll(AddSceneObjectDataOnClientHandle, data);
        }
        private void OnRemoveSceneObjectData(NetworkMessage networkMessage)
        {
            var data = networkMessage.ReadMessage<SceneObject.Data>();

            lock (_sceneData)
            {
                _sceneData.RemoveAll(sceneObject => sceneObject.ID == data.ID);
            }

            // Send to all clients
            NetworkServer.SendToAll(RemoveSceneObjectDataOnClientHandle, data);
        }
        private void OnUpdateSceneObjectData(NetworkMessage networkMessage)
        {
            var data = networkMessage.ReadMessage<SceneObject.DataCollection>();
            
            lock (_sceneData)
            {
                foreach (var element in data.DataEnumerable)
                {
                    var index = _sceneData.FindIndex(e => e.ID == element.ID);
                    _sceneData[index] = element;
                }
            }
            
            // Send to all clients
            NetworkServer.SendToAll(UpdateSceneObjectDataOnClientHandle, data);
        }

        private static ServerController _instance;
        private readonly List<SceneObject.Data> _sceneData = new List<SceneObject.Data>();
    }
}
