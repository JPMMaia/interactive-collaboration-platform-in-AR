using System;
using CollaborationEngine.Objects;
using CollaborationEngine.Objects.Components;
using CollaborationEngine.Scenes;
using CollaborationEngine.States.Server;

namespace CollaborationEngine.States
{
    public class ServerCollaborationState : IApplicationState
    {
        public event InputColliderComponent<IndicationObject>.InputEvent OnIndicationObjectClicked;

        public void Initialize()
        {
            _currentState = new NoneState(this);
            _currentState.Initialize();

            ObjectLocator.Instance.ServerRoot.SetActive(true);
            ObjectLocator.Instance.ClientRoot.SetActive(false);

            Scene = new Scene(ObjectLocator.Instance.SceneRoot);
            Scene.OnIndicationObjectAdded += Scene_OnIndicationObjectAdded;
        }
        public void Shutdown()
        {
            Scene = null;

            if (_currentState != null)
            {
                _currentState.Shutdown();
                _currentState = null;
            }
        }

        public void FixedUpdate()
        {
            _currentState.FixedUpdate();
        }
        public void FrameUpdate()
        {
            _currentState.FrameUpdate();
        }

        public Scene Scene { get; private set; }
        public IndicationType SelectedIndicationType
        {
            get
            {
                return _selectedIndicationType;
            }
            set { _selectedIndicationType = value; }
        }

        public IApplicationState CurrentState
        {
            get
            {
                return _currentState;
            }
            set
            {
                _currentState.Shutdown();
                _currentState = value;
                _currentState.Initialize();
            }
        }

        private void Scene_OnIndicationObjectAdded(Scene scene, Scene.SceneEventArgs<IndicationObject> eventArgs)
        {
            var sceneObject = eventArgs.SceneObject;

            var inputCollider = new InputColliderComponent<IndicationObject>(sceneObject);
            inputCollider.OnPressed += InputCollider_OnPressed;
        }
        private void InputCollider_OnPressed(InputColliderComponent<IndicationObject> sender, EventArgs eventArgs)
        {
            if (OnIndicationObjectClicked != null)
                OnIndicationObjectClicked(sender, eventArgs);
        }

        private IApplicationState _currentState;
        private IndicationType _selectedIndicationType = IndicationType.None;
    }
}
