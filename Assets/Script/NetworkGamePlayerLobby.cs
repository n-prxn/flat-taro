using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NetworkGamePlayerLobby : NetworkBehaviour
{
    [SyncVar]
    private string displayName = "Loading...";

    [SyncVar]
    private int deathCount = 0;

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

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);
        Room.GamePlayers.Add(this);
    }

    public override void OnStopClient()
    {
        Room.GamePlayers.Remove(this);
    }

    [Server]
    public void SetDisplayName(string displayName){
        this.displayName = displayName;
    }

    [Server]
    public void SetDeathCount(int deathCount){
        this.deathCount = deathCount;
    }
    
    public string GetDisplayName(){
        return this.displayName;
    }

    public int GetDeathCount(){
        return deathCount;
    }
}
