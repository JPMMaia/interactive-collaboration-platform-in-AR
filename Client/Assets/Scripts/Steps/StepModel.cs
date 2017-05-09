﻿using System;
using System.Collections.Generic;
using CollaborationEngine.Base;
using CollaborationEngine.Hints;
using CollaborationEngine.Network;
using CollaborationEngine.Utilities;
using UnityEngine.Networking;

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

        #region Unity Editor
        public HintModel HintModelPrefab;
        #endregion

        #region Properties
        public UInt32 ID { get; set; }
        public UInt32 TaskID { get; private set; }
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

        private THint CreateHint<THint>(THint prefab) where THint : HintModel
        {
            // Create new hint and assign a unique ID:
            var hint = Instantiate(prefab);
            hint.ID = HintModel.GenerateID();

            // Add hint to list:
            _hints.Add(hint.ID, hint);

            return hint;
        }
        public THint Create<THint>(THint prefab) where THint : HintModel
        {
            var hint = CreateHint(prefab);

            // Raise event:
            if (OnHintCreated != null)
                OnHintCreated(this, new HintEventArgs(hint));

            return hint;
        }
        public HintModel Duplicate(uint hintID)
        {
            // Get hint to duplicate:
            var hintToDuplicate = _hints[hintID];

            // Create new hint and perform deep copy:
            var duplicatedTask = CreateHint(hintToDuplicate);
            hintToDuplicate.DeepCopy(duplicatedTask);

            // Raise event:
            if (OnHintDuplicated != null)
                OnHintDuplicated(this, new HintEventArgs(duplicatedTask));

            return duplicatedTask;
        }
        public void Delete(uint hintID)
        {
            // Get hint:
            HintModel hint;
            if (!_hints.TryGetValue(hintID, out hint))
                return;

            // Remove hint:
            _hints.Remove(hintID);

            // Raise event:
            if (OnHintDeleted != null)
                OnHintDeleted(this, new HintEventArgs(hint));
        }
        public HintModel Get(uint hintID)
        {
            return _hints[hintID];
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.WritePackedUInt32(ID);
            writer.WritePackedUInt32(TaskID);
            writer.Write(Name);

            writer.WritePackedUInt32((UInt32)_hints.Count);
            throw new NotImplementedException();
            //foreach (var hint in _hints)
//                hint.Value.Serialize(writer);
        }
        public void Deserialize(NetworkReader reader)
        {
            ID = reader.ReadPackedUInt32();
            TaskID = reader.ReadPackedUInt32();
            Name = reader.ReadString();

            throw new NotImplementedException();
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
