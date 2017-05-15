using System;
using System.IO;
using CollaborationEngine.Base;
using CollaborationEngine.Utilities;

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
        }
        public virtual void Deserialize(BinaryReader reader)
        {
            ID = reader.ReadUInt32();
            TaskID = reader.ReadUInt32();
            StepID = reader.ReadUInt32();
            Name = reader.ReadString();
        }
    }
}
