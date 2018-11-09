using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace RolePlayOverlord
{
    public class Network : NetworkBehaviour
    {
        NetworkConnection[] _clientConnections;
        ClientEntity[] _clients;
        // TODO: Does anything need to talk to the host directly?
        ClientEntity _host;

        string _dataPath;
        string _modPath;
        Dictionary<string, Texture2D> _textureCache = new Dictionary<string, Texture2D>();
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
        }

        void ClientStartup(ClientEntity ent)
        {
            if(!ent.isLocalPlayer)
                return;

            if(ent.isServer)
            {
                ent.ProcessKeyboardInput = PlayerInput.ProcessHostKeyboard;
                ent.ProcessMouseInput = PlayerInput.ProcessHostMouse;

                ent.UI = _hostUI;
                ent.HostUIController = _hostUI.GetComponent<HostUIController>();
                ent.HostUIController.Setup();

                _host = ent;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;

                ent.UI = _playerUI;
                ent.PlayerUIController = _playerUI.GetComponent<PlayerUIController>();
                ent.PlayerUIController.Setup();
            }

            ent.Network = this;
        }
        
        void ServerStartup()
        {
            var ents = FindObjectsOfType<ClientEntity>();
            if(ents != null)
            {
                _clients = new ClientEntity[ents.Length];
                _clientConnections = new NetworkConnection[ents.Length];
                for(int i = 0; i < ents.Length; ++i)
                {
                    ClientEntity ent = ents[i];
                    if(ent != null)
                    {
                        ClientStartup(ent);
                        if(isServer)
                        {
                            _clients[i] = ent;
                            _clientConnections[i] = ent.connectionToServer;
                        }
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