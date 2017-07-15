using System;
using CollaborationEngine.Base;
using CollaborationEngine.Events;
using CollaborationEngine.Tasks;

namespace CollaborationEngine.Panels
{
    public class StartMentorView : Entity
    {
        public event EventHandler<IDEventArgs> OnTaskSelected;

        public TasksView TasksView;

        public void Start()
        {
            TasksView.OnTaskSelected += TasksView_OnTaskSelected;
        }

        private void TasksView_OnTaskSelected(object sender, IDEventArgs e)
        {
            if (OnTaskSelected != null)
                OnTaskSelected(this, e);
        }
    }
}
