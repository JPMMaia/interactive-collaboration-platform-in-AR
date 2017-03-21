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
            Scene = new Scene2(ObjectLocator.Instance.SceneRoot);
        }
        public void Shutdown()
        {
            Scene = null;

            ApplicationInstance.Instance.NetworkManager.StopHost();
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
                    //var worldToLocalMatrix = Scene.transform.worldToLocalMatrix;
                    //var localPosition = worldToLocalMatrix.MultiplyPoint(worldPosition);

                    //Scene.CmdAdd(ArrowPrefab, localPosition, Quaternion.identity, 1, false);
                }
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                //Scene.CmdAdd(RubiksPrefab, Vector3.zero, Quaternion.identity, 8, false);
            }
        }
    }
}
