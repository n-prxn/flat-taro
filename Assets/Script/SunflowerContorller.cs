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
        if (isServer)
        {
            Debug.Log("Sunflower is Destroy");
            DestroySunflower();
        }
    }

    private void OnEnable()
    {
        SetSunflowerActive();
    }


    void SetSunflowerActive()
    {
        Debug.Log("Sunflower is Spwan");
        sunflowerSeedImg.SetActive(true);
    }

    [Command]
    void DestroySunflower()
    {
        Destroy(this);
    }
}
