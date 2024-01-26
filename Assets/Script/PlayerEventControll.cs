using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerEventControll : NetworkBehaviour
{
    [SerializeField] PlayerStatus playerStatus;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this.isLocalPlayer)
        {
            if (other.CompareTag("SunflowerSeed"))
            {
                playerStatus.sunflower++;
                DestroySunflower(other.gameObject);
            }
        }
    }

    [Command]
    void DestroySunflower(GameObject sunflower)
    {
        Destroy(sunflower);
    }

}
