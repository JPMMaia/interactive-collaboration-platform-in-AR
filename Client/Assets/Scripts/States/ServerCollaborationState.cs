using CollaborationEngine.Objects;
using CollaborationEngine.Scenes;
using UnityEngine;

namespace CollaborationEngine.States
{
    public class ServerCollaborationState : IApplicationState
    {
        public Scene2 Scene { get; private set; }

        public void Initialize()
        {
            ObjectLocator.Instance.ServerRoot.SetActive(true);
            ObjectLocator.Instance.ClientRoot.SetActive(false);

            Scene = new Scene2(ObjectLocator.Instance.SceneRoot);
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
            var applicationInstance = ApplicationInstance.Instance;

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

                    var sceneObjectData = new SceneObject2.Data
                    {
                        Position = localPosition,
                        Rotation = Quaternion.identity,
                        Scale = Vector3.one,
                        Type = SceneObjectType.Real
                    };
                    applicationInstance.NetworkController.CmdAddSceneObject(sceneObjectData);
                }
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                //Scene.CmdAdd(RubiksPrefab, Vector3.zero, Quaternion.identity, 8, false);
            }
        }
    }
}
