using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kudan.AR;

public class KudanEventsHandler : MonoBehaviour
{
    public string TextToShow = string.Empty;
    public GameObject PrefabToInstantiate;

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void OnGUI()
    {
        GUI.Label(new Rect(400, 400, 300, 100), TextToShow);
    }

    public void FoundNewMarker(Trackable marker)
    {
        TextToShow = "Found new marker";
    }

    public void FoundOldMarker(Trackable marker)
    {
        TextToShow = "Found old marker";
    }

    public void LostMarker(Trackable marker)
    {
        TextToShow = "Lost marker";
    }
}
