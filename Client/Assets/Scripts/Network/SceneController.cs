using UnityEngine;
using UnityEngine.Networking;

public class SceneController : NetworkBehaviour
{
    public GameObject SceneRoot;
    public GameObject SceneToSpan;

    private bool _sceneSpawn = false;

    [Command]
    void CmdCreateScene()
    {
        if (_sceneSpawn)
            return;

        // Create the scene object locally:
        var scene = Instantiate(SceneToSpan);
        scene.transform.SetParent(SceneRoot.transform, false);

        // Spawn the scene on the client:
        NetworkServer.Spawn(scene);

        RpcSyncScene(Vector3.zero, Quaternion.identity, scene, SceneRoot);

        _sceneSpawn = true;
    }

    [ClientRpc]
    public void RpcSyncScene(Vector3 position, Quaternion rotation, GameObject scene, GameObject sceneRoot)
    {
        scene.transform.position = position;
        scene.transform.rotation = rotation;
        scene.transform.SetParent(SceneRoot.transform, false);
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isLocalPlayer)
        //    return;

        if (Input.GetKeyDown(KeyCode.T))
        {
            CmdCreateScene();
        }
    }
}
