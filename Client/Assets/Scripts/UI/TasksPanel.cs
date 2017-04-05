using CollaborationEngine.States;
using UnityEngine;

namespace CollaborationEngine.UI
{
    public class TasksPanel : MonoBehaviour
    {
        public GameObject TaskButtonPrefab;
        public RectTransform Content;

        public void Awake()
        {
            var currentState = ApplicationInstance.Instance.CurrentState;
            if (currentState is ServerCollaborationState)
            {
                _serverState = currentState as ServerCollaborationState;
                _serverState.TaskManager.OnTaskAdded += TaskManager_OnTaskAdded;
            }

            var taskButtonTransform = TaskButtonPrefab.GetComponent<RectTransform>();
            _taskButtonHeight = taskButtonTransform.rect.size.y;
        }
        public void OnApplicationQuit()
        {
            if (_serverState != null)
            {
                _serverState.TaskManager.OnTaskAdded -= TaskManager_OnTaskAdded;
            }
        }

        private ServerCollaborationState _serverState;
        private uint _tasksCount;
        private float _taskButtonHeight;

        private void TaskManager_OnTaskAdded(Tasks.TaskManager sender, Tasks.TaskManager.TaskEventArgs eventArgs)
        {
            var position = new Vector3(0.0f, _tasksCount * _taskButtonHeight);
            var taskButton = Instantiate(TaskButtonPrefab, position, Quaternion.identity);
            taskButton.transform.SetParent(Content.transform, false);

            ++_tasksCount;
        }
    }
}
