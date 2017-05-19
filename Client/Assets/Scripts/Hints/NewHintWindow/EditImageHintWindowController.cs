using System;
using CollaborationEngine.Base;

namespace CollaborationEngine.Hints.NewHintWindow
{
    public class EditImageHintWindowController : Controller
    {
        public class WindowDataEventArgs : EventArgs
        {
            public ImageHintType ImageHintType { get; private set; }

        public WindowDataEventArgs(ImageHintType imageHintType)
        {
            ImageHintType = imageHintType;
        }
    }

    public event EventHandler<WindowDataEventArgs> OnEndCreate;

    public EditImageHintWindowView View;

    public ImageHintType SelectedImageHintType
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
