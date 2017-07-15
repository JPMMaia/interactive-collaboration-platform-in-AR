using System;
using System.IO;
using CollaborationEngine.Base;

namespace CollaborationEngine.Panels
{
    public class StartApprenticeController : Controller
    {
        public event EventHandler<StartApprenticeView.ConnectEventArgs> OnConnectToServer;

        public StartApprenticeView StartApprenticeViewPrefab;

        private StartApprenticeView _view;
        private String _stateFilename;

        public void Start()
        {
            // Instantiate view:
            _view = Instantiate(StartApprenticeViewPrefab, Application.View.MainCanvas.transform);

            // Subscribe to events:
            _view.OnConnectToServer += View_OnConnectToServer;

            //LoadState();
        }
        public void OnDestroy()
        {
            //SaveState();

            if(_view)
                Destroy(_view.gameObject);
        }

        private void LoadState()
        {
#if UNITY_ANDROID
            var directory = "/Android/data/pt.feup.coopar/files/";
#else
            var directory = String.Format("{0}/", UnityEngine.Application.dataPath);
#endif

            // Set filename:
            _stateFilename = directory + "State.dat";

            if (File.Exists(_stateFilename))
            {
                using (var stream = File.OpenRead(_stateFilename))
                {
                    using (var streamReader = new StreamReader(stream))
                    {
                        _view.IPAddressInputField.text = streamReader.ReadLine();
                    }
                }
            }
        }
        private void SaveState()
        {
            using (var stream = File.OpenWrite(_stateFilename))
            {
                using (var streamWriter = new StreamWriter(stream))
                {
                    streamWriter.WriteLine(_view.IPAddressInputField.text);
                }
            }
        }

        private void View_OnConnectToServer(object sender, StartApprenticeView.ConnectEventArgs e)
        {
            if (OnConnectToServer != null)
                OnConnectToServer(this, e);
        }
    }
}
