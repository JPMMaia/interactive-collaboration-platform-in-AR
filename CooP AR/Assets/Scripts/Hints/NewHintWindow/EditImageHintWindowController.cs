using System;
using CollaborationEngine.Base;

namespace CollaborationEngine.Hints.NewHintWindow
{
    public class EditImageHintWindowController : Controller
    {
        public class WindowDataEventArgs : EventArgs
        {
            public uint ImageHintType { get; private set; }

        public WindowDataEventArgs(uint imageHintType)
        {
            ImageHintType = imageHintType;
        }
    }

    public event EventHandler<WindowDataEventArgs> OnEndCreate;

    public EditImageHintWindowView View;

    public uint SelectedImageHintType
    {
        get { return View.SelectedImageHintType; }
        set
        {
            View.SelectedImageHintType = value;
        }
    }

    public void OnOKClick()
    {
        if (OnEndCreate != null)
            OnEndCreate(this, new WindowDataEventArgs(View.SelectedImageHintType));

        Destroy(gameObject);
    }
}
}
