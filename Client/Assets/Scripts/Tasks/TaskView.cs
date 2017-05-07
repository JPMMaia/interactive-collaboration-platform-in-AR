using System;
using CollaborationEngine.Base;
using CollaborationEngine.Events;
using UnityEngine.UI;

namespace CollaborationEngine.Tasks
{
    public class TaskView : Entity
    {
        #region Events
        public event IDEventDelegate OnSelected;
        public event IDEventDelegate OnEdited;
        public event IDEventDelegate OnDuplicated;
        public event IDEventDelegate OnDeleted;
        #endregion

        #region UnityEditor
        public Text TaskNameText;
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

        private void UpdateView()
        {
            TaskNameText.text = String.Format("{0,2:D2}. {1}", _taskOrder, _taskName.ToUpper());
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
