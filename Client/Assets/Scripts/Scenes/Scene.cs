using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Scenes
{
    public class Scene : NetworkBehaviour
    {
        public GameObject Root;

        [Command]
        public void CmdAdd(GameObject prefab, Vector3 position, Quaternion rotation, int layer, bool worldPositionStays)
        {
            // Instantiate prefab:
            var instance = Instantiate(prefab);
            //PrepareInstance(instance, position, rotation, layer, worldPositionStays);
            
            // Spawn the scene on the client:
            NetworkServer.Spawn(instance);

            var sceneObject = instance.GetComponent<SceneObject>();
            sceneObject.RpcSetTransform(position, rotation, Root, worldPositionStays);
            sceneObject.RpcSetLayer(layer);

            //RpcPrepareInstance(instance, Vector3.zero, Quaternion.identity, layer, worldPositionStays);
        }

        [ClientRpc]
        public void RpcPrepareInstance(GameObject instance, Vector3 position, Quaternion rotation, int layer, bool worldPositionStays)
        {
            PrepareInstance(instance, position, rotation, layer, worldPositionStays);
        }

        private void PrepareInstance(GameObject instance, Vector3 position, Quaternion rotation, int layer, bool worldPositionStays)
        {
            instance.transform.SetParent(Root.transform, worldPositionStays);
            /*if (!worldPositionStays)
            {
                instance.transform.localPosition = position;
                instance.transform.localRotation = rotation;
            }
            else
            {
                instance.transform.position = position;
                instance.transform.rotation = rotation;
            }*/

            instance.layer = layer;
            foreach (Transform transform in instance.GetComponentsInChildren<Transform>(true))
                transform.gameObject.layer = layer;
        }
    }
}
