using CollaborationEngine.Network;
using CollaborationEngine.Objects;
using CollaborationEngine.Scenes;
using UnityEngine;

namespace CollaborationEngine.States
{
    public class ServerCollaborationState : IApplicationState
    {
        public Scene Scene { get; private set; }

        public void Initialize()
        {
            ObjectLocator.Instance.ServerRoot.SetActive(true);
            ObjectLocator.Instance.ClientRoot.SetActive(false);

            Scene = new Scene(ObjectLocator.Instance.SceneRoot);
        }
        public void Shutdown()
        {
            Scene = null;
        }

        public void FixedUpdate()
        {
        }
        public void FrameUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var camera = ObjectLocator.Instance.MainCamera;
                var ray = camera.ScreenPointToRay(Input.mousePosition);

                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    var worldPosition = hitInfo.point;
                    var worldToLocalMatrix = Scene.GameObject.transform.worldToLocalMatrix;
                    var localPosition = worldToLocalMatrix.MultiplyPoint(worldPosition);

                    var sceneObjectData = new SceneObject.Data
                    {
                        Position = localPosition,
                        Rotation = Quaternion.identity,
                        Scale = Vector3.one,
                        Type = SceneObjectType.Indication
                    };
                    ClientController.Instance.AddSceneObjectData(sceneObjectData);
                }
            }

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
    }
}
