using CollaborationEngine.States;
using UnityEngine;

namespace CollaborationEngine.UI
{
    public class StartServerButton : MonoBehaviour
    {
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
                ApplicationInstance.Instance.ChangeState(new ServerCollaborationState(), false);
        }

        public void OnClick()
        {
            // Start a server and a client on the same application:
            ApplicationInstance.Instance.NetworkManager.StartHost();
        }
    }
}
