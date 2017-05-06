using System;

namespace CollaborationEngine.Base
{
    public class Controller : Entity
    {
        public virtual void OnNotify(String eventID, UnityEngine.Object target, params object[] data)
        {
        }
    }
}
