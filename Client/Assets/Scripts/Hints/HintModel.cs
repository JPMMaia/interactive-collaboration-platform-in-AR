using System;
using System.IO;
using CollaborationEngine.Base;
using CollaborationEngine.Utilities;
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
            get { return transform.localPosition; }
            set { transform.localPosition = value; }
        }
        public Quaternion Rotation
        {
            get { return transform.localRotation; }
            set { transform.localRotation = value; }
        }
        public Vector3 Scale
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

        public virtual void DeepCopy(HintModel other)
        {
            other.TaskID = TaskID;
            other.StepID = StepID;
            other.Name = CopyUtilities.GenerateCopyName(Name);
        }

        public virtual void Serialize(BinaryWriter writer)
        {
            writer.Write(ID);
            writer.Write(TaskID);
            writer.Write(StepID);
            writer.Write(Name);

            writer.Write(Position.x);
            writer.Write(Position.y);
            writer.Write(Position.z);

            writer.Write(Rotation.x);
            writer.Write(Rotation.y);
            writer.Write(Rotation.z);
            writer.Write(Rotation.w);

            writer.Write(Scale.x);
            writer.Write(Scale.y);
            writer.Write(Scale.z);
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
                Position = new Vector3(x, y, z);
            }

            {
                var x = reader.ReadSingle();
                var y = reader.ReadSingle();
                var z = reader.ReadSingle();
                var w = reader.ReadSingle();
                Rotation = new Quaternion(x, y, z, w);
            }

            {
                var x = reader.ReadSingle();
                var y = reader.ReadSingle();
                var z = reader.ReadSingle();
                Scale = new Vector3(x, y, z);
            }
        }
    }
}
