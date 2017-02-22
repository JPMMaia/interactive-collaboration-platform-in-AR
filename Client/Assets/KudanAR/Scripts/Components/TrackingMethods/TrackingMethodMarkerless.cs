using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Kudan.AR
{
	[DisallowMultipleComponent]
	[AddComponentMenu("Kudan AR/Tracking Methods/Markerless Tracking")]
	/// <summary>
	/// The Markerless Tracking Method. This method tracks objects using arbitrary tracking.
	/// </summary>
	public class TrackingMethodMarkerless : TrackingMethodBase
	{
		/// <summary>
		/// The name of this tracking method.
		/// </summary>
		/// <value>The name of this tracking method is "Markerless".</value>
		public override string Name
		{
			get { return "Markerless"; }
		}

		/// <summary>
		/// The ID of this tracking method.
		/// </summary>
		/// <value>The ID of this tracking method is 1.</value>
		public override int TrackingMethodId
		{
			get { return 1; }
		}

		/// <summary>
		/// Event triggered each frame that ArbiTrack is running.
		/// </summary>
		public MarkerUpdateEvent _updateMarkerEvent;

		/// <summary>
		/// The ArbiTrack floor depth. Default value of 200.0f. Changing this number will affect the overall distance of the floor relative to the device.
		/// </summary>
		public float _floorDepth = 200.0f;

		/// <summary>
		/// Processes the current frame and updates the state of ArbiTrack.
		/// </summary>
		public override void ProcessFrame()
		{
            Vector3 position;
            Quaternion orientation;

            _kudanTracker.ArbiTrackGetPose(out position, out orientation);

            Trackable trackable = new Trackable();
            trackable.position = position;
            trackable.orientation = orientation;

            trackable.isDetected = _kudanTracker.ArbiTrackIsTracking();

            _updateMarkerEvent.Invoke(trackable);
		}

		/// <summary>
		/// Starts ArbiTrack.
		/// Sets the floor height using floorDepth, so changing floorDepth at runtime will not immediately affect tracking.
		/// Tracking must be stopped and started again in order to see the difference.
		/// </summary>
		public override void StartTracking()
		{
			_kudanTracker.SetArbiTrackFloorHeight (_floorDepth);

			base.StartTracking ();
		}
			
		/// <summary>
		/// Stops ArbiTrack.
		/// </summary>
        public override void StopTracking()
        {
            base.StopTracking();
            Trackable trackable = new Trackable();
            trackable.isDetected = false;

            _updateMarkerEvent.Invoke(trackable);
        }
	}
}
