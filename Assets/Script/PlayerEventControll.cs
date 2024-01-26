using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerEventControll : NetworkBehaviour
{
    PlayerStatus playerStatus;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.isLocalPlayer)
        {
            if (playerStatus.sunflower == 10)
            {
                Debug.Log("Now My Sunflower is 10");
            }
        }
    }
}
