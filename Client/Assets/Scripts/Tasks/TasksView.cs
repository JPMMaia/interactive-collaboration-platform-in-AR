using System;
using System.Collections.Generic;
using CollaborationEngine.Base;
using UnityEngine;

namespace CollaborationEngine.Tasks
{
    public class TasksView : Entity
    {
        public RectTransform Container;

        #region Events
        public event EventHandler OnCreateTaskClicked;
        #endregion

        #region Unity Event Handlers
        public void OnCreateTaskButtonClicked()
        {
            if(OnCreateTaskClicked != null)
                OnCreateTaskClicked(this, EventArgs.Empty);
        }
        #endregion
    }
}
