using System;
using CollaborationEngine.Objects;
using CollaborationEngine.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.UI.Instructions
{
    public class InstructionItem : MonoBehaviour
    {
        #region Delegates
        public delegate void IntructionItemEventDelegate(InstructionItem sender, EventArgs eventArgs);
        #endregion

        #region Events
        public event IntructionItemEventDelegate OnClicked;
        public event IntructionItemEventDelegate OnDeleted;
        #endregion

        #region Unity Editor
        public Text IntructionNameText;
        public Image InstructionButtonImage;
        #endregion

        #region Properties
        public Step Step { get; set; }
        #endregion

        #region Members

        #endregion

        public void Start()
        {
            IntructionNameText.text = Step.Name;
        }
        public void OnDestroy()
        {
        }

        public void SetSelectedAppearance(bool enable)
        {
            InstructionButtonImage.color = enable ? Color.green : Color.white;
        }

        #region Unity UI
        public void OnStepClick()
        {
            if (OnClicked != null)
                OnClicked(this, EventArgs.Empty);
        }
        public void OnEditClick()
        {
            var editTaskPanel = Instantiate(ObjectLocator.Instance.EditInstructionPanelPrefab);
            editTaskPanel.Step = Step;
        }
        public void OnDeleteClick()
        {
            if (OnDeleted != null)
                OnDeleted(this, EventArgs.Empty);
        }
        #endregion

        #region Event Handlers
        private void Step_OnNameChanged(Step sender, EventArgs eventArgs)
        {
            IntructionNameText.text = Step.Name;
        }
        #endregion
    }
}
