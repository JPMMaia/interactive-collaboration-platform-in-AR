using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;

namespace CollaborationEngine.Tasks
{
    public class Task
    {
        #region Classes
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
                    step.Serialize(writer);
                }
            }
            public override void Deserialize(NetworkReader reader)
            {
                Data = new Task { _id = reader.ReadPackedUInt32(), _name = reader.ReadString() };

                var stepLength = (Int32)reader.ReadPackedUInt32();
                Data._steps = new List<Step>(stepLength);
                for (var i = 0; i < stepLength; ++i)
                {
                    var step = new Step();
                    step.Deserialize(reader);
                    Data._steps.Add(step);
                }
            }
        }
        public class StepEventArgs
        {
            public Step Step { get; private set; }

            public StepEventArgs(Step step)
            {
                Step = step;
            }
        }
        #endregion

        #region Delegates
        public delegate void TaskEventDelegate(Task sender, EventArgs eventArgs);
        public delegate void StepEventDelegate(Task sender, StepEventArgs eventArgs);
        #endregion

        #region Events
        public event TaskEventDelegate OnNameChanged;
        public event StepEventDelegate OnStepAdded;
        public event StepEventDelegate OnStepUpdated;
        public event StepEventDelegate OnStepDeleted;
        #endregion

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
        public List<Step> Steps
        {
            get { return _steps; }
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
            var step = new Step(_id, (UInt32)_steps.Count, name);
            _steps.Add(step);

            step.OnUpdated += Step_OnUpdated;

            if (OnStepAdded != null)
                OnStepAdded(this, new StepEventArgs(step));
        }
        public void DeleteStep(UInt32 stepId)
        {
            var stepIndex = _steps.FindIndex(element => element.ID == stepId);

            var step = _steps[stepIndex];

            step.OnUpdated -= Step_OnUpdated;

            _steps.RemoveAt(stepIndex);

            {
                var stepsAfter = from element in _steps
                                    where element.Order > step.Order
                                    select element;

                foreach (var stepAfter in stepsAfter)
                    --stepAfter.Order;
            }

            if (OnStepDeleted != null)
                OnStepDeleted(this, new StepEventArgs(step));
        }

        #region Event Handlers
        private void Step_OnUpdated(Step sender, EventArgs eventArgs)
        {
            if (OnStepUpdated != null)
                OnStepUpdated(this, new StepEventArgs(sender));
        }
        #endregion
    }
}
