using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoundSystem : NetworkBehaviour
{
    [SerializeField] private Animator animator;
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

    public void CountdownEnded()
    {
        animator.enabled = false;
    }

    #region Server
    public override void OnStartServer()
    {
        NetworkManagerLobby.OnServerStopped += CleanUpServer;
        NetworkManagerLobby.OnServerReadied += CheckToStartRound;
    }

    [ServerCallback]
    private void OnDestroy()
    {
        CleanUpServer();
    }

    [Server]
    private void CleanUpServer()
    {
        NetworkManagerLobby.OnServerStopped -= CleanUpServer;
        NetworkManagerLobby.OnServerReadied -= CheckToStartRound;

        GameManager.instance.canPlayerMove = false;
    }

    [ServerCallback]
    public void StartRound()
    {
        RpcStartRound();
    }

    [Server]
    private void CheckToStartRound(NetworkConnection conn)
    {
        if (Room.GamePlayers.Count(x => x.connectionToClient.isReady) != Room.GamePlayers.Count)
        {
            return;
        }

        animator.enabled = true;
        RpcStartCountdown();
    }
    #endregion

    #region Client
    [ClientRpc]
    private void RpcStartCountdown()
    {
        animator.enabled = true;
    }

    [ClientRpc]
    private void RpcStartRound()
    {
        Debug.Log("Start");
        GameManager.instance.canPlayerMove = true;
    }
    #endregion
}
