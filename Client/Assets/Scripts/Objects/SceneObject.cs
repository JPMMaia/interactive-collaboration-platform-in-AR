using System;
using System.Collections.Generic;
using System.Linq;
using CollaborationEngine.Objects.Collision;
using CollaborationEngine.Objects.Components;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace CollaborationEngine.Objects
{
    public abstract class SceneObject
    {
        #region Classes
        public class Message : MessageBase
        {
            public SceneObject Data { get; set; }

            public override void Serialize(NetworkWriter writer)
            {
                /*writer.WritePackedUInt32(Data._id);
                writer.Write(Data._name);
                writer.WritePackedUInt32((UInt32)Data._steps.Count);
                foreach (var step in Data._steps)
                {
                    var stepMessage = new Step.StepMessage { Data = step };
                    stepMessage.Serialize(writer);
                }*/
            }
            public override void Deserialize(NetworkReader reader)
            {
                /*Data = new Task { _id = reader.ReadPackedUInt32(), _name = reader.ReadString() };

                var stepLength = (Int32)reader.ReadPackedUInt32();
                Data._steps = new List<Step>(stepLength);
                for (var i = 0; i < stepLength; ++i)
                {
                    var stepMessage = reader.ReadMessage<Step.StepMessage>();
                    Data._steps.Add(stepMessage.Data);
                }*/
            }
        }
        public class CollectionMessage : MessageBase
        {
            public IEnumerable<SceneObject> DataEnumerable { get; set; }

            public override void Serialize(NetworkWriter writer)
            {
                writer.WritePackedUInt32((uint)DataEnumerable.Count());
                foreach (var data in DataEnumerable)
                {
                    writer.Write(new Message() { Data = data });
                }
            }
            public override void Deserialize(NetworkReader reader)
            {
                var count = reader.ReadPackedUInt32();

                var data = new List<SceneObject>((int)count);
                for (var i = 0; i < count; ++i)
                    data.Add(reader.ReadMessage<Message>().Data);

                DataEnumerable = data;
            }
        }
        #endregion

        #region Delegates
        public delegate void SceneObjectDelegate(SceneObject sender, EventArgs eventArgs);
        #endregion

        #region Events
        public event SceneObjectDelegate OnNameChanged;
        #endregion

        #region Properties
        public GameObject Prefab { get; set; }
        public GameObject GameObject { get; private set; }
        public uint ID { get; private set; }
        public String Name
        {
            get { return _name; }
            set
            {
                _name = value;

                if(OnNameChanged != null)
                    OnNameChanged(this, EventArgs.Empty);
            }
        }
        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public Quaternion Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }
        public Vector3 Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }
        public SceneObjectType Type { get; set; }
        public uint Flag { get; set; }
        public bool IsInstanced { get; private set; }
        public List<IComponent> Components
        {
            get
            {
                return _components;
            }
        }
        public bool IsDirty { get; set; }
        public InputColliderComponent<SceneObject> InputCollider { get; private set; }
        #endregion

        #region Members
        private static uint _count;
        private readonly List<IComponent> _components = new List<IComponent>();
        private Vector3 _position = Vector3.zero;
        private Quaternion _rotation = Quaternion.identity;
        private Vector3 _scale = Vector3.one;
        private string _name;
        #endregion

        protected SceneObject()
        {
        }
        protected SceneObject(GameObject prefab, SceneObjectType type)
        {
            ID = _count++;
            Prefab = prefab;
            Type = type;

            InputCollider = new InputColliderComponent<SceneObject>(this);
        }

        public virtual GameObject Instantiate(Transform parent)
        {
            GameObject = Object.Instantiate(Prefab, Position, Rotation);
            System.Diagnostics.Debug.Assert(GameObject != null, "GameObject != null");

            GameObject.transform.localScale = Scale;
            GameObject.transform.SetParent(parent, false);
            foreach (var component in Components)
                component.Instantiate();

            IsInstanced = true;

            return GameObject;
        }
        public virtual void Destroy()
        {
            if (!IsInstanced)
                return;

            IsInstanced = false;

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
            {
                Components.Add(component);

                if (IsInstanced)
                    component.Instantiate();
            }

        }
        public void RemoveComponent(IComponent component)
        {
            Components.Remove(component);

            if (IsInstanced)
                component.Destroy();
        }
        public void ClearComponents()
        {
            if (IsInstanced)
            {
                foreach (var component in Components)
                    component.Destroy();
            }

            Components.Clear();
        }

        public void UpdateTransform(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;

            GameObject.transform.position = position;
            GameObject.transform.rotation = rotation;
            GameObject.transform.localScale = scale;
        }

        public TComponentType GetComponent<TComponentType>()
        {
            return GameObject.GetComponent<TComponentType>();
        }
    }
}
