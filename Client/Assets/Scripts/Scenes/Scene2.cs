using System.Collections.Generic;
using CollaborationEngine.Objects;
using CollaborationEngine.Server;
using UnityEngine;

namespace CollaborationEngine.Scenes
{
    public class Scene2 : IScene
    {
        public GameObject GameObject { get; private set; }
        public List<SceneObject2> SceneObjects { get; private set; }

        public Scene2(GameObject gameObject)
        {
            Debug.Assert(gameObject != null, "Scene game object is null");

            GameObject = gameObject;
            SceneObjects = new List<SceneObject2>();

            ApplicationInstance.Instance.NetworkController.OnSceneObjectAdded += NetworkController_OnSceneObjectAdded;
            Debug.LogError("Scene Created");
            // TODO sync already existing data
        }

        private void NetworkController_OnSceneObjectAdded(NetworkController sender, NetworkController.NetworkEventArgs eventArgs)
        {
            Debug.LogError("Object Added");
            Add(eventArgs.Data);
        }

        public void Add(SceneObject2.Data sceneObjectData)
        {
            if (sceneObjectData.Type == SceneObjectType.Real)
            {
                var sceneObject = new RealObject(sceneObjectData);
                sceneObject.Instantiate(GameObject.transform);
                SceneObjects.Add(sceneObject);
            }
        }
        public void Remove(SceneObject2.Data sceneObjectData)
        {
            // TODO
        }
        public void Clear()
        {
            SceneObjects.Clear();
        }

        public void FixedUpdate()
        {
            foreach (var sceneObject in SceneObjects)
                sceneObject.FixedUpdate();
        }
        public void FrameUpdate()
        {
            foreach (var sceneObject in SceneObjects)
                sceneObject.FrameUpdate();
        }
    }
}
