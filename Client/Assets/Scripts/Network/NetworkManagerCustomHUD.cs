using System.ComponentModel;
using UnityEngine;
using UnityEngine.Networking;

#if ENABLE_UNET

namespace CollaborationEngine.Network
{
    [AddComponentMenu("Network/NetworkManagerCustomHUD")]
    [RequireComponent(typeof(NetworkManager))]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class NetworkManagerCustomHUD : MonoBehaviour
    {
        public NetworkManager Manager;
        [SerializeField] public bool ShowGUI = true;
        [SerializeField] public int OffsetX;
        [SerializeField] public int OffsetY;
        [SerializeField] public float SizeMultiplier = 1.0f;

        // Runtime variable
        bool _showServer;

        public void Awake()
        {
            Manager = GetComponent<NetworkManager>();
        }

        public void OnGUI()
        {
            if (!ShowGUI)
                return;

            int xpos = 10 + OffsetX;
            int ypos = 40 + OffsetY;
            int spacing = (int)(24.0f * SizeMultiplier);

            bool noConnection = (Manager.client?.connection == null || Manager.client.connection.connectionId == -1);

            if (!Manager.IsClientConnected() && !NetworkServer.active && Manager.matchMaker == null)
            {
                if (noConnection)
                {
                    if (Application.platform != RuntimePlatform.WebGLPlayer)
                    {
                        if (GUI.Button(new Rect(xpos, ypos, 200 * SizeMultiplier, 20 * SizeMultiplier), "LAN Host(H)"))
                        {
                            Manager.StartHost();
                        }
                        ypos += spacing;
                    }

                    if (GUI.Button(new Rect(xpos, ypos, 105 * SizeMultiplier, 20 * SizeMultiplier), "LAN Client(C)"))
                    {
                        Manager.StartClient();
                    }

                    Manager.networkAddress = GUI.TextField(new Rect(xpos + 100 * SizeMultiplier, ypos, 95 * SizeMultiplier, 20 * SizeMultiplier), Manager.networkAddress);
                    ypos += spacing;

                    if (Application.platform == RuntimePlatform.WebGLPlayer)
                    {
                        // cant be a server in webgl build
                        GUI.Box(new Rect(xpos, ypos, 200 * SizeMultiplier, 25 * SizeMultiplier), "(  WebGL cannot be server  )");
                        ypos += spacing;
                    }
                    else
                    {
                        if (GUI.Button(new Rect(xpos, ypos, 200 * SizeMultiplier, 20 * SizeMultiplier), "LAN Server Only(S)"))
                        {
                            Manager.StartServer();
                        }
                        ypos += spacing;
                    }
                }
                else
                {
                    GUI.Label(new Rect(xpos, ypos, 200 * SizeMultiplier, 20 * SizeMultiplier), "Connecting to " + Manager.networkAddress + ":" + Manager.networkPort + "..");
                    ypos += spacing;


                    if (GUI.Button(new Rect(xpos, ypos, 200 * SizeMultiplier, 20 * SizeMultiplier), "Cancel Connection Attempt"))
                    {
                        Manager.StopClient();
                    }
                }
            }
            else
            {
                if (NetworkServer.active)
                {
                    string serverMsg = "Server: port=" + Manager.networkPort;
                    if (Manager.useWebSockets)
                    {
                        serverMsg += " (Using WebSockets)";
                    }
                    GUI.Label(new Rect(xpos, ypos, 300 * SizeMultiplier, 20 * SizeMultiplier), serverMsg);
                    ypos += spacing;
                }
                if (Manager.IsClientConnected())
                {
                    GUI.Label(new Rect(xpos, ypos, 300 * SizeMultiplier, 20 * SizeMultiplier), "Client: address=" + Manager.networkAddress + " port=" + Manager.networkPort);
                    ypos += spacing;
                }
            }

            if (Manager.IsClientConnected() && !ClientScene.ready)
            {
                if (GUI.Button(new Rect(xpos, ypos, 200 * SizeMultiplier, 20 * SizeMultiplier), "Client Ready"))
                {
                    ClientScene.Ready(Manager.client.connection);

                    if (ClientScene.localPlayers.Count == 0)
                    {
                        ClientScene.AddPlayer(0);
                    }
                }
                ypos += spacing;
            }

            if (NetworkServer.active || Manager.IsClientConnected())
            {
                if (GUI.Button(new Rect(xpos, ypos, 200 * SizeMultiplier, 20 * SizeMultiplier), "Stop (X)"))
                {
                    Manager.StopHost();
                }
                ypos += spacing;
            }

            if (!NetworkServer.active && !Manager.IsClientConnected() && noConnection)
            {
                ypos += 10;

                if (Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    GUI.Box(new Rect(xpos - 5, ypos, 220 * SizeMultiplier, 25 * SizeMultiplier), "(WebGL cannot use Match Maker)");
                    return;
                }

                if (Manager.matchMaker == null)
                {
                    if (GUI.Button(new Rect(xpos, ypos, 200 * SizeMultiplier, 20 * SizeMultiplier), "Enable Match Maker (M)"))
                    {
                        Manager.StartMatchMaker();
                    }
                }
                else
                {
                    if (Manager.matchInfo == null)
                    {
                        if (Manager.matches == null)
                        {
                            if (GUI.Button(new Rect(xpos, ypos, 200 * SizeMultiplier, 20 * SizeMultiplier), "Create Internet Match"))
                            {
                                Manager.matchMaker.CreateMatch(Manager.matchName, Manager.matchSize, true, "", "", "", 0, 0, Manager.OnMatchCreate);
                            }
                            ypos += spacing;

                            GUI.Label(new Rect(xpos, ypos, 100 * SizeMultiplier, 20 * SizeMultiplier), "Room Name:");
                            Manager.matchName = GUI.TextField(new Rect(xpos + 100, ypos, 100 * SizeMultiplier, 20 * SizeMultiplier), Manager.matchName);
                            ypos += spacing;

                            ypos += 10;

                            if (GUI.Button(new Rect(xpos, ypos, 200 * SizeMultiplier, 20 * SizeMultiplier), "Find Internet Match"))
                            {
                                Manager.matchMaker.ListMatches(0, 20, "", false, 0, 0, Manager.OnMatchList);
                            }
                            ypos += spacing;
                        }
                        else
                        {
                            foreach (var match in Manager.matches)
                            {
                                if (GUI.Button(new Rect(xpos, ypos, 200 * SizeMultiplier, 20 * SizeMultiplier), "Join Match:" + match.name))
                                {
                                    Manager.matchName = match.name;
                                    Manager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, Manager.OnMatchJoined);
                                }
                                ypos += spacing;
                            }

                            if (GUI.Button(new Rect(xpos, ypos, 200 * SizeMultiplier, 20 * SizeMultiplier), "Back to Match Menu"))
                            {
                                Manager.matches = null;
                            }
                            ypos += spacing;
                        }
                    }

                    if (GUI.Button(new Rect(xpos, ypos, 200 * SizeMultiplier, 20 * SizeMultiplier), "Change MM server"))
                    {
                        _showServer = !_showServer;
                    }
                    if (_showServer)
                    {
                        ypos += spacing;
                        if (GUI.Button(new Rect(xpos, ypos, 100 * SizeMultiplier, 20 * SizeMultiplier), "Local"))
                        {
                            Manager.SetMatchHost("localhost", 1337, false);
                            _showServer = false;
                        }
                        ypos += spacing;
                        if (GUI.Button(new Rect(xpos, ypos, 100 * SizeMultiplier, 20 * SizeMultiplier), "Internet"))
                        {
                            Manager.SetMatchHost("mm.unet.unity3d.com", 443, true);
                            _showServer = false;
                        }
                        ypos += spacing;
                        if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Staging"))
                        {
                            Manager.SetMatchHost("staging-mm.unet.unity3d.com", 443, true);
                            _showServer = false;
                        }
                    }

                    ypos += spacing;

                    GUI.Label(new Rect(xpos, ypos, 300 * SizeMultiplier, 20 * SizeMultiplier), "MM Uri: " + Manager.matchMaker.baseUri);
                    ypos += spacing;

                    if (GUI.Button(new Rect(xpos, ypos, 200 * SizeMultiplier, 20 * SizeMultiplier), "Disable Match Maker"))
                    {
                        Manager.StopMatchMaker();
                    }
                }
            }
        }
    }
}
#endif //ENABLE_UNET