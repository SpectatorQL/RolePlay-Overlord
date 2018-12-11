﻿using System.Collections;
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

        string[] _characterStats;
        public List<string> Documents = new List<string>();

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
        
        IEnumerator LoadTex(Texture2D tex, string path)
        {
            string url = "file:///" + path;
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
            string texPath = GetAssetFilePath(texName);

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

            // TODO: Load GM's documents or player's diary.
        }

        public string GetDocument(string path)
        {
            string result = "";

            using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using(StreamReader sr = new StreamReader(fs))
            {
                result = sr.ReadToEnd();
            }

            return result;
        }

        public void UpdateDocument(string path, string data)
        {
            using(var fs = new FileStream(path, FileMode.Truncate, FileAccess.Write, FileShare.Read))
            using(var sw = new StreamWriter(fs))
            {
                sw.Write(data);
            }
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

            Mod defaultMod;
            string defaultModManifest = DEFAULT_ASSETS_PATH + "Default.rmm";

            using(var modStream = new FileStream(PATH(defaultModManifest), FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                defaultMod = (Mod)formatter.Deserialize(modStream);
            }

            ServerStartup();

            _walls = FindObjectsOfType<Wall>();
        }
    }
}