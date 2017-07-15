using System;
using UnityEngine;
using Vuforia;

namespace CollaborationEngine.AugmentedReality
{
    public class TrackableEventHandler : MonoBehaviour, ITrackableEventHandler
    {
        public event EventHandler OnTargetFound;
        public event EventHandler OnTargetLost;

        public TrackableBehaviour TrackableBehaviour;

        private Base.Application Application
        {
            get { return FindObjectOfType<Base.Application>(); }
        }

        public void Start()
        {
            if (!Application.IsApprentice)
                return;

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
                component.enabled = true;
            }

            if(OnTargetFound != null)
                OnTargetFound(this, EventArgs.Empty);
        }

        public void OnTrackingLost()
        {
            var renderers = GetComponentsInChildren<Renderer>();
            foreach (var component in renderers)
            {
                component.enabled = false;
            }

            if (OnTargetLost != null)
                OnTargetLost(this, EventArgs.Empty);
        }
    }
}
