using UnityEngine;
using Vuforia;

namespace CollaborationEngine.AugmentedReality
{
    public class TrackableEventHandler : MonoBehaviour, ITrackableEventHandler
    {
        public TrackableBehaviour TrackableBehaviour;

        public void Start()
        {
            TrackableBehaviour.RegisterTrackableEventHandler(this);

            OnTrackingLost();
        }

        public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
        {
            if(newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED)
                OnTrackingFound();
            else
                OnTrackingLost();
        }

        public void OnTrackingFound()
        {
            var renderers = GetComponentsInChildren<Renderer>();
            foreach (var component in renderers)
            {
                component.enabled = component.gameObject.tag != "ClientHide";
            }
        }

        public void OnTrackingLost()
        {
            var renderers = GetComponentsInChildren<Renderer>();
            foreach (var component in renderers)
            {
                component.enabled = component.gameObject.tag != "ClientHide";
            }
        }
    }
}
