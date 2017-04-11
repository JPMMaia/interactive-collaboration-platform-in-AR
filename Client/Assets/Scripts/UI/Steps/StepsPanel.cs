using System;
using System.Collections.Generic;
using CollaborationEngine.Tasks;
using UnityEngine;

namespace CollaborationEngine.UI.Steps
{
    public class StepsPanel: MonoBehaviour
    {
        public event StepItem.StepEventDelegate OnStepItemClicked;

        public StepItem StepItemPrefab;
        public RectTransform Content;

        public void Start()
        {
            var taskButtonTransform = StepItemPrefab.GetComponent<RectTransform>();
            _stepButtonHeight = taskButtonTransform.rect.size.y;

            Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0.0f);
        }
        public void OnDestroy()
        {
        }

        public TaskManager TaskManager { get; set; }

        private readonly List<StepItem> _stepsItems = new List<StepItem>();
        private float _stepButtonHeight;

        private void TaskManager_OnTaskAdded(TaskManager sender, TaskManager.TaskEventArgs eventArgs)
        {
            // Allocate space for new element:
            Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (_stepsItems.Count + 1) * _stepButtonHeight);

            // Add new element:
            var position = new Vector3(0.0f, -_stepsItems.Count * _stepButtonHeight);
            var taskItem = Instantiate(StepItemPrefab, position, Quaternion.identity);
            taskItem.Step = eventArgs.Step;
            taskItem.transform.SetParent(Content.transform, false);
            taskItem.OnClicked += TaskItem_OnClicked;
            taskItem.OnDeleted += TaskItem_OnDeleted;

            // Add to list:
            _stepsItems.Add(taskItem);
        }
        private void TaskManager_OnTaskRemoved(TaskManager sender, TaskManager.TaskEventArgs eventArgs)
        {
            // Return if task item does not exist:
            if (!_stepsItems.Exists(element => element.Task.Name == eventArgs.Task.Name))
                return;

            // Find element:
            var index = _stepsItems.FindIndex(element => element.Task.Name == eventArgs.Task.Name);
            var taskItem = _stepsItems[index];

            // Remove from list:
            _stepsItems.RemoveAt(index);

            // Destroy task item:
            taskItem.OnDeleted -= TaskItem_OnDeleted;
            taskItem.OnClicked -= TaskItem_OnClicked;
            taskItem.transform.SetParent(null);
            Destroy(taskItem);

            // Deallocate space of deleted element:
            Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _stepsItems.Count * _stepButtonHeight);
        }

        private void TaskItem_OnClicked(StepItem sender, EventArgs eventArgs)
        {
            if (OnTaskItemClicked != null)
                OnTaskItemClicked(sender, EventArgs.Empty);
        }
        private void TaskItem_OnDeleted(StepItem sender, EventArgs eventArgs)
        {
            TaskManager.RemoveTask(sender.Task.ID);
        }
    }
}
