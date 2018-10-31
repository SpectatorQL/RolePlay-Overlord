using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace RolePlayOverlord
{
    public class Network : NetworkBehaviour
    {
        List<ClientEntity> _clients;
        ClientEntity _host;

        NetworkLobbyManager _networkManager;

        string _dataPath;
        string _modPath;
        // TODO: This is static for now. Change it once we implement
        // dynamic asset loading?
        static Dictionary<string, Texture2D> _textureCache = new Dictionary<string, Texture2D>();
        Wall[] _walls;

        [SerializeField] private GameObject _hostUI;
        [SerializeField] private GameObject _playerUI;

        string GetAssetFilePath(string file)
        {
            string result = _dataPath + _modPath + file;
            return result;
        }

        // TODO: Turn this into something more reasonable.
        IEnumerator LoadTex(Texture2D tex, string url)
        {
            using(WWW www = new WWW(url))
            {
                yield return www;
                www.LoadImageIntoTexture(tex);
            }
        }

        [ClientRpc]
        public void RpcSetTexture(string texName)
        {
            int textureWidth = 1024;
            int textureHeight = 1024;
            string texUrl = GetAssetFilePath(texName);

            Texture2D tex;
            if(_textureCache.ContainsKey(texUrl))
            {
                tex = _textureCache[texUrl];
            }
            else
            {
                tex = new Texture2D(textureWidth, textureHeight);
                StartCoroutine(LoadTex(tex, texUrl));
                _textureCache.Add(texUrl, tex);
            }
            
            for(int i = 0; i < _walls.Length; ++i)
            {
                _walls[i].ChangeWallTexture(tex);
            }
        }

        void Awake()
        {
#if UNITY_EDITOR
            _dataPath = "file:///Assets/";
#else
            _dataPath = "file:///Test/";
#endif
            Debug.LogError("==== " + _dataPath + " ====");
        }

        [ServerCallback]
        void ServerStartup()
        {
            _networkManager = FindObjectOfType<NetworkLobbyManager>();

            var connections = NetworkServer.connections;
            if(connections != null)
            {
                _clients = new List<ClientEntity>(_networkManager.maxPlayers);
                for(int i = 0; i < connections.Count; ++i)
                {
                    GameObject go = connections[i].playerControllers[0].gameObject;
                    ClientEntity ent = go.GetComponent<ClientEntity>();
                    if(ent != null)
                    {
                        if(ent.isServer)
                        {
                            _host = ent;
                            ent.ProcessKeyboardInput = PlayerInput.ProcessHostKeyboard;
                            ent.RotateCamera = PlayerInput.ProcessHostMouse;

                            ent.UI = _hostUI;
                            ent.HostUIController = _hostUI.GetComponent<HostUIController>();
                            ent.HostUIController.Setup();
                        }
                        else
                        {
                            _clients.Add(ent);
                        }

                        ent.Network = this;
                    }
                }
            }
        }

        void Start()
        {
            ServerStartup();

            _walls = FindObjectsOfType<Wall>();
        }
    }
}