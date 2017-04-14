using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.UI
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(VerticalLayoutGroup))]
    public class VerticalPanel : MonoBehaviour
    {
        #region Members
        private RectTransform _transform;
        #endregion

        public void Awake()
        {
            var verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
            verticalLayoutGroup.childControlWidth = true;
            verticalLayoutGroup.childControlHeight = true;

            _transform = GetComponent<RectTransform>();
            _transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0.0f);
        }

        public void Add(RectTransform item)
        {
            // Calculate new height of contentor:
            var newHeight = _transform.rect.height + item.rect.height;

            // Allocate space for new element:
            _transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);

            // Set item as child:
            item.transform.SetParent(_transform, false);
        }
        public void Remove(RectTransform item)
        {
            item.SetParent(null);

            _transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _transform.rect.height - item.rect.height);
        }
    }
}
