using System;
using CollaborationEngine.Base;
using CollaborationEngine.Utilities;

namespace CollaborationEngine.Hints
{
    public class HintModel : Entity
    {
        #region Properties
        public uint ID { get; set; }
        public uint TaskID { get; set; }
        public uint StepID { get; set; }
        public String Name { get; set; }
        #endregion

        #region Members
        private static uint _count;
        #endregion

        public void AssignID()
        {
            ID = _count++;
        }

        public virtual void DeepCopy(HintModel other)
        {
            other.TaskID = TaskID;
            other.StepID = StepID;
            other.Name = CopyUtilities.GenerateCopyName(Name);
        }
    }
}
