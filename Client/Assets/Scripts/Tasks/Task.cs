using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace CollaborationEngine.Tasks
{
    public class Task
    {
        public class TaskMesssage : MessageBase
        {
            public Task Data { get; set; }

            public override void Serialize(NetworkWriter writer)
            {
                writer.WritePackedUInt32(Data._id);
                writer.Write(Data._name);
                writer.WritePackedUInt32((UInt32)Data._steps.Count);
                foreach (var step in Data._steps)
                {
                    var stepMessage = new Step.StepMessage { Data = step };
                    stepMessage.Serialize(writer);
                }
            }
            public override void Deserialize(NetworkReader reader)
            {
                Data = new Task { _id = reader.ReadPackedUInt32(), _name = reader.ReadString() };

                var stepLength = (Int32)reader.ReadPackedUInt32();
                Data._steps = new List<Step>(stepLength);
                for (var i = 0; i < stepLength; ++i)
                {
                    var stepMessage = reader.ReadMessage<Step.StepMessage>();
                    Data._steps.Add(stepMessage.Data);
                }
            }
        }

        public delegate void TaskEventDelegate(Task sender, EventArgs eventArgs);

        public event TaskEventDelegate OnNameChanged;

        #region Properties
        public UInt32 ID
        {
            get { return _id; }
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
        #endregion

        #region Members
        private static uint _count;
        private List<Step> _steps = new List<Step>();
        private UInt32 _id;
        private string _name;
        #endregion

        private static UInt32 GenerateID()
        {
            return _count++;
        }

        private Task()
        {
        }
        public Task(String name)
        {
            _id = GenerateID();
            _name = name;
        }

        public void Update(Task task)
        {
            Name = task.Name;
        }

        public void AddStep(String name)
        {
            _steps.Add(new Step(_id, name));
        }
    }
}
