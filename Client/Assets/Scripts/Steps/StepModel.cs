using System;
using System.Collections.Generic;
using System.IO;
using CollaborationEngine.Base;
using CollaborationEngine.Hints;
using CollaborationEngine.Network;
using CollaborationEngine.Utilities;

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
        public UInt32 ID { get; set; }
        public UInt32 TaskID { get; set; }
        public String Name { get; set; }
        public IEnumerable<KeyValuePair<uint, HintModel>> Hints
        {
            get { return _hints; }
        }
        #endregion

        #region Members
        private static uint _count;
        private readonly Dictionary<uint, HintModel> _hints = new Dictionary<uint, HintModel>();
        #endregion

        public static uint GenerateID()
        {
            return _count++;
        }

        private THint InternalCreateHint<THint>(THint prefab) where THint : HintModel
        {
            // CreateStep new hint and assign a unique ID:
            var hint = Instantiate(prefab, transform);
            hint.ID = HintModel.GenerateID();
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
            // GetStep hint to duplicate:
            var hintToDuplicate = _hints[hintID];

            // CreateStep new hint and perform deep copy:
            var duplicatedTask = InternalCreateHint(hintToDuplicate);
            hintToDuplicate.DeepCopy(duplicatedTask);

            // Raise event:
            if (OnHintDuplicated != null)
                OnHintDuplicated(this, new HintEventArgs(duplicatedTask));

            return duplicatedTask;
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
        }
        public HintModel GetHint(uint hintID)
        {
            return _hints[hintID];
        }

        public void Serialize(BinaryWriter writer)
        {
            throw new NotImplementedException();

            /*writer.WritePackedUInt32(ID);
            writer.WritePackedUInt32(TaskID);
            writer.Write(Name);

            writer.WritePackedUInt32((UInt32)_hints.Count);
            //foreach (var hint in _hints)
//                hint.Value.Serialize(writer);*/
        }
        public void Deserialize(BinaryReader reader)
        {
            throw new NotImplementedException();

            /*ID = reader.ReadPackedUInt32();
            TaskID = reader.ReadPackedUInt32();
            Name = reader.ReadString();

            /*var hintCount = reader.ReadPackedUInt32();
            for (var i = 0; i < hintCount; ++i)
            {
                
                var hint = Instantiate(HintModelPrefab);
                hint.Deserialize(reader);
                _hints.Add(hint.ID, hint);
            }*/
        }

        public void DeepCopy(StepModel other)
        {
            other.Name = CopyUtilities.GenerateCopyName(Name);

            throw new NotImplementedException();
        }
    }
}
