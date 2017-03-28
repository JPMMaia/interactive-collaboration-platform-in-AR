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

            var networkController = ClientController.Instance;
            networkController.OnSceneObjectDataAdded += ClientController_OnSceneObjectDataAdded;

            OnRealObjectAdded += Scene_OnSceneObjectAdded;
            OnIndicationObjectAdded += Scene_OnSceneObjectAdded;
        }

        public void Add(SceneObject.Data sceneObjectData)
        {
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

        private readonly List<SceneObject> _sceneObjects = new List<SceneObject>();
    }
}
