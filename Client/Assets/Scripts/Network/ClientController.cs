using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            public IEnumerable<SceneObject.Data> Data { get; set; }
        }
        public delegate void NetworkEventDelegate(object sender, NetworkEventArgs eventArgs);

        public event NetworkEventDelegate OnSceneObjectDataAdded;
        public event NetworkEventDelegate OnSceneObjectDataRemoved;

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
            _networkClient.RegisterHandler(ServerController.InitializeSceneDataOnClientHandle, OnInitializeSceneDataOnClientHandle);
            _networkClient.RegisterHandler(ServerController.AddSceneObjectDataOnClientHandle, OnAddSceneObjectData);
            _networkClient.RegisterHandler(ServerController.RemoveSceneObjectDataOnClientHandle, OnRemoveSceneObjectData);
        }

        public void AddSceneObjectData(SceneObject.Data sceneObjectData)
        {
            _networkClient.Send(ServerController.AddSceneObjectDataOnServerHandle, sceneObjectData);
        }
        public void RemoveSceneObjectData(SceneObject.Data sceneObjectData)
        {
            _networkClient.Send(ServerController.RemoveSceneObjectDataOnServerHandle, sceneObjectData);
        }

        public void AddSceneObjectsData(String sceneObjectsData)
        {
            UnityEngine.Debug.LogError("Data Received:" + sceneObjectsData);

            // Convert from string to array of bytes:
            var data = Convert.FromBase64String(sceneObjectsData);

            // Create memory string from data:
            var memoryStream = new MemoryStream(data);

            var binaryFormatter = new BinaryFormatter();
            var sceneObjectsData2 = (List<SceneObject.Data>)binaryFormatter.Deserialize(memoryStream);

            lock (_sceneData)
            {
                _sceneData.AddRange(sceneObjectsData2);
            }
        }

        public List<SceneObject.Data> SceneData
        {
            get { return _sceneData; }
        }

        private ClientController()
        {
        }

        private void OnInitializeSceneDataOnClientHandle(NetworkMessage networkMessage)
        {
            var data = networkMessage.ReadMessage<SceneObject.DataCollection>();

            lock (_sceneData)
            {
                _sceneData.AddRange(data.DataEnumerable);
            }

            NotifySceneObjectDataAdded(data.DataEnumerable);
        }
        private void OnAddSceneObjectData(NetworkMessage networkMessage)
        {
            var data = networkMessage.ReadMessage<SceneObject.Data>();

            lock (_sceneData)
            {
                _sceneData.Add(data);
            }

            NotifySceneObjectDataAdded(new List<SceneObject.Data> { data });
        }
        private void OnRemoveSceneObjectData(NetworkMessage networkMessage)
        {
            var data = networkMessage.ReadMessage<SceneObject.Data>();

            lock (_sceneData)
            {
                _sceneData.RemoveAll(sceneObject => sceneObject.ID == data.ID);
            }

            NotifySceneObjectDataRemoved(new List<SceneObject.Data> { data });
        }

        private void NotifySceneObjectDataAdded(IEnumerable<SceneObject.Data> data)
        {
            if (OnSceneObjectDataAdded != null)
            {
                var eventArgs = new NetworkEventArgs
                {
                    Data = data
                };

                OnSceneObjectDataAdded(this, eventArgs);
            }
        }
        private void NotifySceneObjectDataRemoved(IEnumerable<SceneObject.Data> data)
        {
            if (OnSceneObjectDataRemoved != null)
            {
                var eventArgs = new NetworkEventArgs
                {
                    Data = data
                };

                OnSceneObjectDataRemoved(this, eventArgs);
            }
        }

        private static ClientController _instance;
        private readonly List<SceneObject.Data> _sceneData = new List<SceneObject.Data>();
        private NetworkClient _networkClient;
    }
}
