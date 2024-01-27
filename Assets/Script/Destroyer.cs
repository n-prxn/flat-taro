using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Destroyer : NetworkBehaviour
{
    [Client]
    private void OnTriggerEnter2D(Collider2D other)
    {
        // DestroyerCmd(other.gameObject);
        Debug.Log("Hit Destroyer");
    }
    [Command]
    void DestroyerCmd(GameObject obj)
    {
        NetworkServer.Destroy(obj);
    }
}
