using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SunflowerContorller : NetworkBehaviour
{
    public bool canUse;
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
        if (isServer)
        {
            SetSunflowerActive(false);
            Invoke("SetCanUse", 0.1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isServer && !canUse)
        {
            Debug.Log("Destroy sunflower");
            GameManager.instance.SetSunflowerSpawnValue();
            GameManager.instance.sunflowerCount--;
            NetworkServer.Destroy(this.gameObject);
        }
    }

    void SetCanUse()
    {
        SetSunflowerActive(true);
    }

    [ClientRpc]
    void SetSunflowerActive(bool value)
    {
        canUse = value;
        sunflowerSeedImg.SetActive(value);
    }

}
