using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance; 
    [SyncVar]
    public float timeCount = 180;
    public int day = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimeCounter();   
    }

    [Command]
    void TimeCounter(){
        timeCount -= Time.deltaTime;
    }
}
