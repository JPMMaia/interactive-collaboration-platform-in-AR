using System;
using UnityEngine;

namespace CollaborationEngine.Base
{
    public class Application : MonoBehaviour
    {
        public Model Model;
        public View View;
        public Controller Controller;
        public Prefabs Prefabs;

        public void Notify(String eventPath, UnityEngine.Object target, params object[] data)
        {
            var controllers = GetAllControllers();
            foreach (var controller in controllers)
                controller.OnNotify(eventPath, target, data);
        }

        private Controller[] GetAllControllers()
        {
            return FindObjectsOfType<Controller>();
        }
    }
}
