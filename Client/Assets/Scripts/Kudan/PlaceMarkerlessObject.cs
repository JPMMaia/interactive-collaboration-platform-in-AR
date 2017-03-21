using Kudan.AR;
using UnityEngine;

namespace CollaborationEngine.Kudan
{
    public class PlaceMarkerlessObject : MonoBehaviour
    {
        public KudanTracker KudanTracker;

        public void PlaceClick()
        {
            Vector3 position;
            Quaternion orientation;

            KudanTracker.FloorPlaceGetPose(out position, out orientation);
            KudanTracker.ArbiTrackStart(position, orientation);
        }
    }
}
