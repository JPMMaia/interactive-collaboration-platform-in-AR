using CollaborationEngine.Historic;
using CollaborationEngine.States;
using UnityEngine;

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

        public History History { get; set; }
        public IApplicationState CurrentState { get; private set; }

        public void Start()
        {
            History = new History();

            // Initial state:
            ChangeState(new StartState(), false);
        }

        public void FixedUpdate()
        {
            CurrentState?.FixedUpdate();
        }

        public void Update()
        {
            CurrentState?.FrameUpdate();

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
            CurrentState?.Shutdown();

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
