using System.Collections.Generic;
using System.Linq;
using CollaborationEngine.Objects.Components;
using UnityEngine;
using UnityEngine.Networking;

namespace CollaborationEngine.Objects
{
    public abstract class SceneObject
    {
        public class Data : MessageBase
        {
            public Vector3 Position;
            public Quaternion Rotation;
            public Vector3 Scale;
            public SceneObjectType Type;
        };

        public class DataCollection : MessageBase
        {
            public IEnumerable<Data> DataEnumerable { get; set; }

            public override void Serialize(NetworkWriter writer)
            {
                writer.WritePackedUInt32((uint) DataEnumerable.Count());
                foreach (var data in DataEnumerable)
                {
                    writer.Write(data);
                }
            }

            public override void Deserialize(NetworkReader reader)
            {
                var count = reader.ReadPackedUInt32();

                var data = new List<Data>((int) count);
                for (var i = 0; i < count; ++i)
                    data.Add(reader.ReadMessage<Data>());

                DataEnumerable = data;
            }
        }

        public GameObject Prefab { get; set; }
        public GameObject GameObject { get; private set; }

        private readonly List<IComponent> _components = new List<IComponent>();
        public List<IComponent> Components
        {
            get
            {
                return _components;
            }
        }
        public Data NetworkData { get; private set; }

        protected SceneObject(GameObject prefab, SceneObjectType type)
        {
            NetworkData = new Data
            {
                Position = Vector3.zero,
                Rotation = Quaternion.identity,
                Scale = Vector3.one,
                Type = type
            };
            Prefab = prefab;   
        }
        protected SceneObject(GameObject prefab, Data networkData)
        {
            NetworkData = networkData;
            Prefab = prefab;
        }

        public virtual GameObject Instantiate(Transform parent)
        {
            GameObject = Object.Instantiate(Prefab, NetworkData.Position, NetworkData.Rotation);
            System.Diagnostics.Debug.Assert(GameObject != null, "GameObject != null");

            GameObject.transform.localScale = NetworkData.Scale;
            GameObject.transform.SetParent(parent, false);
            foreach (var component in Components)
                component.Instantiate();

            return GameObject;
        }

        public virtual void Destroy()
        {
            ClearComponents();

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
            foreach (var component in Components)
                component.Destroy();

            Components.Clear();
        }

        public TComponentType GetComponent<TComponentType>()
        {
            return GameObject.GetComponent<TComponentType>();
        }
    }
}
