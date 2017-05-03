using System;
using CollaborationEngine.Objects;
using CollaborationEngine.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.UI.Steps
{
    public class StepItem : MonoBehaviour
    {
        #region Delegates
        public delegate void StepEventDelegate(StepItem sender, EventArgs eventArgs);
        #endregion

        #region Events
        public event StepEventDelegate OnClicked;
        public event StepEventDelegate OnDeleted;
        #endregion

        #region Unity Editor
        public Text StepNameText;
        public Image StepButtonImage;
        #endregion

        #region Properties
        public Step Step
        {
            get { return _step; }
            set
            {
                if (_step != null)
                    _step.OnNameChanged -= Step_OnNameChanged;

                _step = value;

                if (_step != null)
                    _step.OnNameChanged += Step_OnNameChanged;
            }
        }
        #endregion

        #region Members
        private Step _step;
        #endregion

        public void Start()
        {
            StepNameText.text = _step.Name;
        }
        public void OnDestroy()
        {
            if (Step != null)
                Step.OnNameChanged -= Step_OnNameChanged;
        }

        public void SetSelectedAppearance(bool enable)
        {
            StepButtonImage.color = enable ? Color.green : Color.white;
        }

        #region Unity UI
        public void OnStepClick()
        {
            if (OnClicked != null)
                OnClicked(this, EventArgs.Empty);
        }
        public void OnEditClick()
        {
            var editTaskPanel = Instantiate(ObjectLocator.Instance.EditStepPanelPrefab);
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
            StepNameText.text = _step.Name;
        }
        #endregion
    }
}
