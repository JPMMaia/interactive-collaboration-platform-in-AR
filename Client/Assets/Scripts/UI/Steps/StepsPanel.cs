using System;
using System.Collections.Generic;
using CollaborationEngine.Objects;
using CollaborationEngine.Tasks;
using CollaborationEngine.UI.Instructions;
using UnityEngine;

namespace CollaborationEngine.UI.Steps
{
    public class StepsPanel : MonoBehaviour
    {
        #region Events
        public event StepItem.StepEventDelegate OnStepItemClicked;
        #endregion

        #region Unity Editor
        public StepItem StepItemPrefab;
        public HierarchyPanel HierarchyPanelPrefab;
        public PresentStepPanel PresentStepPanelPrefab;
        public RectTransform Content;
        #endregion

        #region Properties
        public Task Task { get; set; }
        public StepItem SelectedStepItem
        {
            get
            {
                return _selectedStepItem;
            }
            private set
            {
                if (_selectedStepItem != value)
                {
                    if (_selectedStepItem != null)
                    {
                        _selectedStepItem.SetSelectedAppearance(false);
                        DestroyPresentStepPanel();
                        DestroyHierarchyPanel();
                    }
                        
                    _selectedStepItem = value;

                    if (_selectedStepItem != null)
                    {
                        _selectedStepItem.SetSelectedAppearance(true);
                        CreateHierarchyPanel(value.Step);
                        CreatePresentStepPanel(value.Step);
                    }
                }
                else if(value != null)
                {
                    SelectedStepItem = null;
                }
            }
        }
        #endregion

        #region Members
        private readonly List<StepItem> _stepsItems = new List<StepItem>();
        private StepItem _selectedStepItem;
        private float _stepButtonHeight;
        private HierarchyPanel _hierarchyPanel;
        private PresentStepPanel _presentStepPanel;
        #endregion

        public void Start()
        {
            var stepButtonTransform = StepItemPrefab.GetComponent<RectTransform>();
            _stepButtonHeight = stepButtonTransform.rect.size.y;

            Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0.0f);

            Task.OnStepAdded += Task_OnStepAdded;
            Task.OnStepDeleted += Task_OnStepDeleted;
            Task.OnStepUpdated += Task_OnStepUpdated;

            ObjectLocator.Instance.HintText.SetText("Create, delete or select a step.");
        }
        public void OnDestroy()
        {
            Task.OnStepUpdated -= Task_OnStepUpdated;
            Task.OnStepDeleted -= Task_OnStepDeleted;
            Task.OnStepAdded -= Task_OnStepAdded;
        }

        private void AddStepItem(Step step)
        {
            // Allocate space for new element:
            Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (_stepsItems.Count + 1) * _stepButtonHeight);

            // Add new element:
            var position = new Vector3(0.0f, -step.Order * _stepButtonHeight);
            var stepItem = Instantiate(StepItemPrefab, position, Quaternion.identity);
            stepItem.Step = step;
            stepItem.transform.SetParent(Content.transform, false);
            stepItem.OnClicked += StepItem_OnClicked;
            stepItem.OnDeleted += StepItem_OnDeleted;
            stepItem.Step.OnOrderChanged += Step_OnOrderChanged;

            // Add to list:
            _stepsItems.Add(stepItem);

            SelectedStepItem = stepItem;
        }
        private void DeleteStepItem(Step step)
        {
            // Return if task item does not exist:
            if (!_stepsItems.Exists(element => element.Step.ID == step.ID))
                return;

            // Find element:
            var index = _stepsItems.FindIndex(element => element.Step.ID == step.ID);
            var stepItem = _stepsItems[index];

            if (stepItem == SelectedStepItem)
                SelectedStepItem = null;

            // Remove from list:
            _stepsItems.RemoveAt(index);

            // Destroy task item:
            stepItem.Step.OnOrderChanged -= Step_OnOrderChanged;
            stepItem.OnDeleted -= StepItem_OnDeleted;
            stepItem.OnClicked -= StepItem_OnClicked;
            stepItem.transform.SetParent(null);
            Destroy(stepItem.gameObject);
        }
        private void RecreateStepItem(Step step)
        {
            DeleteStepItem(step);
            AddStepItem(step);
        }

        private void CreateHierarchyPanel(Step step)
        {
            DestroyHierarchyPanel();

            _hierarchyPanel = Instantiate(HierarchyPanelPrefab);
            _hierarchyPanel.Step = step;
            ObjectLocator.Instance.RightPanel.Add(_hierarchyPanel.GetComponent<RectTransform>());
        }
        private void DestroyHierarchyPanel()
        {
            if (_hierarchyPanel == null)
                return;

            ObjectLocator.Instance.RightPanel.Remove(_hierarchyPanel.GetComponent<RectTransform>());
            Destroy(_hierarchyPanel.gameObject);
            _hierarchyPanel = null;
        }

        private void CreatePresentStepPanel(Step step)
        {
            _presentStepPanel = Instantiate(PresentStepPanelPrefab);
            _presentStepPanel.transform.SetParent(ObjectLocator.Instance.UICanvas, false);
            _presentStepPanel.StepsPanel = this;
            _presentStepPanel.Step = step;
        }
        private void DestroyPresentStepPanel()
        {
            if (_presentStepPanel == null)
                return;

            Destroy(_presentStepPanel.gameObject);
            _presentStepPanel = null;
        }

        #region Unity UI Events
        public void OnCreateNewStepButtonClicked()
        {
            var editStepPanel = Instantiate(ObjectLocator.Instance.EditStepPanelPrefab);
            editStepPanel.Task = Task;
        }
        #endregion

        #region Event Handlers
        private void Task_OnStepAdded(Task sender, Task.StepEventArgs eventArgs)
        {
            AddStepItem(eventArgs.Step);
        }
        private void Task_OnStepDeleted(Task sender, Task.StepEventArgs eventArgs)
        {
            DeleteStepItem(eventArgs.Step);
        }
        private void Task_OnStepUpdated(Task sender, Task.StepEventArgs eventArgs)
        {
        }
        private void Step_OnOrderChanged(Step sender, EventArgs eventArgs)
        {
            RecreateStepItem(sender);
        }

        private void StepItem_OnClicked(StepItem sender, EventArgs eventArgs)
        {
            SelectedStepItem = sender;

            if (OnStepItemClicked != null)
                OnStepItemClicked(sender, eventArgs);
        }
        private void StepItem_OnDeleted(StepItem sender, EventArgs eventArgs)
        {
            Task.DeleteStep(sender.Step.ID);
        }
        #endregion
    }
}
