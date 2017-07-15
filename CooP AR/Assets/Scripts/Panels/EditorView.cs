using System;
using CollaborationEngine.Base;

namespace CollaborationEngine.Panels
{
    public class EditorView : Entity
    {
        public event EventHandler OnGoBack;

        public void OnGoBackClick()
        {
            if(OnGoBack != null)
                OnGoBack(this, EventArgs.Empty);
        }
    }
}
