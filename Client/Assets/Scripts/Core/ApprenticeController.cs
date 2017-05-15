using CollaborationEngine.Base;
using CollaborationEngine.Network;
using CollaborationEngine.Panels;

namespace CollaborationEngine.Core
{
    public class ApprenticeController : Controller
    {
        #region Unity Editor
        public StartApprenticeController StartApprenticeControllerPrefab;
        public ARApprenticeController ARApprenticeControllerPrefab;
        public ApprenticeNetworkManager NetworkManager;
        #endregion

        private Entity _curentController;

        public void Start()
        {
            // Subsribe to network events:
            NetworkManager.OnConnected += NetworkManager_OnConnected;
            NetworkManager.OnDisconnected += NetworkManager_OnDisconnected;

            // Present start screen:
            PresentStartScreen();
        }

        private void PresentStartScreen()
        {
            var controller = Instantiate(StartApprenticeControllerPrefab, transform);
            controller.OnConnectToServer += Controller_OnConnectToServer;

            _curentController = controller;
        }
        private void PresentARScreen()
        {
            var controller = Instantiate(ARApprenticeControllerPrefab, transform);

            _curentController = controller;
        }

        private void Controller_OnConnectToServer(object sender, StartApprenticeView.ConnectEventArgs e)
        {
            NetworkManager.networkAddress = e.IPAddress;
            NetworkManager.StartClient();
        }
        private void NetworkManager_OnConnected(object sender, System.EventArgs e)
        {
            if (_curentController)
                Destroy(_curentController.gameObject);

           PresentARScreen();
        }
        private void NetworkManager_OnDisconnected(object sender, System.EventArgs e)
        {
            if (_curentController)
                Destroy(_curentController.gameObject);

            PresentStartScreen();
        }
    }
}
