using System;
using System.Collections.Generic;
using CollaborationEngine.Objects;
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
        public event OnSceneObjectAddedDelegate<InstructionObject> OnIndicationObjectAdded;

        public Scene(GameObject gameObject)
        {
            Debug.Assert(gameObject != null, "Scene game object is null");

            GameObject = gameObject;


            OnRealObjectAdded += Scene_OnSceneObjectAdded;
            OnIndicationObjectAdded += Scene_OnSceneObjectAdded;
        }

        public void Add(SceneObject.Message sceneObjectData)
        {
            Debug.LogError("Add!");

            if(sceneObjectData.Data.ID == 0)
                throw new Exception("Network Data ID must be different from 0!");

            if (sceneObjectData.Data.Type == SceneObjectType.Real)
            {
                //NotifyRealObjectAdded(sceneObjectData.Data);
            }
            else if (sceneObjectData.Data.Type == SceneObjectType.Indication)
            {
                //NoifyIndicationObjectAdded(new InstructionObject(sceneObjectData));
            }
        }
        public void Remove(SceneObject.Message sceneObjectData)
        {
            Debug.LogError("Remove!");

            lock (_sceneObjects)
            {
                // Find object:
                var index = _sceneObjects.FindIndex(element => element.ID == sceneObjectData.Data.ID);

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

        public void SynchronizeScene()
        {
            /*var dataToUpdate = new List<SceneObject.Message>();

            lock (_sceneObjects)
            {
                dataToUpdate.AddRange(
                    from sceneObject in _sceneObjects
                    where sceneObject.IsDirty
                    select sceneObject.NetworkData
                    );
            }

            var data = new SceneObject.CollectionMessage
            {
                DataEnumerable = dataToUpdate
            };
            ClientController.Instance.UpdateSceneObjectData(data);*/
        }

        public GameObject GameObject { get; private set; }

        private void NotifyRealObjectAdded(RealObject sceneObject)
        {
            if (OnRealObjectAdded != null)
                OnRealObjectAdded(this, new SceneEventArgs<RealObject> { SceneObject = sceneObject });
        }
        private void NotifyIndicationObjectAdded(InstructionObject sceneObject)
        {
            if (OnIndicationObjectAdded != null)
                OnIndicationObjectAdded(this, new SceneEventArgs<InstructionObject> { SceneObject = sceneObject });
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


        private readonly List<SceneObject> _sceneObjects = new List<SceneObject>();
    }
}
