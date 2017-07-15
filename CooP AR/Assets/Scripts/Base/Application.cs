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
            get
            {
                var component = Controller.GetComponentInChildren<MentorController>();
                return component && component.gameObject.activeInHierarchy;
            }
        }
        public bool IsApprentice
        {
            get
            {
                var component = Controller.GetComponentInChildren<ApprenticeController>();
                return component && component.gameObject.activeInHierarchy;
            }
        }
    }
}
