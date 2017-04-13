using System;
using CollaborationEngine.Objects;
using CollaborationEngine.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.UI.Steps
{
    public class StepItem : MonoBehaviour
    {
        public delegate void StepEventDelegate(StepItem sender, EventArgs eventArgs);

        public event StepEventDelegate OnClicked;
        public event StepEventDelegate OnDeleted;

        public Text StepNameText;

        private Step _step;

        public void Start()
        {
            StepNameText.text = _step.Name;
        }
        public void OnDestroy()
        {
            if (Step != null)
                Step.OnNameChanged -= Step_OnNameChanged;
        }

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

        private void Step_OnNameChanged(Step sender, EventArgs eventArgs)
        {
            StepNameText.text = _step.Name;
        }
    }
}
