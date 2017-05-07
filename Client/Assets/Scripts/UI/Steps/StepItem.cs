using System;
using CollaborationEngine.Objects;
using CollaborationEngine.Tasks;
using CollaborationEngine.Tasks.Steps;
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
        public StepModel StepModel
        {
            get { return _stepModel; }
            set
            {
                if (_stepModel != null)
                    _stepModel.OnNameChanged -= StepModelOnNameChanged;

                _stepModel = value;

                if (_stepModel != null)
                    _stepModel.OnNameChanged += StepModelOnNameChanged;
            }
        }
        #endregion

        #region Members
        private StepModel _stepModel;
        #endregion

        public void Start()
        {
            StepNameText.text = _stepModel.Name;
        }
        public void OnDestroy()
        {
            if (StepModel != null)
                StepModel.OnNameChanged -= StepModelOnNameChanged;
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
            editTaskPanel.StepModel = StepModel;
        }
        public void OnDeleteClick()
        {
            if (OnDeleted != null)
                OnDeleted(this, EventArgs.Empty);
        }
        #endregion

        #region Event Handlers
        private void StepModelOnNameChanged(StepModel sender, EventArgs eventArgs)
        {
            StepNameText.text = _stepModel.Name;
        }
        #endregion
    }
}
