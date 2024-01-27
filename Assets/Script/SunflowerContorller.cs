using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SunflowerContorller : NetworkBehaviour
{
    public bool canUse = false;
    [SerializeField] GameObject sunflowerSeedImg;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        DestroySunflower();
    }

    private void OnEnable()
    {
        SetSunflowerActiveCMD();
    }

    [Command]
    void SetSunflowerActiveCMD()
    {
        SetSunflowerActive();
    }


    [ClientRpc]
    void SetSunflowerActive()
    {
        sunflowerSeedImg.SetActive(true);
    }

    [Command]
    void DestroySunflower()
    {
        Destroy(this);
    }
}
