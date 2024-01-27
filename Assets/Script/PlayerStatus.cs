using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    public int pulse = 300;
    public int urge = 100;
    public int sunflower = 0;

    [SerializeField] bool isDead;
    [SerializeField] float deadTime;

    private float sprintTimeCounter = 1, restTimeCounter = 3, pulseTimeCounter = 0.5f, urgeTimeCounter = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.canPlayerMove)
        {
            CheckSprint();
            CheckRest();
            CheckUrge();
            Addsunflower();
        }

    }

    void CheckSprint()
    {
        if (playerController.IsSprint)
        {
            restTimeCounter = 3;
            pulseTimeCounter = 1;

            sprintTimeCounter -= Time.deltaTime;
            if (sprintTimeCounter <= 0)
            {
                pulse += 20;
                sprintTimeCounter = 1;
            }
        }
        else
        {
            restTimeCounter -= Time.deltaTime;
        }
    }

    void CheckRest()
    {
        if (restTimeCounter <= 0)
        {
            pulseTimeCounter -= Time.deltaTime;

            if (pulse <= 300)
            {
                pulse = 300;
                restTimeCounter = 3;
            }

            if (pulseTimeCounter <= 0)
            {
                pulse -= 1;
                pulseTimeCounter = 0.5f;
            }
        }
    }

    void CheckUrge()
    {
        urgeTimeCounter -= Time.deltaTime;
        if (urgeTimeCounter <= 0)
        {
            urge -= 1;
            urgeTimeCounter = 1;
        }
    }

    void Addsunflower()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            sunflower++;
        }
    }

    public void StartSetDead()
    {
        StartCoroutine("SetDead");
    }

    IEnumerator SetDead()
    {
        isDead = true;
        yield return new WaitForSeconds(deadTime);
        isDead = false;
    }

}
