using System;
using CollaborationEngine.Base;
using UnityEngine.UI;

namespace CollaborationEngine.Panels
{
    public class ARApprenticeView : Entity
    {
        public event EventHandler OnNeedMoreInstructionsClicked;
        public event EventHandler OnCompletedTheStepClicked;

        public Text HeaderText;
        public Button MoreInstructionsButton;
        public Button StepCompletedButton;

        public void OnNeedMoreInstructionsClick()
        {
            if(OnNeedMoreInstructionsClicked != null)
                OnNeedMoreInstructionsClicked(this, EventArgs.Empty);
        }
        public void OnCompletedTheStepClick()
        {
            if (OnCompletedTheStepClicked != null)
                OnCompletedTheStepClicked(this, EventArgs.Empty);
        }
    }
}
