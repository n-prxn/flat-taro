using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SunflowerContorller : NetworkBehaviour
{
    [SyncVar] public bool canUse;
    [SerializeField] GameObject sunflowerSeedImg;

    // Start is called before the first frame update
    void Start()
    {
        if (isServer)
            Invoke("SetCanUse", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        Debug.Log("Try Spawn Sunflower");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isServer && !canUse)
        {
            Debug.Log("Destroy sunflower");
            GameManager.instance.SetSunflowerSpawnValue();
            NetworkServer.Destroy(this.gameObject);
        }
    }

    void SetCanUse()
    {
        Debug.Log("Sunflower Can Spawn");
        canUse = true;
        SetSunflowerActive();
    }

    [ClientRpc]
    void SetSunflowerActive()
    {
        sunflowerSeedImg.SetActive(true);
    }

}
