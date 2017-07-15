using CollaborationEngine.Base;
using CollaborationEngine.Events;

namespace CollaborationEngine.Panels
{
    public class StartMentorController : Controller
    {
        public delegate void EventDelegate(StartMentorController sender, IDEventArgs eventArgs);

        public event EventDelegate OnTaskSelected;

        public StartMentorView StartMentorViewPrefab;

        private StartMentorView _startMentorView;

        public void Start()
        {
            // CreateStep start mentor view:
            _startMentorView = Instantiate(StartMentorViewPrefab);

            // Add to canvas:
            _startMentorView.transform.SetParent(Application.View.MainCanvas.transform, false);

            // Subscribe to events:
            _startMentorView.OnTaskSelected += StartMentorView_OnTaskSelected;
        }
        public void OnDestroy()
        {
            if(_startMentorView)
                Destroy(_startMentorView.gameObject);
        }

        private void StartMentorView_OnTaskSelected(object sender, IDEventArgs e)
        {
            if (OnTaskSelected != null)
                OnTaskSelected(this, e);
        }
    }
}
