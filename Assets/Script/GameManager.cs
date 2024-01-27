using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    [SyncVar] public float timeCount = 180;

    [SerializeField] Bounds floor;
    public float subflowerCount = 0;
    public float MaxSubflower;
    [SerializeField] float spawnTimer = 0;
    [SerializeField] float spawnTimerRate;
    [SerializeField] GameObject sunflowerPrefab;

    [SyncVar] public int day = 1;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        floor = this.gameObject.GetComponent<Renderer>().bounds;
    }

    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            TimeCounter();
            SunflowerSpawn();
        }
    }

    void TimeCounter()
    {
        if (timeCount > 0)
        {
            timeCount -= Time.deltaTime;
        }
    }

    void SunflowerSpawn()
    {
        if (spawnTimer >= spawnTimerRate)
        {
            spawnTimer = 0f;
            Instantiate(sunflowerPrefab, RandomSpawnpoint(), sunflowerPrefab.transform.rotation);
        }
        else
        {
            spawnTimer += Time.deltaTime;
        }
    }

    Vector3 RandomSpawnpoint()
    {
        float rndX, rndY;
        rndX = Random.Range(floor.min.x, floor.max.x);
        rndY = Random.Range(floor.min.y, floor.max.y);
        Vector3 spawnpoint = new Vector3(rndX, rndY, -0.1f);
        return spawnpoint;
    }
}
