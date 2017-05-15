﻿using CollaborationEngine.Base;
using CollaborationEngine.Network;
using CollaborationEngine.Panels;
using UnityEngine;
using UnityEngine.Networking;

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
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            // Subsribe to network events:
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
        private void PresentARScreen(NetworkMessage networkMessage)
        {
            var controller = Instantiate(ARApprenticeControllerPrefab, transform);
            controller.OnPresentStep(networkMessage);
            
            _curentController = controller;
        }

        private void Controller_OnConnectToServer(object sender, StartApprenticeView.ConnectEventArgs e)
        {
            NetworkManager.networkAddress = e.IPAddress;
            NetworkManager.StartClient();
            NetworkManager.client.RegisterHandler(NetworkHandles.PresentStep, OnPresentStep);
        }
        private void NetworkManager_OnDisconnected(object sender, System.EventArgs e)
        {
            if (_curentController)
                Destroy(_curentController.gameObject);

            PresentStartScreen();
        }

        private void OnPresentStep(NetworkMessage networkMessage)
        {
            if (_curentController)
                Destroy(_curentController.gameObject);

            PresentARScreen(networkMessage);
        }
    }
}
