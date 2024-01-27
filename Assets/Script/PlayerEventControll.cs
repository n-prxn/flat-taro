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

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this.isLocalPlayer)
        {
            if (other.CompareTag("SunflowerSeed"))
            {
                if (other.gameObject.GetComponent<SunflowerContorller>().canUse)
                {
                    playerStatus.sunflower++;
                    DestroySunflower(other.gameObject);
                }
            }
        }
    }

    [Command]
    void DestroySunflower(GameObject sunflower)
    {
        GameManager.instance.sunflowerCount--;
        NetworkServer.Destroy(sunflower);
    }

}
