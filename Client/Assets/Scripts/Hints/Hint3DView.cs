using CollaborationEngine.Base;
using UnityEngine;

namespace CollaborationEngine.Hints
{
    public class Hint3DView : Entity
    {
        public Vector3 Position
        {
            get { return transform.localPosition; }
            set { transform.localPosition = value; }
        }
        public Quaternion Rotation
        {
            get { return transform.localRotation; }
            set { transform.localRotation = value; }
        }
        public Vector3 Scale
        {
            get { return transform.localScale; }
            set { transform.localScale = value; }
        }
    }
}
