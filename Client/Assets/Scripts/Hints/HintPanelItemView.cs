using System;
using CollaborationEngine.Base;
using UnityEngine;
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

        public RawImage IconRawImage;
        public InputField NameInputField;
        public CanvasGroup NameInputFieldCanvasGroup;

        public Texture Icon
        {
            get { return IconRawImage.texture; }
            set { IconRawImage.texture = value; }
        }
        public String Name
        {
            get { return NameInputField.text; }
            set { NameInputField.text = value; }
        }

        public void OnNameChange()
        {
            if (OnNameChanged != null)
                OnNameChanged(this, new NameEventArgs(NameInputField.text));
        }
        public void OnNameEndEdit()
        {
            if(OnNameEndedEdit != null)
                OnNameEndedEdit(this, new NameEventArgs(NameInputField.text));

            NameInputFieldCanvasGroup.interactable = false;
            NameInputField.readOnly = true;
        }
        public void OnEditClick()
        {
            NameInputFieldCanvasGroup.interactable = true;
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
