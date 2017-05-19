using CollaborationEngine.Core;
using UnityEngine;

namespace CollaborationEngine.Base
{
    public class Application : MonoBehaviour
    {
        public Model Model;
        public View View;
        public Controller Controller;
        public Prefabs Prefabs;

        public bool IsMentor
        {
            get { return Controller.GetComponentInChildren<MentorController>().gameObject.activeInHierarchy; }
        }
        public bool IsApprentice
        {
            get { return Controller.GetComponentInChildren<ApprenticeController>().gameObject.activeInHierarchy; }
        }
    }
}
