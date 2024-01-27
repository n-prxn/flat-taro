using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    [SyncVar] public float timeCount = 180;

    [SerializeField] Bounds floor;
    [SyncVar] public float sunflowerCount = 0;
    public float MaxSunflower;
    [SerializeField] float spawnTimer = 0;
    [SerializeField] float spawnTimerRate;
    [SerializeField] GameObject sunflowerPrefab;
    [SerializeField] Renderer floorOBJ;

    [SyncVar] public int day = 1;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        floor = floorOBJ.bounds;
    }

    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            TimeCounter();
            if (sunflowerCount < MaxSunflower)
                SunflowerSpawnTimeCount();
        }
    }

    void TimeCounter()
    {
        if (timeCount > 0)
        {
            timeCount -= Time.deltaTime;
        }
    }

    void SunflowerSpawnTimeCount()
    {
        if (spawnTimer >= spawnTimerRate)
        {
            SetSunflowerSpawnValue();
        }
        else
        {
            spawnTimer += Time.deltaTime;
        }
    }

    public void SetSunflowerSpawnValue()
    {
        spawnTimer = 0f;
        sunflowerCount++;
        Vector3 tempPos = RandomSpawnpoint();
        SunflowerSpawn(tempPos);
    }

    Vector3 RandomSpawnpoint()
    {
        float rndX, rndY;
        rndX = Random.Range(floor.min.x, floor.max.x);
        rndY = Random.Range(floor.min.y, floor.max.y);
        Vector3 spawnpoint = new Vector3(rndX, rndY, -0.1f);
        return spawnpoint;
    }

    [Command(requiresAuthority = false)]
    void SunflowerSpawn(Vector3 pos)
    {
        GameObject tempOBJ = Instantiate(sunflowerPrefab, pos, sunflowerPrefab.transform.rotation);
        NetworkServer.Spawn(tempOBJ);
    }
}
