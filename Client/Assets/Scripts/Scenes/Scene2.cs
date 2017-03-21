using System.Collections.Generic;
using CollaborationEngine.Objects;
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
        }

        public void Add(SceneObject2 sceneObject)
        {
            SceneObjects.Add(sceneObject);
        }
        public void Remove(SceneObject2 sceneObject)
        {
            SceneObjects.Remove(sceneObject);
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
