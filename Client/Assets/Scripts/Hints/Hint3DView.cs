using CollaborationEngine.Base;
using UnityEngine;

namespace CollaborationEngine.Hints
{
    public class Hint3DView : Entity
    {
        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }
        public Quaternion Rotation
        {
            get { return transform.rotation; }
            set { transform.rotation = value; }
        }
        public Vector3 LocalPosition
        {
            get { return transform.localPosition; }
            set { transform.localPosition = value; }
        }
        public Quaternion LocalRotation
        {
            get { return transform.localRotation; }
            set { transform.localRotation = value; }
        }
        public Vector3 LocalScale
        {
            get { return transform.localScale; }
            set { transform.localScale = value; }
        }
        public bool Showing
        {
            get
            {
                return gameObject.activeInHierarchy;
            }
            set
            {
                gameObject.SetActive(value);
            }
        }
    }
}
