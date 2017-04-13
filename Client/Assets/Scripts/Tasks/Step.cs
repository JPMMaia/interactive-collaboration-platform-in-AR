using System;
using CollaborationEngine.Objects;
using UnityEngine.Networking;

namespace CollaborationEngine.Tasks
{
    public class Step
    {
        #region Classes
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
        #endregion

        #region Delegate
        public delegate void StepEventDelegate(Step sender, EventArgs eventArgs);
        #endregion

        #region Events
        public event StepEventDelegate OnOrderChanged;
        public event StepEventDelegate OnNameChanged;
        public event StepEventDelegate OnUpdated;
        #endregion

        #region Properties
        public UInt32 ID
        {
            get { return _id; }
        }
        public UInt32 TaskID
        {
            get { return _taskId; }
        }
        public UInt32 Order
        {
            get
            {
                return _order;
            }
            set
            {
                if (_order == value)
                    return;

                _order = value;

                if (OnOrderChanged != null)
                    OnOrderChanged(this, EventArgs.Empty);
            }
        }
        public String Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name == value)
                    return;

                _name = value;

                if (OnNameChanged != null)
                    OnNameChanged(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Members
        private static uint _count;
        private UInt32 _id;
        private UInt32 _taskId;
        private UInt32 _order;
        private string _name;
        #endregion

        private Step()
        {
        }
        public Step(UInt32 taskId, UInt32 order, String name)
        {
            _id = GenerateID();
            _taskId = taskId;
            _order = order;
            _name = name;
        }

        public void Update(Step step)
        {
            Order = step.Order;
            Name = step.Name;

            if(OnUpdated != null)
                OnUpdated(this, EventArgs.Empty);
        }

        public void AddInstruction(SceneObject instruction)
        {
        }

        private static UInt32 GenerateID()
        {
            return _count++;
        }
    }
}
