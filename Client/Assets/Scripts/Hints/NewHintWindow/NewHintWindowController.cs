using System;
using CollaborationEngine.Base;

namespace CollaborationEngine.Hints.NewHintWindow
{
    public class NewHintWindowController : Controller
    {
        public class WindowDataEventArgs : EventArgs
        {
            public String Name { get; private set; }
            public HintType HintType { get; private set; }
            public ImageHintType ImageHintType { get; private set; }

            public WindowDataEventArgs(String name, HintType hintType, ImageHintType imageHintType)
            {
                Name = name;
                HintType = hintType;
                ImageHintType = imageHintType;
            }
        }

        public event EventHandler<WindowDataEventArgs> OnEndCreate;

        public NewHintWindowView View;

        public void OnOKClick()
        {
            // Ensure that name is not empty:
            if (View.Name.Length == 0)
            {
                View.NameInputField.ActivateInputField();
                return;
            }

            if(OnEndCreate != null)
                OnEndCreate(this, new WindowDataEventArgs(View.Name, View.HintType, View.SelectedImageHintType));

            Destroy(gameObject);
        }
    }
}
