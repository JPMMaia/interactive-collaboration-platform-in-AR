using UnityEngine;

public class NetworkLogger : MonoBehaviour
{
    public string textToShow = string.Empty;

    ////using the awake method in a random gameobject on the scene, subscribe to this event by assigning it an event handler. 
    void Awake()
    {
        Application.logMessageReceived += Application_logMessageReceived;
    }

    //define the event handler that displays the exception on the UI text component 
    private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
        textToShow = string.Format("{0}, {1}, {2}", condition, stackTrace, type);
    }

    public void OnGUI()
    {
        GUI.Label(new Rect(400, 400, 300, 100), textToShow);
    }
}
