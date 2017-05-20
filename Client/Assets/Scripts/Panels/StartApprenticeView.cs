using System;
using CollaborationEngine.Base;
using UnityEngine.UI;

namespace CollaborationEngine.Panels
{
    public class StartApprenticeView : Entity
    {
        public class ConnectEventArgs : EventArgs
        {
            public String IPAddress { get; private set; }

            public ConnectEventArgs(String ipAddress)
            {
                IPAddress = ipAddress;
            }
        }

        public event EventHandler<ConnectEventArgs> OnConnectToServer;

        public InputField IPAddressInputField;

        public void Start()
        {
            IPAddressInputField.ActivateInputField();
        }

        public void OnOKClick()
        {
            if (IPAddressInputField.text.Length == 0)
            {
                IPAddressInputField.ActivateInputField();
                return;
            }

            if (OnConnectToServer != null)
                OnConnectToServer(this, new ConnectEventArgs(IPAddressInputField.text));
        }
    }
}