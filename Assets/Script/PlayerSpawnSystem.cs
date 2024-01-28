using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnSystem : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    private static List<Transform> spawnPoints = new List<Transform>();

    private int nextIndex = 0;

    private NetworkManagerLobby room;
    private NetworkManagerLobby Room
    {
        get
        {
            if (room != null)
                return room;
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    public static void AddSpawnPoint(Transform transform){
        spawnPoints.Add(transform);
        spawnPoints = spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
    }

    public static void RemoveSpawnPoint(Transform transform){
        spawnPoints.Remove(transform);
    }

    public override void OnStartServer(){
        NetworkManagerLobby.OnServerReadied += SpawnPlayer;
    }

    [ServerCallback]
    private void OnDestroy() {
        NetworkManagerLobby.OnServerReadied -= SpawnPlayer;
    }

    [Server]
    public void SpawnPlayer(NetworkConnection conn){
        Transform spawnPoint = spawnPoints.ElementAtOrDefault(nextIndex);
        if(spawnPoint == null){
            return;
        }

        GameObject playerInstance = Instantiate(playerPrefab, spawnPoints[nextIndex].position, spawnPoints[nextIndex].rotation);
        Debug.Log("Room name is " + Room.GamePlayers[nextIndex].displayName);
        playerInstance.GetComponent<PlayerController>().playerName = Room.GamePlayers[nextIndex].displayName;
        NetworkServer.Spawn(playerInstance, conn);

        nextIndex++;
    }
}
