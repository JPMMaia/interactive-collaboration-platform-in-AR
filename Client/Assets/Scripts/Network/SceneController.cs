using UnityEngine;
using UnityEngine.Networking;

public class SceneController : NetworkBehaviour
{
    public GameObject SceneToSpan;

    private bool _sceneSpawn = false;

    [Command]
    void CmdCreateScene()
    {
        if (_sceneSpawn)
            return;

        // Create the scene object locally:
        var scene = Instantiate(SceneToSpan, this.transform);

        // Spawn the scene on the client:
        NetworkServer.Spawn(scene);

        Debug.Log("Spawn!");
        _sceneSpawn = true;
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
