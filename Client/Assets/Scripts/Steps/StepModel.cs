using System;
using System.Collections.Generic;
using System.IO;
using CollaborationEngine.Base;
using CollaborationEngine.Hints;
using CollaborationEngine.Network;
using UnityEngine;

namespace CollaborationEngine.Steps
{
    public class StepModel : Entity, ISerializable
    {
        #region Events
        public delegate void HintModelEventDelegate(StepModel sender, HintEventArgs eventArgs);

        public event HintModelEventDelegate OnHintCreated;
        public event HintModelEventDelegate OnHintDuplicated;
        public event HintModelEventDelegate OnHintDeleted;
        #endregion

        #region Properties

        public UInt32 ID
        {
            get { return _id; }
            set
            {
                _id = value;

                gameObject.name = String.Format("Step Model {0}", value);
            }
        }
        public UInt32 TaskID { get; set; }
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public IEnumerable<KeyValuePair<uint, HintModel>> Hints
        {
            get { return _hints; }
        }
        #endregion

        #region Members
        private static uint _count;
        private uint _id;
        private readonly Dictionary<uint, HintModel> _hints = new Dictionary<uint, HintModel>();
        private string _name = String.Empty;
        #endregion

        public void AssignID()
        {
            ID = _count++;
        }

        private THint InternalCreateHint<THint>(THint prefab) where THint : HintModel
        {
            // CreateStep new hint and assign a unique ID:
            var hint = Instantiate(prefab, transform);
            hint.AssignID();
            hint.TaskID = TaskID;
            hint.StepID = ID;

            // Add hint to list:
            _hints.Add(hint.ID, hint);

            return hint;
        }
        public THint CreateHint<THint>(THint prefab) where THint : HintModel
        {
            var hint = InternalCreateHint(prefab);

            // Raise event:
            if (OnHintCreated != null)
                OnHintCreated(this, new HintEventArgs(hint));

            return hint;
        }
        public HintModel DuplicateHint(uint hintID)
        {
            // Get hint to duplicate:
            var hintToDuplicate = _hints[hintID];

            // Create new hint and perform deep copy:
            var duplicatedHint = hintToDuplicate.DeepCopy(transform, TaskID, ID);
            _hints.Add(duplicatedHint.ID, duplicatedHint);

            // Raise event:
            if (OnHintDuplicated != null)
                OnHintDuplicated(this, new HintEventArgs(duplicatedHint));

            return duplicatedHint;
        }
        public void DeleteHint(uint hintID)
        {
            // GetStep hint:
            HintModel hint;
            if (!_hints.TryGetValue(hintID, out hint))
                return;

            // Remove hint:
            _hints.Remove(hintID);

            // Raise event:
            if (OnHintDeleted != null)
                OnHintDeleted(this, new HintEventArgs(hint));

            Destroy(hint.gameObject);
        }
        public HintModel GetHint(uint hintID)
        {
            return _hints[hintID];
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(ID);
            writer.Write(TaskID);
            writer.Write(Name);

            writer.Write(_hints.Count);
            foreach (var hint in _hints)
            {
                writer.Write((byte)hint.Value.Type);
                hint.Value.Serialize(writer);
            }
        }
        public void Deserialize(BinaryReader reader)
        {
            ID = reader.ReadUInt32();
            TaskID = reader.ReadUInt32();
            Name = reader.ReadString();

            var hintCount = reader.ReadInt32();
            for (var i = 0; i < hintCount; ++i)
            {
                var hintType = (HintType) reader.ReadByte();

                HintModel hint;
                if (hintType == HintType.Text)
                    hint = Instantiate(Application.Prefabs.TextHintModelPrefab, transform);
                else
                    hint = Instantiate(Application.Prefabs.ImageHintModelPrefab, transform);

                hint.Deserialize(reader);

                _hints.Add(hint.ID, hint);
            }

            if (_count <= ID)
                _count = ID + 1;
        }

        public StepModel DeepCopy(Transform parent, uint taskID)
        {
            var stepCopy = Instantiate(this, parent);
            stepCopy.AssignID();

            // Copy properties:
            stepCopy.TaskID = taskID;
            stepCopy.Name = Name;

            // Deep copy hints:
            foreach (var hintModel in _hints.Values)
            {
                // Copy hint:
                var hintCopy = hintModel.DeepCopy(stepCopy.transform, stepCopy.TaskID, stepCopy.ID);

                // Add step copy:
                stepCopy._hints.Add(hintCopy.ID, hintCopy);
            }

            return stepCopy;
        }
    }
}
