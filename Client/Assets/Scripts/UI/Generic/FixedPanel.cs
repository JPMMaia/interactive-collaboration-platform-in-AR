using UnityEngine;

namespace CollaborationEngine.UI.Generic
{
    [RequireComponent(typeof(VerticalPanel))]
    public class FixedPanel : MonoBehaviour
    {
        #region Properties
        private VerticalPanel VerticalPanel
        {
            get
            {
                if (_verticalPanel == null)
                    _verticalPanel = GetComponent<VerticalPanel>();

                return _verticalPanel;
            }
            set { _verticalPanel = value; }
        }
        #endregion

        #region Members
        private VerticalPanel _verticalPanel;
        private Vector2 _offsetMax;
        private Vector2 _offsetMin;
        #endregion

        public void Awake()
        {
            var rectTransform = GetComponent<RectTransform>();
            _offsetMax = rectTransform.offsetMax;
            _offsetMin = rectTransform.offsetMin;

            if(VerticalPanel.ChildCount == 0)
                gameObject.SetActive(false);
        }

        public void Add(RectTransform itemTransform)
        {
            VerticalPanel.Add(itemTransform);

            if (!gameObject.activeInHierarchy)
                gameObject.SetActive(true);

            ResetSize();
        }
        public void Remove(RectTransform itemTransform)
        {
            VerticalPanel.Remove(itemTransform);

            if (VerticalPanel.ChildCount == 0)
                gameObject.SetActive(false);

            ResetSize();
        }

        private void ResetSize()
        {
            var rectTransform = GetComponent<RectTransform>();
            rectTransform.offsetMax = _offsetMax;
            rectTransform.offsetMin = _offsetMin;
        }
    }
}
