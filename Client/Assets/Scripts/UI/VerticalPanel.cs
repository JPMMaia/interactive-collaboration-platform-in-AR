using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.UI
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(VerticalLayoutGroup))]
    public class VerticalPanel : MonoBehaviour
    {
        #region Properties
        public int ChildCount
        {
            get { return Transform.childCount; }
        }
        private RectTransform Transform
        {
            get
            {
                if (_transform == null)
                    _transform = GetComponent<RectTransform>();

                return _transform;
            }
        }
        #endregion

        #region Members
        private RectTransform _transform;
        #endregion

        public void Awake()
        {
            var verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
            verticalLayoutGroup.childControlWidth = true;
            verticalLayoutGroup.childControlHeight = true;
        }

        public void Add(RectTransform item)
        {
            // Calculate new height of contentor:
            var newHeight = Transform.rect.height + item.rect.height;

            // Allocate space for new element:
            Transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);

            // Set item as child:
            item.transform.SetParent(Transform, false);
        }
        public void Remove(RectTransform item)
        {
            item.SetParent(null);

            Transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Transform.rect.height - item.rect.height);
        }
    }
}
