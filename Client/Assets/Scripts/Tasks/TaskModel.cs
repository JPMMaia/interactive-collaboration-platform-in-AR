using CollaborationEngine.Base;
using CollaborationEngine.Network;
using System;
using System.Collections.Generic;
using System.IO;
using CollaborationEngine.Steps;
using CollaborationEngine.Utilities;

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
        public UInt32 ID
        {
            get { return _id; }
            set
            {
                _id = value;
                _unsaved = true;
            }
        }
        public String Name
        {
            get { return _name; }
            set
            {
                _name = value;
                _unsaved = true;
            }
        }
        public IEnumerable<KeyValuePair<uint, StepModel>> Steps
        {
            get { return _steps; }
        }
        public int StepCount
        {
            get { return _steps.Count; }
        }
        #endregion

        #region Members
        private static uint _count;
        private bool _unsaved;
        private uint _id;
        private string _name;
        private readonly Dictionary<uint, StepModel> _steps = new Dictionary<uint, StepModel>();
        #endregion

        public void AssignID()
        {
            _id = _count++;
        }

        private StepModel InternalCreateStep()
        {
            // CreateStep new step and assign a unique ID:
            var step = Instantiate(StepModelPrefab, transform);
            step.AssignID();
            step.TaskID = ID;

            // Add step to list:
            _steps.Add(step.ID, step);

            return step;
        }
        public StepModel CreateStep()
        {
            var step = InternalCreateStep();

            // Raise event:
            if (OnStepCreated != null)
                OnStepCreated(this, new StepEventArgs(step));

            return step;
        }
        public StepModel DuplicateStep(uint stepID)
        {
            // Get step to duplicate:
            var stepToDuplicate = _steps[stepID];

            // Deep copy:
            var duplicatedStep = stepToDuplicate.DeepCopy(transform, ID);
            _steps.Add(duplicatedStep.ID, duplicatedStep);

            // Raise event:
            if (OnStepDuplicated != null)
                OnStepDuplicated(this, new StepEventArgs(duplicatedStep));

            return duplicatedStep;
        }
        public void DeleteStep(uint stepID)
        {
            // GetStep step:
            StepModel step;
            if (!_steps.TryGetValue(stepID, out step))
                return;

            // Remove step:
            _steps.Remove(stepID);

            // Raise event:
            if (OnStepDeleted != null)
                OnStepDeleted(this, new StepEventArgs(step));
        }
        public StepModel GetStep(uint stepID)
        {
            return _steps[stepID];
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(_id);
            writer.Write(_name);
        }
        public void Deserialize(BinaryReader reader)
        {
            _id = reader.ReadUInt32();
            _name = reader.ReadString();
            _unsaved = false;
        }

        public TaskModel DeepCopy()
        {
            var taskCopy = Instantiate(this, transform.parent);
            taskCopy.AssignID();

            // Copy name:
            taskCopy.Name = CopyUtilities.GenerateCopyName(Name);

            // Deep copy steps:
            foreach (var stepModel in _steps.Values)
            {
                // Copy step:
                var stepCopy = stepModel.DeepCopy(taskCopy.transform, taskCopy.ID);

                // Add step copy:
                taskCopy._steps.Add(stepCopy.ID, stepCopy);
            }

            return taskCopy;
        }

        public void Save(String directory)
        {
            if (!_unsaved)
                return;

            // CreateStep directory if it doesn't exist:
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            // Serialize object:
            var data = new MemoryStream();
            Serialize(new BinaryWriter(data));

            // Write to file:
            var file = directory + "Task.data";
            using (var stream = File.OpenWrite(file))
            {
                using (var binaryStream = new BinaryWriter(stream))
                {
                    binaryStream.Write(data.ToArray(), 0, (int) data.Length);
                }
            }
        }
        public void Load(String directory)
        {
            MemoryStream data;

            // Read file:
            var file = directory + "Task.data";
            using (var stream = File.OpenRead(file))
            {
                using (var binaryStream = new BinaryReader(stream))
                {
                    var bytes = binaryStream.ReadBytes((int)stream.Length);
                    data = new MemoryStream(bytes);
                }
            }

            // Deserialize object:
            Deserialize(new BinaryReader(data));
        }
    }
}
