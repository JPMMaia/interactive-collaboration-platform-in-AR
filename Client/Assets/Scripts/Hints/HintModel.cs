using System;
using System.IO;
using CollaborationEngine.Base;
using UnityEngine;

namespace CollaborationEngine.Hints
{
    public class HintModel : Entity
    {
        #region Properties
        public uint ID { get; set; }
        public uint TaskID { get; set; }
        public uint StepID { get; set; }
        public String Name { get; set; }
        public HintType Type { get; protected set;  }
        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }
        public Quaternion Rotation
        {
            get { return transform.rotation; }
            set { transform.rotation = value; }
        }
        public Vector3 LocalPosition
        {
            get { return transform.localPosition; }
            set { transform.localPosition = value; }
        }
        public Quaternion LocalRotation
        {
            get { return transform.localRotation; }
            set { transform.localRotation = value; }
        }
        public Vector3 LocalScale
        {
            get { return transform.localScale; }
            set { transform.localScale = value; }
        }
        #endregion

        #region Members
        private static uint _count;
        #endregion

        public void AssignID()
        {
            ID = _count++;
        }

        public virtual HintModel DeepCopy(Transform parent, uint taskID, uint stepID)
        {
            throw new NotSupportedException();
        }
        protected void DeepCopy(HintModel copy, uint taskID, uint stepID)
        {
            copy.AssignID();

            // Copy properties:
            copy.TaskID = taskID;
            copy.StepID = stepID;
            copy.Name = Name;
            copy.LocalPosition = LocalPosition;
            copy.LocalRotation = LocalRotation;
            copy.LocalScale = LocalScale;
        }

        public virtual void Serialize(BinaryWriter writer)
        {
            writer.Write(ID);
            writer.Write(TaskID);
            writer.Write(StepID);
            writer.Write(Name);

            writer.Write(LocalPosition.x);
            writer.Write(LocalPosition.y);
            writer.Write(LocalPosition.z);

            writer.Write(LocalRotation.x);
            writer.Write(LocalRotation.y);
            writer.Write(LocalRotation.z);
            writer.Write(LocalRotation.w);

            writer.Write(LocalScale.x);
            writer.Write(LocalScale.y);
            writer.Write(LocalScale.z);
        }
        public virtual void Deserialize(BinaryReader reader)
        {
            ID = reader.ReadUInt32();
            TaskID = reader.ReadUInt32();
            StepID = reader.ReadUInt32();
            Name = reader.ReadString();

            {
                var x = reader.ReadSingle();
                var y = reader.ReadSingle();
                var z = reader.ReadSingle();
                LocalPosition = new Vector3(x, y, z);
            }

            {
                var x = reader.ReadSingle();
                var y = reader.ReadSingle();
                var z = reader.ReadSingle();
                var w = reader.ReadSingle();
                LocalRotation = new Quaternion(x, y, z, w);
            }

            {
                var x = reader.ReadSingle();
                var y = reader.ReadSingle();
                var z = reader.ReadSingle();
                LocalScale = new Vector3(x, y, z);
            }

            if (_count <= ID)
                _count = ID + 1;
        }
    }
}
