using System.Collections.Generic;
using CollaborationEngine.Objects.Components;
using UnityEngine;

namespace CollaborationEngine.Objects
{
    public class SceneObject2
    {
        public GameObject Prefab { get; set; }
        public GameObject GameObject { get; private set; }
        public List<IComponent> Components { get; }
        public string Name { get; protected set; }
        public bool Rotate { get; set; }

        protected SceneObject2(GameObject prefab)
        {
            Prefab = prefab;
            Components = new List<IComponent>();
        }

        public virtual GameObject Instantiate(Vector3 position, Quaternion rotation, Vector3 scale, Transform parent)
        {
            GameObject = Object.Instantiate(Prefab, position, rotation);
            System.Diagnostics.Debug.Assert(GameObject != null, "GameObject != null");

            GameObject.transform.localScale = scale;
            GameObject.transform.SetParent(parent, false);
            foreach (var component in Components)
                component.Instantiate();

            return GameObject;
        }

        public virtual void Destroy()
        {
            foreach (var component in Components)
                component.Destroy();

            if (GameObject)
            {
                Object.Destroy(GameObject);
                GameObject = null;
            }
        }

        public virtual void FixedUpdate()
        {
            foreach (var component in Components)
                component.Update();
        }
        public virtual void FrameUpdate()
        {
        }

        public void AddComponent(IComponent component)
        {
            if (!Components.Exists(c => c == component))
                Components.Add(component);
        }
        public void RemoveComponent(IComponent component)
        {
            Components.Remove(component);
        }
        public void ClearComponents()
        {
            Components.Clear();
        }

        public TComponentType GetComponent<TComponentType>()
        {
            return GameObject.GetComponent<TComponentType>();
        }
    }
}
