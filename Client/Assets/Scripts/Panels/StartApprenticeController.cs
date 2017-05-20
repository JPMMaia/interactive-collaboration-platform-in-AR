using System;
using CollaborationEngine.Base;

namespace CollaborationEngine.Panels
{
    public class StartApprenticeController : Controller
    {
        public event EventHandler<StartApprenticeView.ConnectEventArgs> OnConnectToServer;

        public StartApprenticeView StartApprenticeViewPrefab;

        private StartApprenticeView _view;

        public void Start()
        {
            // Instantiate view:
            _view = Instantiate(StartApprenticeViewPrefab, Application.View.MainCanvas.transform);

            // Subscribe to events:
            _view.OnConnectToServer += View_OnConnectToServer;
        }
        public void OnDestroy()
        {
            if(_view)
                Destroy(_view.gameObject);
        }

        private void View_OnConnectToServer(object sender, StartApprenticeView.ConnectEventArgs e)
        {
            if (OnConnectToServer != null)
                OnConnectToServer(this, e);
        }
    }
}
