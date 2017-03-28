using System;
using CollaborationEngine.Network;
using CollaborationEngine.Objects;
using CollaborationEngine.Scenes;
using UnityEngine;

namespace CollaborationEngine.States.Server
{
    public class IndicationButtonClickedState : IApplicationState
    {
        public IndicationButtonClickedState(ServerCollaborationState serverState, IndicationType indicationType)
        {
            if (indicationType == IndicationType.None)
                throw new Exception("Indication type must not be None.");

            _serverState = serverState;
            _indicationType = indicationType;
        }

        public void Initialize()
        {
            Debug.Log("Initialize IndicationButtonClickedState");

            _serverState.Scene.OnIndicationObjectAdded += Scene_OnIndicationObjectAdded;
        }
        public void Shutdown()
        {
            Debug.Log("Shutdown IndicationButtonClickedState");

            _serverState.Scene.OnIndicationObjectAdded -= Scene_OnIndicationObjectAdded;
        }

        public void FixedUpdate()
        {
        }
        public void FrameUpdate()
        {
            // If using UI:
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                return;

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var camera = ObjectLocator.Instance.MainCamera;
                var ray = camera.ScreenPointToRay(Input.mousePosition);

                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    var worldPosition = hitInfo.point;
                    var worldToLocalMatrix = _serverState.Scene.GameObject.transform.worldToLocalMatrix;
                    var localPosition = 0.1f * hitInfo.normal + worldToLocalMatrix.MultiplyPoint(worldPosition);

                    var sceneObjectData = new SceneObject.Data
                    {
                        Position = localPosition,
                        Rotation = Quaternion.FromToRotation(Vector3.forward, hitInfo.normal),
                        Scale = Vector3.one,
                        Type = SceneObjectType.Indication,
                        Flag = (uint) _indicationType
                    };
                    ClientController.Instance.AddSceneObjectData(sceneObjectData);
                }
                else
                {
                    // Change state:
                    _serverState.CurrentState = new NoneState(_serverState);
                }
            }
        }

        private void Scene_OnIndicationObjectAdded(Scene scene, Scene.SceneEventArgs<IndicationObject> eventArgs)
        {
            // Change state:
            _serverState.CurrentState = new IndicationSelectedState(_serverState, eventArgs.SceneObject);
        }

        private readonly ServerCollaborationState _serverState;
        private readonly IndicationType _indicationType;
    }
}
