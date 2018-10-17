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

        [ServerCallback]
        void Start()
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
                    if(ent.isServer)
                    {
                        _host = ent;
                    }
                    else
                    {
                        _clients.Add(ent);
                    }
                }
            }
        }

        [ServerCallback]
        void Update()
        {
            
        }
    }
}