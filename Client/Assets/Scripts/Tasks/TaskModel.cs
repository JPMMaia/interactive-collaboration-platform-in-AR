using CollaborationEngine.Base;
using CollaborationEngine.Network;
using System;
using System.Collections.Generic;
using CollaborationEngine.Steps;
using CollaborationEngine.Utilities;
using UnityEngine.Networking;

namespace CollaborationEngine.Tasks
{
    public class TaskModel : Entity, ISerializable
    {
        #region Events
        public delegate void StepModelEventDelegate(TaskModel sender, StepEventArgs eventArgs);

        public event StepModelEventDelegate OnStepCreated;
        public event StepModelEventDelegate OnStepDuplicated;
        public event StepModelEventDelegate OnStepDeleted;
        #endregion

        #region Unity Editor
        public StepModel StepModelPrefab;
        #endregion

        #region Properties
        public UInt32 ID { get; set; }
        public String Name { get; set; }
        public IEnumerable<KeyValuePair<uint, StepModel>> Steps
        {
            get { return _steps; }
        }
        #endregion

        #region Members
        private static uint _count;
        private Dictionary<uint, StepModel> _steps = new Dictionary<uint, StepModel>();
        #endregion

        public static uint GenerateID()
        {
            return _count++;
        }

        private StepModel CreateStep()
        {
            // Create new step and assign a unique ID:
            var step = Instantiate(StepModelPrefab);
            step.ID = StepModel.GenerateID();

            // Add step to list:
            _steps.Add(step.ID, step);

            return step;
        }
        public StepModel Create()
        {
            var step = CreateStep();

            // Raise event:
            if (OnStepCreated != null)
                OnStepCreated(this, new StepEventArgs(step));

            return step;
        }
        public StepModel Duplicate(uint stepID)
        {
            // Get step to duplicate:
            var stepToDuplicate = _steps[stepID];

            // Create new step and perform deep copy:
            var duplicatedTask = CreateStep();
            stepToDuplicate.DeepCopy(duplicatedTask);

            // Raise event:
            if (OnStepDuplicated != null)
                OnStepDuplicated(this, new StepEventArgs(duplicatedTask));

            return duplicatedTask;
        }
        public void Delete(uint stepID)
        {
            // Get step:
            StepModel step;
            if (!_steps.TryGetValue(stepID, out step))
                return;

            // Remove step:
            _steps.Remove(stepID);

            // Raise event:
            if (OnStepDeleted != null)
                OnStepDeleted(this, new StepEventArgs(step));
        }
        public StepModel Get(uint stepID)
        {
            return _steps[stepID];
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.WritePackedUInt32(ID);
            writer.Write(Name);
            writer.WritePackedUInt32((UInt32)_steps.Count);
            foreach (var step in _steps)
            {
                step.Value.Serialize(writer);
            }
        }
        public void Deserialize(NetworkReader reader)
        {
            ID = reader.ReadPackedUInt32();
            Name = reader.ReadString();

            var stepLength = (Int32) reader.ReadPackedUInt32();
            _steps = new Dictionary<uint, StepModel>(stepLength);
            for (var i = 0; i < stepLength; ++i)
            {
                var step = Instantiate(StepModelPrefab);
                step.Deserialize(reader);
                _steps.Add(step.ID, step);
            }
        }

        public void DeepCopy(TaskModel other)
        {
            other.Name = CopyUtilities.GenerateCopyName(Name);

            throw new NotImplementedException();
        }
    }
}
