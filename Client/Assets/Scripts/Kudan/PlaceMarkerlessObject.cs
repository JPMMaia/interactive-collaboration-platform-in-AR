using UnityEngine;
using Kudan.AR;

namespace Assets.Scripts.Assets.Scripts.Kudan
{
    public class PlaceMarkerlessObject : MonoBehaviour
    {
        public KudanTracker _kudanTracker;

        public void PlaceClick()
        {
            Vector3 position;
            Quaternion orientation;

            _kudanTracker.FloorPlaceGetPose(out position, out orientation);
            _kudanTracker.ArbiTrackStart(position, orientation);
        }
    }
}
