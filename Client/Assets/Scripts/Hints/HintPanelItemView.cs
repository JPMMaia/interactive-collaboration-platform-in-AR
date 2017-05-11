using System;
using CollaborationEngine.Base;
using UnityEngine.UI;

namespace CollaborationEngine.Hints
{
    public class HintPanelItemView : Entity
    {
        public class NameEventArgs : EventArgs
        {
            public String Name { get; private set; }

            public NameEventArgs(String name)
            {
                Name = name;
            }
        }

        public event EventHandler<NameEventArgs> OnNameChanged;
        public event EventHandler<NameEventArgs> OnNameEndedEdit;
        public event EventHandler OnDuplicateClicked;
        public event EventHandler OnDeleteClicked;

        public InputField NameInputField;

        public void OnNameChange()
        {
            if (OnNameChanged != null)
                OnNameChanged(this, new NameEventArgs(NameInputField.text));
        }
        public void OnNameEndEdit()
        {
            if(OnNameEndedEdit != null)
                OnNameEndedEdit(this, new NameEventArgs(NameInputField.text));

            NameInputField.caretWidth = 1;
            NameInputField.readOnly = true;
        }
        public void OnEditClick()
        {
            NameInputField.caretWidth = 3;
            NameInputField.readOnly = false;
            NameInputField.ActivateInputField();
        }
        public void OnDuplicateClick()
        {
            if(OnDuplicateClicked != null)
                OnDuplicateClicked(this, EventArgs.Empty);
        }
        public void OnDeleteClick()
        {
            if(OnDeleteClicked != null)
                OnDeleteClicked(this, EventArgs.Empty);
        }
    }
}
