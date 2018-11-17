using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using RolePlayOverlord.UI;

namespace RolePlayOverlord
{
    public class Network : NetworkBehaviour
    {
        NetworkConnection[] _clientConnections;
        ClientEntity[] _clients;
        // TODO: Does anything need to talk to the host directly?
        ClientEntity _host;

        string[] _characterStats;
        public List<string> Docs = new List<string>();
        Dictionary<string, string> _documents;

        string _dataPath;
        string _modPath;
        Dictionary<string, Texture2D> _textureCache = new Dictionary<string, Texture2D>();
        Wall[] _walls;

        [SerializeField] private GameObject _hostUI;
        [SerializeField] private GameObject _playerUI;

        public string GetClientCharacterInfo(int clientIndex)
        {
            string result = "Invalid client index";
            if((clientIndex >= 0) && (clientIndex < _characterStats.Length))
            {
                result = _characterStats[clientIndex];
            }

            return result;
        }

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
            // TODO: Remove file:/// from these, as it's only needed in WWW urls.
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
                ent.HostUIController.Setup(this);

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

        public string GetDocument(string path)
        {
            string result = _documents[path]; 
            return result;
        }

        void Start()
        {
            // TODO: Character data loading.
            _characterStats = new string[]
            {
                "Player 1 info",
                "Player 2 info",
                "Player 3 info",
                "Player 4 info",
                "Player 5 info",
                "Player 6 info",
            };

            // TODO: Documents loading.
#if UNITY_EDITOR
            string win32TestAssetsPath = "Assets\\";
#else
            string win32TestAssetsPath = "Test\\";
#endif
            string testDoc1 = win32TestAssetsPath + "testDoc1.txt";
            string testDoc2 = win32TestAssetsPath + "testDoc2.txt";
            FileStream fs1 = new FileStream(testDoc1, FileMode.Open, FileAccess.Read, FileShare.Read);
            FileStream fs2 = new FileStream(testDoc2, FileMode.Open, FileAccess.Read, FileShare.Read);
            
            Docs.Add(testDoc1);
            Docs.Add(testDoc2);

            _documents = new Dictionary<string, string>(Docs.Count)
            {
                { testDoc1, new StreamReader(fs1).ReadToEnd() },
                { testDoc2, new StreamReader(fs2).ReadToEnd() }
            };

            ServerStartup();

            _walls = FindObjectsOfType<Wall>();
        }
    }
}