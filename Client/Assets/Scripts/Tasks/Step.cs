using System;
using UnityEngine.Networking;

namespace CollaborationEngine.Tasks
{
    public class Step
    {
        public class StepMessage : MessageBase
        {
            public Step Data { get; set; }

            public override void Serialize(NetworkWriter writer)
            {
                writer.WritePackedUInt32(Data._id);
                writer.WritePackedUInt32(Data._taskId);
                writer.Write(Data._name);
            }
            public override void Deserialize(NetworkReader reader)
            {
                Data = new Step
                {
                    _id = reader.ReadPackedUInt32(),
                    _taskId = reader.ReadPackedUInt32(),
                    _name = reader.ReadString()
                };
            }
        }

        public delegate void StepEventDelegate(Step sender, EventArgs eventArgs);

        public event StepEventDelegate OnNameChanged;

        public UInt32 ID
        {
            get { return _id; }
        }
        public UInt32 TaskID
        {
            get { return _taskId; }   
        }
        public String Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;

                if (OnNameChanged != null)
                    OnNameChanged(this, EventArgs.Empty);
            }
        }

        private static UInt32 GenerateID()
        {
            return _count++;
        }

        private Step()
        {
        }
        public Step(UInt32 taskId, String name)
        {
            _id = GenerateID();
            _taskId = taskId;
            _name = name;
        }

        public void Update(Task task)
        {
            Name = task.Name;
        }

        private static uint _count;
        private UInt32 _id;
        private UInt32 _taskId;
        private string _name;
    }
}
