using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using RolePlayOverlord.UI;
using static RolePlayOverlord.IO;

namespace RolePlayOverlord
{
    /*
        TODO: Delete the cache dictionaries as we don't want to store assets in memory all the time.
    */
    public class Network : NetworkBehaviour
    {
        NetworkConnection[] _clientConnections;
        ClientEntity[] _clients;
        // TODO: Does anything need to talk to the host directly?
        ClientEntity _host;

        Mod _mod;

        string[] _characterStats;
        
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
        
        IEnumerator LoadTex(Texture2D tex, string path)
        {
            string url = "file:///" + path;
            using(WWW www = new WWW(url))
            {
                yield return www;
                www.LoadImageIntoTexture(tex);
            }
        }

        [Command]
        public void CmdOnResourceButtonClick(ResourceType resourceType, string resource)
        {
            switch(resourceType)
            {
                case ResourceType.WallTexture:
                {
                    RpcSetWallTexture(resource);
                    break;
                }
                case ResourceType.FloorTexture:
                {
                    Debug.LogError("Resource type: " + resourceType + " Resource: " + resource);
                    break;
                }
                case ResourceType.CeilingTexture:
                {
                    Debug.LogError("Resource type: " + resourceType + " Resource: " + resource);
                    break;
                }
                case ResourceType.SkyboxTexture:
                {
                    Debug.LogError("Resource type: " + resourceType + " Resource: " + resource);
                    break;
                }
                case ResourceType.Audio:
                {
                    Debug.LogError("Resource type: " + resourceType + " Resource: " + resource);
                    break;
                }

                default:
                {
                    Debug.LogError("Invalid resource " + resourceType + " assigned to a ResourceButton!");
                    break;
                }
            }
        }

        [ClientRpc]
        public void RpcSetWallTexture(string texPath)
        {
            // NOTE(SpectatorQL): Do these two even matter?
            int textureWidth = 1024;
            int textureHeight = 1024;

            Texture2D tex;
            if(_textureCache.ContainsKey(texPath))
            {
                tex = _textureCache[texPath];
            }
            else
            {
                tex = new Texture2D(textureWidth, textureHeight);
                StartCoroutine(LoadTex(tex, texPath));
                _textureCache.Add(texPath, tex);
            }

            for(int i = 0; i < _walls.Length; ++i)
            {
                _walls[i].ChangeWallTexture(tex);
            }
        }

        void ClientStartup(ClientEntity ent)
        {
            if(!ent.isLocalPlayer)
                return;

            if(ent.isServer)
            {
                ent.ProcessKeyboardInput = PlayerInput.ProcessHostKeyboard;
                ent.ProcessMouseInput = PlayerInput.ProcessHostMouse;

                _host = ent;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
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

            if(isServer)
            {
                _hostUI.GetComponent<HostUIController>().Setup(this, _mod);
            }
            else
            {
                _playerUI.GetComponent<PlayerUIController>().Setup();
            }
        }

        public string GetDocument(string path)
        {
            string result = LoadDocument(path);
            return result;
        }

        public void UpdateDocument(string path, string data)
        {
            SaveDocument(path, data);
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
            
            // TODO: Get the actual manifest file, probably from the NetworkManager or another script attached to it.
            string modManifest = DEFAULT_ASSETS_PATH + "Default.rmm";
            LoadCurrentMod(ref _mod, modManifest);

            _mod.LocalData = new LocalData();
            LoadLocalData(ref _mod.LocalData);

            ServerStartup();

            _walls = FindObjectsOfType<Wall>();
        }
    }
}