using System;
using System.Collections.Generic;
using System.Linq;
using CollaborationEngine.Base;
using CollaborationEngine.Network;
using CollaborationEngine.Tasks.Steps;
using UnityEngine.Networking;

namespace CollaborationEngine.Tasks
{
    public class TaskModel : Entity, ISerializable
    {
        #region Classes
        public class TaskMesssage : MessageBase
        {
            public TaskModel Data { get; set; }

            public override void Serialize(NetworkWriter writer)
            {
                Data.Serialize(writer);
            }
            public override void Deserialize(NetworkReader reader)
            {
                Data = new TaskModel();

                Data.Deserialize(reader);
            }
        }
        public class StepEventArgs
        {
            public StepModel StepModel { get; private set; }

            public StepEventArgs(StepModel stepModel)
            {
                StepModel = stepModel;
            }
        }
        #endregion

        #region Delegates
        public delegate void TaskEventDelegate(TaskModel sender, EventArgs eventArgs);
        public delegate void StepEventDelegate(TaskModel sender, StepEventArgs eventArgs);
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
            set { _id = value; }
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
        public List<StepModel> Steps
        {
            get { return _steps; }
        }
        #endregion

        #region Members
        private static uint _count;
        private List<StepModel> _steps = new List<StepModel>();
        private UInt32 _id;
        private string _name;
        #endregion

        public static uint GenerateID()
        {
            return _count++;
        }

        /*public void Update(TaskModel taskModel)
        {
            Name = taskModel.Name;
        }*/

        public void AddStep(String name)
        {
            var step = new StepModel(_id, (UInt32)_steps.Count, name);
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

        public void Serialize(NetworkWriter writer)
        {
            writer.WritePackedUInt32(_id);
            writer.Write(_name);
            writer.WritePackedUInt32((UInt32)_steps.Count);
            foreach (var step in _steps)
            {
                step.Serialize(writer);
            }
        }
        public void Deserialize(NetworkReader reader)
        {
            _id = reader.ReadPackedUInt32();
            _name = reader.ReadString();

            var stepLength = (Int32)reader.ReadPackedUInt32();
            _steps = new List<StepModel>(stepLength);
            for (var i = 0; i < stepLength; ++i)
            {
                var step = new StepModel();
                step.Deserialize(reader);
                _steps.Add(step);
            }
        }

        #region Event Handlers
        private void Step_OnUpdated(StepModel sender, EventArgs eventArgs)
        {
            if (OnStepUpdated != null)
                OnStepUpdated(this, new StepEventArgs(sender));
        }

        public void DeepCopy(TaskModel other)
        {
            other.Name = GenerateCopyName(Name);

            //throw new NotImplementedException();
        }
        private String GenerateCopyName(String originalName)
        {
            return originalName + "- Copy";
        }

        #endregion
    }
}
