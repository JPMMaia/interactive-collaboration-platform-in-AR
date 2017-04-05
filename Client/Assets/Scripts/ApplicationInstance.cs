using CollaborationEngine.Historic;
using CollaborationEngine.Scenes;
using CollaborationEngine.States;
using CollaborationEngine.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace CollaborationEngine
{
    public class ApplicationInstance : MonoBehaviour
    {
        private static ApplicationInstance _instance;
        public static ApplicationInstance Instance
        {
            get
            {
                if (!_instance)
                    _instance = FindObjectOfType(typeof(ApplicationInstance)) as ApplicationInstance;

                return _instance;
            }
        }

        private readonly History _history = new History();
        public History History
        {
            get
            {
                return _history;
            }
        }

        public IApplicationState CurrentState { get; private set; }

        private NetworkManager _networkManager;
        public NetworkManager NetworkManager
        {
            get
            {
                if (_networkManager == null)
                    _networkManager = FindObjectOfType<NetworkManager>();

                return _networkManager;
            }
        }

        private readonly SceneManager _sceneManager = new SceneManager();
        public SceneManager SceneManager
        {
            get { return _sceneManager; }
        }

        public void Awake()
        {
            DontDestroyOnLoad(this);

            // Initial state:
            ChangeState(new StartState(), false);
        }

        public void OnApplicationQuit()
        {
            if(CurrentState != null)
                CurrentState.Shutdown();
        }

        public void FixedUpdate()
        {
            if(CurrentState != null)
                CurrentState.FixedUpdate();
        }

        public void Update()
        {
            if (CurrentState != null)
                CurrentState.FrameUpdate();

            if (Input.GetKeyDown(KeyCode.P))
                ChangeToPreviousState();
            
            if (Input.GetKeyDown("escape"))
                Application.Quit();
        }

        public void ChangeState<T>(T state, bool addToHistory = true) where T : IApplicationState
        {
            // Add action to history if flag is set:
            if (addToHistory)
                History.PushAction(CurrentState, state);

            // Shutdown previous state:
            if (CurrentState != null)
                CurrentState.Shutdown();

            // Set current state:
            CurrentState = state;

            // Initialize current state:
            CurrentState.Initialize();
        }

        public void ChangeToPreviousState()
        {
            // If history is empty, ignore:
            if (History.IsEmpty())
                return;

            // Change to the previous state:
            var action = History.PopAction();
            ChangeState(action.InitialState, false);
        }

        private ApplicationInstance()
        {
        }
    }
}
