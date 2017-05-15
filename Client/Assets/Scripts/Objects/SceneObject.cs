using System;
using System.Collections.Generic;
using CollaborationEngine.Network;
using CollaborationEngine.Objects.Components;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace CollaborationEngine.Objects
{
    public abstract class SceneObject
    {
        #region Classes
        public class IDMessage : MessageBase
        {
            public uint ID { get; set; }

            public override void Serialize(NetworkWriter writer)
            {
                writer.WritePackedUInt32(ID);
            }
            public override void Deserialize(NetworkReader reader)
            {
                ID = reader.ReadPackedUInt32();
            }
        }
        public class DataMessage : MessageBase
        {
            public SceneObject Data { get; set; }

            public override void Serialize(NetworkWriter writer)
            {
                Data.Serialize(writer);
            }
            public override void Deserialize(NetworkReader reader)
            {
                var type = Type.GetType(reader.ReadString());
                if (type == null)
                    throw new Exception("Unexpected type.");

                Data = (SceneObject)Activator.CreateInstance(type);

                Data.Deserialize(reader);
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
        public abstract Type Type { get; }
        public GameObject Prefab { get; set; }
        public GameObject GameObject { get; private set; }
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

        public uint ID { get; private set; }
        public String Name
        {
            get { return _name; }
            set
            {
                _name = value;

                if (OnNameChanged != null)
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
        protected SceneObject(GameObject prefab)
        {
            ID = _count++;
            Prefab = prefab;

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

        public virtual void Serialize(NetworkWriter writer)
        {
            writer.Write(Type.FullName);
            writer.WritePackedUInt32(ID);
            writer.Write(Name);
            writer.Write(Position);
            writer.Write(Rotation);
            writer.Write(Scale);
        }
        public virtual void Deserialize(NetworkReader reader)
        {
            ID = reader.ReadPackedUInt32();
            Name = reader.ReadString();
            Position = reader.ReadVector3();
            Rotation = reader.ReadQuaternion();
            Scale = reader.ReadVector3();
        }

        public virtual void Update(SceneObject instruction)
        {
            Position = instruction.Position;
            Rotation = instruction.Rotation;
            Scale = instruction.Scale;

            if (IsInstanced)
            {
                GameObject.transform.localPosition = Position;
                GameObject.transform.localRotation = Rotation;
                GameObject.transform.localScale = Scale;
            }
        }

        public virtual bool PerformNetworkSynch()
        {
            if (!IsInstanced)
                return true;

            var dirty = false;

            if (_position != GameObject.transform.localPosition)
            {
                _position = GameObject.transform.localPosition;
                dirty = true;
            }

            if (_rotation != GameObject.transform.localRotation)
            {
                _rotation = GameObject.transform.localRotation;
                dirty = true;
            }

            if (_scale != GameObject.transform.localScale)
            {
                _scale = GameObject.transform.localScale;
                dirty = true;
            }

            if (dirty)
            {
                var networkClient = NetworkManager.singleton.client;
                networkClient.Send(NetworkHandles.UpdateHintTransform, new DataMessage { Data = this });
            }

            return dirty;
        }
    }
}
