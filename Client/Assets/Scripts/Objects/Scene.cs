using UnityEngine;
using UnityEngine.Networking;

namespace CollaborationEngine.Objects
{
    public class Scene : NetworkBehaviour
    {
        public GameObject Root;

        [Command]
        public void CmdAdd(GameObject prefab, Vector3 position, Quaternion rotation, int layer, bool worldPositionStays)
        {
            // Instantiate prefab:
            var instance = Instantiate(prefab);
            
            // Spawn the scene on the client:
            NetworkServer.Spawn(instance);

            var sceneObject = instance.GetComponent<SceneObject>();
            sceneObject.RpcSetTransform(position, rotation, worldPositionStays);
            sceneObject.RpcSetLayer(layer);
        }
    }
}
