using CollaborationEngine.Objects;
using CollaborationEngine.UI;
using UnityEngine;

namespace CollaborationEngine.States.Server
{
    public class IndicationSelectedState : IApplicationState
    {
        public IndicationSelectedState(ServerCollaborationState serverState, IndicationObject indicationObject)
        {
            _serverState = serverState;
            _indicationToolsObject = new IndicationToolsObject(indicationObject);
        }

        public void Initialize()
        {
            Debug.Log("Initialize IndicationSelectedState");

            _indicationToolsObject.Instantiate(_indicationToolsObject.IndicationObject.GameObject.transform);

            _toolsComponent = _indicationToolsObject.GetComponent<ToolsWindow>();
            _toolsComponent.CloseButton.OnPressedEvent += CloseButton_OnPressedEvent;
            _toolsComponent.DeleteButton.OnPressedEvent += DeleteButton_OnPressedEvent;
        }
        public void Shutdown()
        {
            Debug.Log("Shutdown IndicationSelectedState");

            if (_toolsComponent)
            {
                _toolsComponent.DeleteButton.OnPressedEvent -= DeleteButton_OnPressedEvent;
                _toolsComponent.CloseButton.OnMouseDownEvent -= CloseButton_OnPressedEvent;
                _toolsComponent = null;
            }

            if (_indicationToolsObject != null)
            {
                _indicationToolsObject.Destroy();
                _indicationToolsObject = null;
            }
        }

        public void FixedUpdate()
        {
            _indicationToolsObject.FixedUpdate();
        }
        public void FrameUpdate()
        {
            _indicationToolsObject.FrameUpdate();
        }

        private void CloseButton_OnPressedEvent(object sender, System.EventArgs args)
        {
            _serverState.CurrentState = new NoneState(_serverState);
        }
        private void DeleteButton_OnPressedEvent(object sender, System.EventArgs args)
        {
            _serverState.CurrentState = new NoneState(_serverState);
        }

        private readonly ServerCollaborationState _serverState;
        private IndicationToolsObject _indicationToolsObject;
        private ToolsWindow _toolsComponent;
    }
}
