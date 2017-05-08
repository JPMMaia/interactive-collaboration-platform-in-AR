using System;
using CollaborationEngine.Base;
using CollaborationEngine.Events;
using CollaborationEngine.UserInterface;
using UnityEngine;

namespace CollaborationEngine.Tasks
{
    public class TaskView : Entity
    {
        #region Events
        public delegate void IDEventDelegate(TaskView sender, IDEventArgs eventArgs);

        public event IDEventDelegate OnSelected;
        public event IDEventDelegate OnEdited;
        public event IDEventDelegate OnDuplicated;
        public event IDEventDelegate OnDeleted;
        public event IDEventDelegate OnEndEdit;
        #endregion

        #region UnityEditor
        public ButtonInputFieldToggle TaskButtonInputFieldToggle;
        #endregion

        #region Properties
        public uint TaskID { get; set; }
        public uint TaskOrder
        {
            get { return _taskOrder; }
            set
            {
                _taskOrder = value;
                UpdateView();
            }
        }
        public String TaskName
        {
            get
            {
                return _taskName;
            }
            set
            {
                _taskName = value;
                UpdateView();
            }
        }
        #endregion

        #region Members
        private uint _taskOrder;
        private String _taskName;
        #endregion

        public void Start()
        {
            TaskButtonInputFieldToggle.OnEndEdit += TaskButtonInputFieldToggle_OnEndEdit;
        }
        public void OnDestroy()
        {
            TaskButtonInputFieldToggle.OnEndEdit -= TaskButtonInputFieldToggle_OnEndEdit;
        }

        private void UpdateView()
        {
            TaskButtonInputFieldToggle.OrderText = String.Format("{0,2:D2}.", _taskOrder);
            TaskButtonInputFieldToggle.Text = _taskName;
        }

        public void EditTaskName()
        {
            TaskButtonInputFieldToggle.GetComponent<CanvasGroup>().ignoreParentGroups = true;
            TaskButtonInputFieldToggle.ActivateInputField();
        }

        private void TaskButtonInputFieldToggle_OnEndEdit(object sender, EventArgs e)
        {
            _taskName = TaskButtonInputFieldToggle.Text;
            TaskButtonInputFieldToggle.GetComponent<CanvasGroup>().ignoreParentGroups = false;

            if (OnEndEdit != null)
                OnEndEdit(this, new IDEventArgs(TaskID));
        }

        #region Unity UI Events
        public void OnClick()
        {
            if (OnSelected != null)
                OnSelected(this, new IDEventArgs(TaskID));
        }
        public void OnEditClick()
        {
            if (OnEdited != null)
                OnEdited(this, new IDEventArgs(TaskID));
        }
        public void OnDuplicateClick()
        {
            if (OnDuplicated != null)
                OnDuplicated(this, new IDEventArgs(TaskID));
        }
        public void OnDeleteClick()
        {
            if (OnDeleted != null)
                OnDeleted(this, new IDEventArgs(TaskID));
        }
        #endregion
    }
}
