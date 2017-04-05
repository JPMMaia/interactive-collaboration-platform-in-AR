using System;
using CollaborationEngine.Network;
using CollaborationEngine.Objects;
using CollaborationEngine.Objects.Components;
using UnityEngine;

namespace CollaborationEngine.States.Server
{
    public class NoneState : IApplicationState
    {
        public NoneState(ServerCollaborationState serverState)
        {
            _serverState = serverState;
        }

        public void Initialize()
        {
            Debug.Log("Initialize NoneState");

            _serverState.OnIndicationObjectClicked += ServerState_OnIndicationObjectClicked;
        }
        public void Shutdown()
        {
            Debug.Log("Shutdown NoneState");

            _serverState.OnIndicationObjectClicked -= ServerState_OnIndicationObjectClicked;
        }

        public void FixedUpdate()
        {
        }
        public void FrameUpdate()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                var sceneObjectData = new SceneObject.Data
                {
                    Position = Vector3.zero,
                    Rotation = Quaternion.identity,
                    Scale = Vector3.one,
                    Type = SceneObjectType.Real
                };

                ClientController.Instance.AddSceneObjectData(sceneObjectData);
            }
        }

        private void ServerState_OnIndicationObjectClicked(InputColliderComponent<IndicationObject> sender, EventArgs eventArgs)
        {
            _serverState.CurrentState = new IndicationSelectedState(_serverState, sender.GameObject);
        }

        private readonly ServerCollaborationState _serverState;
    }
}
