using UnityEngine;

namespace CollaborationEngine.Base
{
    public class Entity : MonoBehaviour
    {
        public Application Application
        {
            get { return FindObjectOfType<Application>(); }
        }
    }
}
