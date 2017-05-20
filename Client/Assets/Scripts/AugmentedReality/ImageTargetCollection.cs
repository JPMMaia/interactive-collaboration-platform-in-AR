using System.Collections.Generic;
using UnityEngine;
using Vuforia;

namespace CollaborationEngine.AugmentedReality
{
    public class ImageTargetCollection : MonoBehaviour
    {
        public ImageTargetBehaviour this[int index]
        {
            get { return _imageTargets[index]; }
        }
        public ImageTargetBehaviour ActivatedImageTarget
        {
            get
            {
                return _activatedImageTarget;
            }
            set
            {
                if(_activatedImageTarget != null)
                    _activatedImageTarget.gameObject.SetActive(false);

                _activatedImageTarget = value;

                if (_activatedImageTarget != null)
                    _activatedImageTarget.gameObject.SetActive(true);
            }
        }
        public GameObject ActivatedSceneRoot
        {
            get
            {
                return _activatedImageTarget ? _activatedImageTarget.transform.GetChild(0).gameObject : null;
            }
        }

        private readonly Dictionary<int, ImageTargetBehaviour> _imageTargets = new Dictionary<int, ImageTargetBehaviour>();
        private ImageTargetBehaviour _activatedImageTarget;

        public void Awake()
        {
            var imageTargets = GetComponentsInChildren<ImageTargetBehaviour>();
            for (var index = 0; index < imageTargets.Length; index++)
            {
                var imageTarget = imageTargets[index];
                imageTarget.gameObject.SetActive(false);
                _imageTargets.Add(index, imageTargets[index]);
            }
        }

        public void ActivateImageTarget(int index)
        {
            ActivatedImageTarget = this[index];
        }
    }
}
