using System;
using CollaborationEngine.Base;
using CollaborationEngine.Utilities;

namespace CollaborationEngine.Hints
{
    public class HintModel : Entity
    {
        #region Properties
        public UInt32 ID { get; set; }
        public String Name { get; set; }
        #endregion

        #region Members
        private static uint _count;
        #endregion

        public static uint GenerateID()
        {
            return _count++;
        }

        public virtual void DeepCopy(HintModel other)
        {
            other.Name = CopyUtilities.GenerateCopyName(Name);

            throw new NotImplementedException();
        }
    }
}
