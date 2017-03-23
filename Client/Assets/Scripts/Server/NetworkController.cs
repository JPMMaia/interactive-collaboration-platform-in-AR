using System;
using System.Collections.Generic;
using CollaborationEngine.Objects;
using UnityEngine;
using UnityEngine.Networking;

namespace CollaborationEngine.Server
{
    public class NetworkController : NetworkBehaviour
    {
        public class NetworkEventArgs : EventArgs
        {
            public SceneObject2.Data Data { get; set; }
        }
        public delegate void NetworkEventDelegate(NetworkController sender, NetworkEventArgs eventArgs);

        public event NetworkEventDelegate OnSceneObjectAdded;

        private List<SceneObject2.Data> _sceneData = new List<SceneObject2.Data>();

        public void Awake()
        {
            DontDestroyOnLoad(this);
        }

        [Command]
        public void CmdAddSceneObject(SceneObject2.Data sceneObjectData)
        {
            RpcAddSceneObject(sceneObjectData);
        }

        [ClientRpc]
        private void RpcAddSceneObject(SceneObject2.Data sceneObjectData)
        {
            lock (_sceneData)
            {
                _sceneData.Add(sceneObjectData);
            }

            if (OnSceneObjectAdded != null)
            {
                var eventArgs = new NetworkEventArgs
                {
                    Data = sceneObjectData
                };
                OnSceneObjectAdded.Invoke(this, eventArgs);
            }
        }
    }
}
