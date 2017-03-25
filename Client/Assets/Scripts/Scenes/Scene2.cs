using System.Collections.Generic;
using CollaborationEngine.Objects;
using CollaborationEngine.Network;
using UnityEngine;

namespace CollaborationEngine.Scenes
{
    public class Scene2 : IScene
    {
        public Scene2(GameObject gameObject)
        {
            Debug.Assert(gameObject != null, "Scene game object is null");

            GameObject = gameObject;

            var networkController = ClientController.Instance;
            networkController.OnSceneObjectDataAdded += ClientController_OnSceneObjectDataAdded;
            //networkController.RequestSceneObjectsData();
        }

        public void Add(SceneObject2.Data sceneObjectData)
        {
            if (sceneObjectData.Type == SceneObjectType.Real)
            {
                var sceneObject = new RealObject(sceneObjectData);
                sceneObject.Instantiate(GameObject.transform);

                lock (_sceneObjects)
                {
                    _sceneObjects.Add(sceneObject);
                }
            }
        }
        public void Remove(SceneObject2.Data sceneObjectData)
        {
            // TODO
        }
        public void Clear()
        {
            lock (_sceneObjects)
            {
                _sceneObjects.Clear();
            }
        }

        public void FixedUpdate()
        {
            lock (_sceneObjects)
            {
                foreach (var sceneObject in _sceneObjects)
                    sceneObject.FixedUpdate();
            }
        }
        public void FrameUpdate()
        {
            lock (_sceneObjects)
            {
                foreach (var sceneObject in _sceneObjects)
                    sceneObject.FrameUpdate();
            }
        }

        public GameObject GameObject { get; private set; }

        private void ClientController_OnSceneObjectDataAdded(object sender, ClientController.NetworkEventArgs eventArgs)
        {
            Debug.LogError("Object Added");

            foreach (var data in eventArgs.Data)
            {
                Add(data);
            }
        }

        private readonly List<SceneObject2> _sceneObjects = new List<SceneObject2>();
    }
}
