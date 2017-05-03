using System;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
using UnityScene = UnityEngine.SceneManagement.Scene;

namespace CollaborationEngine.Scenes
{
    public class SceneManager
    {
        public delegate void OnSceneLoadedDelegate(SceneManager sender, EventArgs args);
        public event OnSceneLoadedDelegate OnSceneLoaded;

        public UnityScene CurrentScene { get; private set; }

        public SceneManager()
        {
            UnitySceneManager.sceneLoaded += UnitySceneManager_sceneLoaded;
        }
        ~SceneManager()
        {
            UnitySceneManager.sceneLoaded -= UnitySceneManager_sceneLoaded;
        }

        private void UnitySceneManager_sceneLoaded(UnityScene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            CurrentScene = scene;

            if (OnSceneLoaded != null)
                OnSceneLoaded(this, EventArgs.Empty);
        }
    }
}
