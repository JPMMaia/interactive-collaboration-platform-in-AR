using System;
using CollaborationEngine.Base;
using CollaborationEngine.UserInterface;
using UnityEngine.UI;

namespace CollaborationEngine.Steps
{
    public class StepView : Entity
    {
        #region Events
        public class ShowEventArgs : EventArgs
        {
            public uint StepID { get; private set; }

            public ShowEventArgs(uint stepID)
            {
                StepID = stepID;
            }
        }
        public class EndEditEventArgs : EventArgs
        {
            public String Text { get; private set; }

            public EndEditEventArgs(String text)
            {
                Text = text;
            }
        }

        public event EventHandler<ShowEventArgs> OnShowClicked;
        public event EventHandler<EndEditEventArgs> OnDescriptionEndedEdit;
        #endregion

        #region Unity Editor
        public Text NameText;
        public InputField DescriptionInputField;
        public RadioButtonView ShowRadioButton;
        #endregion

        #region Properties
        public uint StepID { get; set; }
        public uint StepOrder
        {
            get { return _stepOrder; }
            set
            {
                _stepOrder = value;
                UpdateName();
            }
        }
        public String StepDescription
        {
            get { return DescriptionInputField.text; }
            set { DescriptionInputField.text = value; }
        }
        public bool Showing
        {
            get
            {
                return ShowRadioButton.Selected;
            }
            set
            {
                ShowRadioButton.Selected = value;
            }
        }
        #endregion

        #region Members
        private uint _stepOrder;
        #endregion

        private void UpdateName()
        {
            NameText.text = String.Format("STEP {0}", StepOrder);
        }

        public void OnShowClick()
        {
            if (OnShowClicked != null)
                OnShowClicked(this, new ShowEventArgs(StepID));
        }

        public void OnDescriptionEndEdit()
        {
            if(OnDescriptionEndedEdit != null)
                OnDescriptionEndedEdit(this, new EndEditEventArgs(DescriptionInputField.text));
        }
    }
}
