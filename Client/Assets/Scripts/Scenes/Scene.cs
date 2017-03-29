using System;
using System.Collections.Generic;
using CollaborationEngine.Objects;
using CollaborationEngine.Network;
using UnityEngine;

namespace CollaborationEngine.Scenes
{
    public class Scene : IScene
    {
        public class SceneEventArgs<TSceneObjectType> : EventArgs where TSceneObjectType : SceneObject
        {
            public TSceneObjectType SceneObject { get; set; }
        }
        public delegate void OnSceneObjectAddedDelegate<TSceneObjectType>(Scene scene, SceneEventArgs<TSceneObjectType> eventArgs) where TSceneObjectType : SceneObject;

        public event OnSceneObjectAddedDelegate<RealObject> OnRealObjectAdded;
        public event OnSceneObjectAddedDelegate<IndicationObject> OnIndicationObjectAdded;

        public Scene(GameObject gameObject)
        {
            Debug.Assert(gameObject != null, "Scene game object is null");

            GameObject = gameObject;

            var clientController = ClientController.Instance;
            clientController.OnSceneObjectDataAdded += ClientController_OnSceneObjectDataAdded;
            clientController.OnSceneObjectDataRemoved += ClientController_OnSceneObjectDataRemoved;

            OnRealObjectAdded += Scene_OnSceneObjectAdded;
            OnIndicationObjectAdded += Scene_OnSceneObjectAdded;
        }

        public void Add(SceneObject.Data sceneObjectData)
        {
            Debug.LogError("Add!");

            if(sceneObjectData.ID == 0)
                throw new Exception("Network Data ID must be different from 0!");

            if (sceneObjectData.Type == SceneObjectType.Real)
            {
                NotifyRealObjectAdded(new RealObject(sceneObjectData));
            }
            else if (sceneObjectData.Type == SceneObjectType.Indication)
            {
                NotifyIndicationObjectAdded(new IndicationObject(sceneObjectData));
            }
        }
        public void Remove(SceneObject.Data sceneObjectData)
        {
            Debug.LogError("Remove!");

            lock (_sceneObjects)
            {
                // Find object:
                var index = _sceneObjects.FindIndex(element => element.NetworkData.ID == sceneObjectData.ID);

                // Destroy it:
                var sceneObject = _sceneObjects[index];
                sceneObject.Destroy();

                // Remove from list:
                _sceneObjects.RemoveAt(index);
            }
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

        private void NotifyRealObjectAdded(RealObject sceneObject)
        {
            if (OnRealObjectAdded != null)
                OnRealObjectAdded(this, new SceneEventArgs<RealObject> { SceneObject = sceneObject });
        }
        private void NotifyIndicationObjectAdded(IndicationObject sceneObject)
        {
            if (OnIndicationObjectAdded != null)
                OnIndicationObjectAdded(this, new SceneEventArgs<IndicationObject> { SceneObject = sceneObject });
        }

        private void Scene_OnSceneObjectAdded<TSceneObjectType>(Scene scene, SceneEventArgs<TSceneObjectType> eventArgs) where TSceneObjectType : SceneObject
        {
            var sceneObject = eventArgs.SceneObject;

            // Instantiate scene object:
            sceneObject.Instantiate(GameObject.transform);

            // Add to scene objects:
            lock (_sceneObjects)
            {
                _sceneObjects.Add(sceneObject);
            }
        }
        private void ClientController_OnSceneObjectDataAdded(object sender, ClientController.NetworkEventArgs eventArgs)
        {
            foreach (var data in eventArgs.Data)
            {
                Add(data);
            }
        }
        private void ClientController_OnSceneObjectDataRemoved(object sender, ClientController.NetworkEventArgs eventArgs)
        {
            foreach (var data in eventArgs.Data)
            {
                Remove(data);
            }
        }

        private readonly List<SceneObject> _sceneObjects = new List<SceneObject>();
    }
}
