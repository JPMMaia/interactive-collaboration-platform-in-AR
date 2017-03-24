using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using CollaborationEngine.Objects;
using UnityEngine.Networking;

namespace CollaborationEngine.Network
{
    public class ClientController : NetworkBehaviour
    {
        public class NetworkEventArgs : EventArgs
        {
            public SceneObject2.Data Data { get; set; }
        }
        public delegate void NetworkEventDelegate(object sender, NetworkEventArgs eventArgs);

        public event NetworkEventDelegate OnSceneObjectDataAdded;

        public static ClientController Instance
        {
            get
            {
                if (!_instance)
                    _instance = ObjectLocator.Instance.ClientController;

                return _instance;
            }
        }

        public void Awake()
        {
            _networkClient = NetworkManager.singleton.client;
            _networkClient.RegisterHandler(ServerController.AddSceneObjectDataOnClientHandle, OnAddSceneObjectData);
        }

        public void AddSceneObjectData(SceneObject2.Data sceneObjectData)
        {
            _networkClient.Send(ServerController.AddSceneObjectDataOnServerHandle, sceneObjectData);
        }

        private void OnAddSceneObjectData(NetworkMessage netMsg)
        {
            var data = netMsg.ReadMessage<SceneObject2.Data>();

            lock (_sceneData)
            {
                _sceneData.Add(data);
            }

            if (OnSceneObjectDataAdded != null)
            {
                var eventArgs = new NetworkEventArgs
                {
                    Data = data
                };
                OnSceneObjectDataAdded(this, eventArgs);
            }
        }

        public void AddSceneObjectsData(String sceneObjectsData)
        {
            UnityEngine.Debug.LogError("Data Received:" + sceneObjectsData);

            // Convert from string to array of bytes:
            var data = Convert.FromBase64String(sceneObjectsData);

            // Create memory string from data:
            var memoryStream = new MemoryStream(data);

            var binaryFormatter = new BinaryFormatter();
            var sceneObjectsData2 = (List<SceneObject2.Data>)binaryFormatter.Deserialize(memoryStream);

            lock (_sceneData)
            {
                _sceneData.AddRange(sceneObjectsData2);
            }
        }

        public List<SceneObject2.Data> SceneData
        {
            get { return _sceneData; }
        }

        private ClientController()
        {
        }

        private static ClientController _instance;
        private readonly List<SceneObject2.Data> _sceneData = new List<SceneObject2.Data>();
        private NetworkClient _networkClient;
    }
}
