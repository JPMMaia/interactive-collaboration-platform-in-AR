using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Kudan.AR
{
	/// <summary>
	/// The base tracking method that other tracking methods inherit from.
	/// </summary>
	public abstract class TrackingMethodBase : MonoBehaviour
	{
		/// <summary>
		/// Reference to the Kudan Tracker.
		/// </summary>
		public KudanTracker _kudanTracker;

		/// <summary>
		/// The name of the tracking method.
		/// </summary>
		/// <value>The tracking method's name.</value>
		public abstract string Name { get; }

		/// <summary>
		/// The ID of this tracking method.
		/// </summary>
		/// <value>The tracking method identifier.</value>
		public abstract int TrackingMethodId { get; }

		/// <summary>
		/// Whether or not tracking is currently enabled for this tracking method.
		/// </summary>
		protected bool _isTrackingEnabled;

		/// <summary>
		/// Gets the plugin interface.
		/// </summary>
		/// <value>The plugin.</value>
		public TrackerBase Plugin
		{
			get { return _kudanTracker.Interface; }
		}

		/// <summary>
		/// Gets whether or not tracking is currently enabled for this tracking method.
		/// </summary>
		/// <value><c>true</c> if tracking is enabled; otherwise, <c>false</c>.</value>
		public bool TrackingEnabled
		{
			get { return _isTrackingEnabled; }
		}

		/// <summary>
		/// Awake is called once when the scene loads.
		/// </summary>
		void Awake()
		{
			if (_kudanTracker == null)
			{
				_kudanTracker = FindObjectOfType<KudanTracker>();
			}
			if (_kudanTracker == null)
			{
				Debug.LogWarning("[KudanAR] Cannot find KudanTracker in scene", this);
			}
		}

		/// <summary>
		/// Initialise this tracking method.
		/// </summary>
		public virtual void Init()
		{
		}

		/// <summary>
		/// Start tracking using this tracking method.
		/// </summary>
		public virtual void StartTracking()
		{
			if (Plugin != null)
			{
				if (Plugin.EnableTrackingMethod(TrackingMethodId))
				{
					_isTrackingEnabled = true;
				}
				else
				{
					Debug.LogError(string.Format("[KudanAR] Tracking method {0} not supported", TrackingMethodId));
				}
			}
		}

		/// <summary>
		/// Stop tracking with this tracking method.
		/// </summary>
		public virtual void StopTracking()
		{
			if (Plugin != null)
			{
				if (Plugin.DisableTrackingMethod(TrackingMethodId))
				{
					_isTrackingEnabled = false;
				}
				else
				{
					Debug.LogError(string.Format("[KudanAR] Tracking method {0} not supported", TrackingMethodId));
				}
			}
		}

		/// <summary>
		/// Processes the current frame.
		/// </summary>
		public virtual void ProcessFrame()
		{
		}

		/// <summary>
		/// Draws the debug GUI at the given scale.
		/// </summary>
		/// <param name="uiScale">Scale of the GUI.</param>
		public virtual void DebugGUI(int uiScale)
		{
		}
	}
}