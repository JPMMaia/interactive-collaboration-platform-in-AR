using System;
using CollaborationEngine.Base;

namespace CollaborationEngine.Panels
{
    public class TransformPanelView : Entity
    {
        public event EventHandler OnTranslateClicked;
        public event EventHandler OnRotateClicked;
        public event EventHandler OnScaleClicked;

        public void OnTranslateClick()
        {
            if(OnTranslateClicked != null)
                OnTranslateClicked(this, EventArgs.Empty);
        }
        public void OnRotateClick()
        {
            if (OnRotateClicked != null)
                OnRotateClicked(this, EventArgs.Empty);
        }
        public void OnScaleClick()
        {
            if (OnScaleClicked != null)
                OnScaleClicked(this, EventArgs.Empty);
        }
    }
}
