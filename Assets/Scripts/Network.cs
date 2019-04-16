using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using RolePlayOverlord.UI;

namespace RolePlayOverlord
{
    /*
        TODO: Delete the cache dictionaries as we don't want to store assets in memory all the time.
        TODO: Clean up all UI related code.
        [?]TODO: Inline ServerStartup.
        [?]TODO: Instantiate UIs.
    */
    public class Network : NetworkBehaviour
    {
        NetworkConnection[] _clientConnections;
        ClientEntity[] _clients;
        // TODO: Does anything need to talk to the host directly?
        ClientEntity _host;

        ModData _modData;

        string[] _characterStats;
        
        Dictionary<string, Texture2D> _textureCache = new Dictionary<string, Texture2D>();
        
        Wall[] _walls;
        
        [SerializeField] private GameObject _hostUI;
        [SerializeField] private GameObject _playerUI;
        UIController _activeUI;

        const int CAPACITY = 32;
        const int LAST_INDEX = CAPACITY - 1;
        string[] _chat = new string[CAPACITY];
        int _count = 0;

        public string GetClientCharacterInfo(int clientIndex)
        {
            string result = "Invalid client index";
            if((clientIndex >= 0) && (clientIndex < _characterStats.Length))
            {
                result = _characterStats[clientIndex];
            }

            return result;
        }

        [Command]
        public void CmdOnChatMessage(string message)
        {
            RpcOnChatMessage(message);
        }

        [ClientRpc]
        public void RpcOnChatMessage(string message)
        {
            if(_count <= LAST_INDEX)
            {
                _chat[_count] = message;
                _count++;
            }
            else
            {
                for(int i = 0;
                    i < LAST_INDEX;
                    ++i)
                {
                    _chat[i] = _chat[i + 1];
                }
                _chat[LAST_INDEX] = message;
            }

            StringBuilder sb = new StringBuilder();
            for(int i = 0;
                i < _count;
                ++i)
            {
                sb.AppendLine(_chat[i]);
            }
            _activeUI.UpdateChatWindow(sb.ToString());
        }
        
        IEnumerator LoadTex(Texture2D tex, string path)
        {
            string url = "file:///" + IO.WorkingDirectory + path;
            using(WWW www = new WWW(url))
            {
                yield return www;
                www.LoadImageIntoTexture(tex);
            }
        }

        [Command]
        public void CmdOnResourceButtonClick(ResourceData resourceData)
        {
            ResourceTypeID resourceTypeID = resourceData.ResourceType;
            int firstResourceIndex = _modData.ResourceTypeEntries[(int)resourceTypeID].FirstResourceIndex;
            switch(resourceTypeID)
            {
                case ResourceTypeID.WallTexture:
                {
                    int resourceOffset = firstResourceIndex + resourceData.ID;
                    string wallTextureFile = _modData.Resources[resourceOffset].File;
                    RpcSetWallTexture(wallTextureFile);
                    break;
                }
                case ResourceTypeID.FloorTexture:
                {
                    Debug.LogError("Resource type: " + resourceTypeID + " Resource: " + resourceData.ID);
                    break;
                }
                case ResourceTypeID.CeilingTexture:
                {
                    Debug.LogError("Resource type: " + resourceTypeID + " Resource: " + resourceData.ID);
                    break;
                }
                case ResourceTypeID.SkyboxTexture:
                {
                    Debug.LogError("Resource type: " + resourceTypeID + " Resource: " + resourceData.ID);
                    break;
                }
                case ResourceTypeID.Audio:
                {
                    Debug.LogError("Resource type: " + resourceTypeID + " Resource: " + resourceData.ID);
                    break;
                }

                default:
                {
                    Debug.LogError("Invalid resource " + resourceTypeID + " assigned to a ResourceButton!");
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
                _hostUI.GetComponent<HostUIController>().Setup(this, _modData);
                _activeUI = _hostUI.GetComponent<HostUIController>();
            }
            else
            {
                _playerUI.GetComponent<PlayerUIController>().Setup();
            }
        }

        public string GetDocument(string path)
        {
            string result = IO.LoadDocument(path);
            return result;
        }

        public void UpdateDocument(string path, string data)
        {
            IO.SaveDocument(path, data);
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
            IO.WorkingDirectory = "Mods/Default/";
            string modManifestName = IO.WorkingDirectory + "Default.rmm";
            IO.LoadModData(ref _modData, modManifestName);

            ServerStartup();

            _walls = FindObjectsOfType<Wall>();
        }
    }
}