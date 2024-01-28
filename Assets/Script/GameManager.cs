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

    [Header("=====Spawner Setting=====")]
    [SerializeField] float spawnEventTimeRate = 1f;

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

    [Header("=====Vacuum Event=====")]
    [SerializeField] GameObject VacuumEventPrefab;
    [SerializeField] float VacuumSpawnTimer = 0;
    [SerializeField] float VacuumSpawnTimerRate;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        floor = floorOBJ.bounds;
        if (isServer)
            InvokeRepeating("RandomEvent", 0f, spawnEventTimeRate);
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
            if (Input.GetKeyDown(KeyCode.N))
            {
                VacuumSpawnValueTest();
            }
        }
    }
    void RandomEvent()
    {
        if (canPlayerMove)
        {
            if (Random.Range(0, 10) > 6)
            {
                VacuumSpawnValueTest();
            }
            else
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

    void BookSpawnValueTest()
    {
        Vector3 tempPos = RandomSpawnpoint();
        BookSpawnTest(tempPos);
    }
    [Command(requiresAuthority = false)]
    void BookSpawnTest(Vector3 pos)
    {
        NetworkServer.Spawn(Instantiate(BookEventPrefab, pos, BookEventPrefab.transform.rotation));
    }

    void VacuumSpawnValueTest()
    {
        Vector3 tempPos = RandomSpawnpoint();
        VacuumSpawnTest(tempPos);
    }
    [Command(requiresAuthority = false)]
    void VacuumSpawnTest(Vector3 pos)
    {
        switch (Random.Range(0, 2))
        {
            case 0:
                NetworkServer.Spawn(Instantiate(VacuumEventPrefab, new Vector3(-89f, pos.y, pos.z), VacuumEventPrefab.transform.rotation));
                break;
            case 1:
                NetworkServer.Spawn(Instantiate(VacuumEventPrefab, new Vector3(89f, pos.y, pos.z), VacuumEventPrefab.transform.rotation));
                break;
        }
        // NetworkServer.Spawn(Instantiate(VacuumEventPrefab, pos, VacuumEventPrefab.transform.rotation));
    }

}
