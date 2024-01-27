using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [HideInInspector] public static GameManager instance;
    [Header("=====Basic=====")]
    [SyncVar] public float timeCount = 180;
    [SyncVar] public int day = 1;
    [SyncVar] public bool canPlayerMove;

    [Header("=====Random area=====")]
    [SerializeField] Bounds floor;
    [SerializeField] Renderer floorOBJ;

    [Header("=====Sunflower Spawner=====")]
    [SyncVar] public float sunflowerCount = 0;
    public float MaxSunflower;
    [SerializeField] float sunflowerSpawnTimer = 0;
    [SerializeField] float sunflowerSpawnTimerRate;
    [SerializeField] GameObject sunflowerPrefab;

    [Header("=====Book Event=====")]
    [SerializeField] GameObject BookEventPrefab;
    [SerializeField] float bookSpawnTimer = 0;
    [SerializeField] float bookSpawnTimerRate;


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
            // BookSpawnTimeCount();
            if (Input.GetKeyDown(KeyCode.M))
            {
                BookSpawnValueTest();
            }
        }
    }

    Vector3 RandomSpawnpoint()
    {
        float rndX, rndY;
        rndX = Random.Range(floor.min.x, floor.max.x);
        rndY = Random.Range(floor.min.y, floor.max.y);
        Vector3 spawnpoint = new Vector3(rndX, rndY, 0f);
        return spawnpoint;
    }

    void TimeCounter()
    {
        if (timeCount > 0 && canPlayerMove)
        {
            timeCount -= Time.deltaTime;
        }
    }

    void SunflowerSpawnTimeCount()
    {
        if (sunflowerSpawnTimer >= sunflowerSpawnTimerRate)
        {
            SetSunflowerSpawnValue();
        }
        else
        {
            sunflowerSpawnTimer += Time.deltaTime;
        }
    }

    public void SetSunflowerSpawnValue()
    {
        sunflowerSpawnTimer = 0f;
        sunflowerCount++;
        Vector3 tempPos = RandomSpawnpoint();
        SunflowerSpawn(tempPos);
    }

    [Command(requiresAuthority = false)]
    void SunflowerSpawn(Vector3 pos)
    {
        NetworkServer.Spawn(Instantiate(sunflowerPrefab, pos, sunflowerPrefab.transform.rotation));
    }

    void BookSpawnTimeCount()
    {
        if (bookSpawnTimer >= bookSpawnTimerRate)
        {
            SetSunflowerSpawnValue();
        }
        else
        {
            sunflowerSpawnTimer += Time.deltaTime;
        }
    }

    void BookSpawnValueTest()
    {
        Vector3 tempPos = RandomSpawnpoint();
        BookSpawnTest(tempPos);
    }
    [Command(requiresAuthority = false)]
    void BookSpawnTest(Vector3 pos)
    {
        NetworkServer.Spawn(Instantiate(BookEventPrefab, pos, sunflowerPrefab.transform.rotation));
    }

}
