using CollaborationEngine.States;
using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.UI
{
    public class ConnectToServerButton : MonoBehaviour
    {
        public InputField IPAddressInputField;

        public void Start()
        {
            ApplicationInstance.Instance.SceneManager.OnSceneLoaded += SceneManager_OnSceneLoaded;
        }
        public void OnApplicationQuit()
        {
            ApplicationInstance.Instance.SceneManager.OnSceneLoaded -= SceneManager_OnSceneLoaded;
        }

        private void SceneManager_OnSceneLoaded(Scenes.SceneManager sender, System.EventArgs args)
        {
            var currentScene = sender.CurrentScene;

            if (currentScene.name == "Multiplayer")
                ApplicationInstance.Instance.ChangeState(new ClientCollaborationState(), false);
        }

        public void OnClick()
        {
            var networkManager = ApplicationInstance.Instance.NetworkManager;

            networkManager.networkAddress = IPAddressInputField.text;

            // Start client:
            networkManager.StartClient();
        }
    }
}
