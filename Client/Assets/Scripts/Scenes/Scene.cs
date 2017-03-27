using System.Collections.Generic;
using CollaborationEngine.Objects;
using CollaborationEngine.Network;
using UnityEngine;

namespace CollaborationEngine.Scenes
{
    public class Scene : IScene
    {
        public Scene(GameObject gameObject)
        {
            Debug.Assert(gameObject != null, "Scene game object is null");

            GameObject = gameObject;

            var networkController = ClientController.Instance;
            networkController.OnSceneObjectDataAdded += ClientController_OnSceneObjectDataAdded;
        }

        public void Add(SceneObject.Data sceneObjectData)
        {
            SceneObject sceneObject;

            if (sceneObjectData.Type == SceneObjectType.Real)
            {
                sceneObject = new RealObject(sceneObjectData);
            }
            else if (sceneObjectData.Type == SceneObjectType.Indication)
            {
                sceneObject = new IndicationObject(sceneObjectData);
            }
            else
            {
                return;
            }

            // Instantiate scene object:
            sceneObject.Instantiate(GameObject.transform);

            // Add to scene objects:
            lock (_sceneObjects)
            {
                _sceneObjects.Add(sceneObject);
            }
        }
        public void Remove(SceneObject.Data sceneObjectData)
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
            foreach (var data in eventArgs.Data)
            {
                Add(data);
            }
        }

        private readonly List<SceneObject> _sceneObjects = new List<SceneObject>();
    }
}
