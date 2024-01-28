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
    [Scene][SerializeField] private string menuScene;

    [Header("Room")]
    [SerializeField] private NetworkRoomPlayerLobby roomPlayerPrefab;

    [Header("Game")]
    [SerializeField] private NetworkGamePlayerLobby gamePlayerPrefab;
    [SerializeField] private GameObject playerSpawnSystem;
    [SerializeField] private GameObject roundSystem;

    [SerializeField] bool isIP;

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action<NetworkConnection> OnServerReadied;
    public static event Action OnServerStopped;


    public List<NetworkRoomPlayerLobby> RoomPlayers { get; } = new List<NetworkRoomPlayerLobby>();
    public List<NetworkGamePlayerLobby> GamePlayers { get; } = new List<NetworkGamePlayerLobby>();

    public override void OnStartServer()
    {
        spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();
    }

    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");
        foreach (var prefab in spawnablePrefabs)
        {
            NetworkClient.RegisterPrefab(prefab);
        }
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        OnClientDisconnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        if (numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }

        if (SceneManager.GetActiveScene().name != "Main Menu")
        {
            conn.Disconnect();
            return;
        }
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            bool isLeader = RoomPlayers.Count == 0;
            NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(roomPlayerPrefab);
            roomPlayerInstance.IsLeader = isLeader;
            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if (conn.identity != null)
        {
            var player = conn.identity.GetComponent<NetworkRoomPlayerLobby>();
            RoomPlayers.Remove(player);
            NotifyPlayersOfReadyState();
        }

        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        OnServerStopped?.Invoke();

        RoomPlayers.Clear();
        GamePlayers.Clear();
    }

    public void NotifyPlayersOfReadyState()
    {
        foreach (var player in RoomPlayers)
        {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }

    private bool IsReadyToStart()
    {
        if (numPlayers < minPlayers)
            return false;

        foreach (var player in RoomPlayers)
        {
            if (!player.IsReady)
                return false;
        }

        return true;
    }

    public void StartGame()
    {
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            if (!IsReadyToStart())
                return;

            int num = UnityEngine.Random.Range(0, 2);
            if (num % 2 == 0)
            {
                ServerChangeScene("Gameplay");
            }
            else
            {
                ServerChangeScene("Gameplay 2");
            }

        }
    }

    public override void ServerChangeScene(string newSceneName)
    {
        //from Menu to Game
        if (SceneManager.GetActiveScene().name == "Main Menu" && newSceneName.StartsWith("Gameplay"))
        {
            for (int i = RoomPlayers.Count - 1; i >= 0; i--)
            {
                var conn = RoomPlayers[i].connectionToClient;
                var gamePlayerInstance = Instantiate(gamePlayerPrefab);
                gamePlayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);
                gamePlayerInstance.SetDeathCount(0);

                NetworkServer.Destroy(conn.identity.gameObject);
                NetworkServer.ReplacePlayerForConnection(conn, gamePlayerInstance.gameObject);
            }
        }

        base.ServerChangeScene(newSceneName);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName.StartsWith("Gameplay"))
        {
            GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
            NetworkServer.Spawn(playerSpawnSystemInstance);

            GameObject roundSystemInstance = Instantiate(roundSystem);
            NetworkServer.Spawn(roundSystemInstance);
        }
    }

    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);
        OnServerReadied?.Invoke(conn);
    }
}
