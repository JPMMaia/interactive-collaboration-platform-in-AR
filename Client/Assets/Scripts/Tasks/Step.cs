using System;
using System.Collections.Generic;
using CollaborationEngine.Network;
using CollaborationEngine.Objects;
using UnityEngine.Networking;

namespace CollaborationEngine.Tasks
{
    public class Step : ISerializable
    {
        #region Classes
        public class InstructionEventArgs : EventArgs
        {
            public SceneObject Instruction { get; set; }
        }
        #endregion

        #region Delegate
        public delegate void StepEventDelegate(Step sender, EventArgs eventArgs);
        public delegate void InstructionEventDelegate(Step sender, InstructionEventArgs eventArgs);
        #endregion

        #region Events
        public event StepEventDelegate OnOrderChanged;
        public event StepEventDelegate OnNameChanged;
        public event StepEventDelegate OnUpdated;
        public event InstructionEventDelegate OnInstructionAdded;
        public event InstructionEventDelegate OnInstructionRemoved;
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
        public List<SceneObject> Instructions
        {
            get { return _instructions; }
        }
        #endregion

        #region Members
        private static uint _count;
        private UInt32 _id;
        private UInt32 _taskId;
        private UInt32 _order;
        private string _name;
        private readonly List<SceneObject> _instructions = new List<SceneObject>();
        #endregion

        public Step()
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

            if (OnUpdated != null)
                OnUpdated(this, EventArgs.Empty);
        }

        public bool AddInstruction(SceneObject instruction)
        {
            if (_instructions.Exists(element => element.ID == instruction.ID))
                return false;

            _instructions.Add(instruction);

            if (OnInstructionAdded != null)
                OnInstructionAdded(this, new InstructionEventArgs { Instruction = instruction });

            return true;
        }
        public void RemoveInstruction(UInt32 instructionID)
        {
            var index = _instructions.FindIndex(element => element.ID == instructionID);
            if (index == -1)
                return;

            var instruction = _instructions[index];
            _instructions.RemoveAt(index);

            if (OnInstructionRemoved != null)
                OnInstructionRemoved(this, new InstructionEventArgs { Instruction = instruction });
        }

        private static UInt32 GenerateID()
        {
            return _count++;
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.WritePackedUInt32(_id);
            writer.WritePackedUInt32(_taskId);
            writer.Write(_name);

            writer.WritePackedUInt32((UInt32)_instructions.Count);
            foreach (var instruction in _instructions)
                instruction.Serialize(writer);
        }
        public void Deserialize(NetworkReader reader)
        {
            _id = reader.ReadPackedUInt32();
            _taskId = reader.ReadPackedUInt32();
            _name = reader.ReadString();

            var instructionCount = reader.ReadPackedUInt32();
            for (var i = 0; i < instructionCount; ++i)
                _instructions.Add(SceneObject.FromNetworkReader(reader));
        }
    }
}
