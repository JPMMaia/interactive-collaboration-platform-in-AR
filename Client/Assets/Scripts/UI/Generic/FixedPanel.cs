using UnityEngine;

namespace CollaborationEngine.UI.Generic
{
    [RequireComponent(typeof(VerticalPanel))]
    public class FixedPanel : MonoBehaviour
    {
        #region Members
        private VerticalPanel _verticalPanel;
        #endregion

        public void Awake()
        {
            _verticalPanel = GetComponent<VerticalPanel>();
            gameObject.SetActive(false);
        }

        public void Add(RectTransform itemTransform)
        {
            _verticalPanel.Add(itemTransform);

            if (!gameObject.activeInHierarchy)
                gameObject.SetActive(true);

            ResetSize();
        }
        public void Remove(RectTransform itemTransform)
        {
            _verticalPanel.Remove(itemTransform);

            if(_verticalPanel.transform.childCount == 0)
                gameObject.SetActive(false);

            ResetSize();
        }

        private void ResetSize()
        {
            var rectTransform = GetComponent<RectTransform>();
            rectTransform.offsetMax = new Vector2(0.0f, 0.0f);
            rectTransform.offsetMin = new Vector2(-rectTransform.rect.width, 0.0f);
        }
    }
}
