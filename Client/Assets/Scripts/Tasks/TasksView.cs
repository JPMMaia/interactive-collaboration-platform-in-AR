using System;
using CollaborationEngine.Base;
using UnityEngine;

namespace CollaborationEngine.Tasks
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TasksView : Entity
    {
        public RectTransform Container;

        #region Events
        public event EventHandler OnCreateTaskClicked;
        #endregion

        public bool Interactale
        {
            get { return GetComponent<CanvasGroup>().interactable; }
            set { GetComponent<CanvasGroup>().interactable = value; }
        }

        #region Unity Event Handlers
        public void OnCreateTaskButtonClicked()
        {
            if(OnCreateTaskClicked != null)
                OnCreateTaskClicked(this, EventArgs.Empty);
        }
        #endregion
    }
}
