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
        public TaskModel TaskModel { get; set; }
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
                        CreateHierarchyPanel(value.StepModel);
                        CreatePresentStepPanel(value.StepModel);
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

            TaskModel.OnStepAdded += Task_OnStepAdded;
            TaskModel.OnStepDeleted += Task_OnStepDeleted;
            TaskModel.OnStepUpdated += Task_OnStepUpdated;

            ObjectLocator.Instance.HintText.SetText("Create, delete or select a stepModel.");
        }
        public void OnDestroy()
        {
            TaskModel.OnStepUpdated -= Task_OnStepUpdated;
            TaskModel.OnStepDeleted -= Task_OnStepDeleted;
            TaskModel.OnStepAdded -= Task_OnStepAdded;
        }

        private void AddStepItem(StepModel stepModel)
        {
            // Allocate space for new element:
            Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (_stepsItems.Count + 1) * _stepButtonHeight);

            // Add new element:
            var position = new Vector3(0.0f, -stepModel.Order * _stepButtonHeight);
            var stepItem = Instantiate(StepItemPrefab, position, Quaternion.identity);
            stepItem.StepModel = stepModel;
            stepItem.transform.SetParent(Content.transform, false);
            stepItem.OnClicked += StepItem_OnClicked;
            stepItem.OnDeleted += StepItem_OnDeleted;
            stepItem.StepModel.OnOrderChanged += Step_OnOrderChanged;

            // Add to list:
            _stepsItems.Add(stepItem);

            SelectedStepItem = stepItem;
        }
        private void DeleteStepItem(StepModel stepModel)
        {
            // Return if taskModel item does not exist:
            if (!_stepsItems.Exists(element => element.StepModel.ID == stepModel.ID))
                return;

            // Find element:
            var index = _stepsItems.FindIndex(element => element.StepModel.ID == stepModel.ID);
            var stepItem = _stepsItems[index];

            if (stepItem == SelectedStepItem)
                SelectedStepItem = null;

            // Remove from list:
            _stepsItems.RemoveAt(index);

            // Destroy taskModel item:
            stepItem.StepModel.OnOrderChanged -= Step_OnOrderChanged;
            stepItem.OnDeleted -= StepItem_OnDeleted;
            stepItem.OnClicked -= StepItem_OnClicked;
            stepItem.transform.SetParent(null);
            Destroy(stepItem.gameObject);
        }
        private void RecreateStepItem(StepModel stepModel)
        {
            DeleteStepItem(stepModel);
            AddStepItem(stepModel);
        }

        private void CreateHierarchyPanel(StepModel stepModel)
        {
            DestroyHierarchyPanel();

            _hierarchyPanel = Instantiate(HierarchyPanelPrefab);
            _hierarchyPanel.StepModel = stepModel;
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

        private void CreatePresentStepPanel(StepModel stepModel)
        {
            _presentStepPanel = Instantiate(PresentStepPanelPrefab);
            _presentStepPanel.transform.SetParent(ObjectLocator.Instance.UICanvas, false);
            _presentStepPanel.StepsPanel = this;
            _presentStepPanel.StepModel = stepModel;
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
            editStepPanel.TaskModel = TaskModel;
        }
        #endregion

        #region Event Handlers
        private void Task_OnStepAdded(TaskModel sender, TaskModel.StepEventArgs eventArgs)
        {
            AddStepItem(eventArgs.StepModel);
        }
        private void Task_OnStepDeleted(TaskModel sender, TaskModel.StepEventArgs eventArgs)
        {
            DeleteStepItem(eventArgs.StepModel);
        }
        private void Task_OnStepUpdated(TaskModel sender, TaskModel.StepEventArgs eventArgs)
        {
        }
        private void Step_OnOrderChanged(StepModel sender, EventArgs eventArgs)
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
            TaskModel.DeleteStep(sender.StepModel.ID);
        }
        #endregion
    }
}
