using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.UserInterface
{
    public class Span : MonoBehaviour
    {
        #region Unity Editor
        public CanvasGroup Content;
        public RawImage DropDownButtonImage;
        public Texture ArrowUp;
        public Texture ArrowDown;
        #endregion

        #region Members
        private bool _collapsed;
        #endregion

        public void OnClick()
        {
            _collapsed = !_collapsed;

            if (_collapsed)
            {
                DropDownButtonImage.texture = ArrowUp;
                Content.alpha = 0.0f;
            }
            else
            {
                DropDownButtonImage.texture = ArrowDown;
                Content.alpha = 1.0f;
            }
        }
    }
}
