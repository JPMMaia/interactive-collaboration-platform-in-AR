using System;
using CollaborationEngine.Base;
using CollaborationEngine.Events;
using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.Tasks
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TasksView : Entity
    {
        #region Unity Editor
        public RectTransform Container;
        public Text Placeholder;
        #endregion

        #region Events
        public event EventHandler<IDEventArgs> OnTaskSelected;
        public event EventHandler OnCreateTaskClicked;
        #endregion

        public bool Interactale
        {
            get { return GetComponent<CanvasGroup>().interactable; }
            set { GetComponent<CanvasGroup>().interactable = value; }
        }

        public void AddToContainer(Transform element)
        {
            element.SetParent(Container, false);
            Placeholder.gameObject.SetActive(false);
        }
        public void RemoveFromContainer(Transform element)
        {
            element.SetParent(null);

            if(Container.transform.childCount == 0)
                Placeholder.gameObject.SetActive(true);
        }

        public void NotifyTaskSelected(uint taskID)
        {
            if (OnTaskSelected != null)
                OnTaskSelected(this, new IDEventArgs(taskID));
        }

        #region Unity Event Handlers
        public void OnCreateTaskButtonClicked()
        {
            if (OnCreateTaskClicked != null)
                OnCreateTaskClicked(this, EventArgs.Empty);
        }
        #endregion
    }
}
