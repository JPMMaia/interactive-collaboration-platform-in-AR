using System;
using UnityEngine;

namespace CollaborationEngine.Network
{
    public class NetworkLogger : MonoBehaviour
    {
        public string TextToShow = string.Empty;

        ////using the awake method in a random gameobject on the scene, subscribe to this event by assigning it an event handler. 
        public void Awake()
        {
            Application.logMessageReceived += Application_logMessageReceived;
        }

        //define the event handler that displays the exception on the UI text component 
        private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
        {
            TextToShow = string.Format("{0}, {1}, {2}", condition, stackTrace, type);
        }

        public void OnGUI()
        {
            GUI.Label(new Rect(400, 400, 300, 100), TextToShow);
        }
    }
}
