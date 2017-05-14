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
        public class EditEventArgs : EventArgs
        {
            public bool Editing { get; private set; }

            public EditEventArgs(bool editing)
            {
                Editing = editing;
            }
        }

        public event EventHandler<NameEventArgs> OnNameChanged;
        public event EventHandler<NameEventArgs> OnNameEndedEdit;
        public event EventHandler<EditEventArgs> OnEditClicked;
        public event EventHandler OnDuplicateClicked;
        public event EventHandler OnDeleteClicked;

        public RawImage IconRawImage;
        public InputField NameInputField;
        public CanvasGroup NameInputFieldCanvasGroup;
        public RawImage EditButtonRawImage;
        public CanvasGroup EditButtonCanvasGroup;

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

        private bool _editing;

        public void OnNameChange()
        {
            if (OnNameChanged != null)
                OnNameChanged(this, new NameEventArgs(NameInputField.text));
        }
        public void OnNameEndEdit()
        {
            if(OnNameEndedEdit != null)
                OnNameEndedEdit(this, new NameEventArgs(NameInputField.text));
        }
        public void OnEditClick()
        {
            if (!_editing)
            {
                Application.View.MainCanvas.GetComponent<CanvasGroup>().interactable = false;

                NameInputFieldCanvasGroup.interactable = true;
                NameInputFieldCanvasGroup.ignoreParentGroups = true;
                NameInputField.readOnly = false;

                EditButtonCanvasGroup.ignoreParentGroups = true;
                EditButtonRawImage.color = new Color(140.0f / 255.0f, 72.0f / 255.0f, 159.0f / 255.0f, 1.0f);
            }
            else
            {
                EditButtonRawImage.color = Color.white;
                EditButtonCanvasGroup.ignoreParentGroups = false;

                NameInputField.readOnly = true;
                NameInputFieldCanvasGroup.ignoreParentGroups = false;
                NameInputFieldCanvasGroup.interactable = false;

                Application.View.MainCanvas.GetComponent<CanvasGroup>().interactable = true;
            }

            _editing = !_editing;

            if (OnEditClicked != null)
                OnEditClicked(this, new EditEventArgs(_editing));
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
