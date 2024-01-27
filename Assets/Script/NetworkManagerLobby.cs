using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System;
using System.Linq;
using UnityEngine.EventSystems;

public class NetworkManagerLobby : NetworkManager
{
    [SerializeField] private int minPlayers = 2;
    [Scene] [SerializeField] private string menuScene;

    [Header("Room")]
    [SerializeField] private NetworkRoomPlayerLobby roomPlayerPrefab = null;

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;

    public override void OnStartServer(){
        spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();
    }

    public override void OnStartClient(){
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");
        foreach(var prefab in spawnablePrefabs){
            NetworkClient.RegisterPrefab(prefab);
        }
    }

    public override void OnClientConnect(){
        base.OnClientConnect();
        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect(){
        base.OnClientDisconnect();
        OnClientDisconnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnectionToClient conn){
        base.OnServerConnect(conn);
        if(numPlayers >= maxConnections){
            conn.Disconnect();
            return;
        }

        if(SceneManager.GetActiveScene().name != menuScene){
            conn.Disconnect();
            return;
        }
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn){
        if(SceneManager.GetActiveScene().name == menuScene){
            NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(roomPlayerPrefab);
            NetworkServer.AddPlayerForConnection(conn,roomPlayerInstance.gameObject);
        }
    }
}
