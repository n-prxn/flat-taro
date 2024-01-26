using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    public int pulse = 300;
    public int urge = 100;
    public int sunflower = 0;

    private float sprintTimeCounter = 1, restTimeCounter = 3, pulseTimeCounter = 0.5f, urgeTimeCounter = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckSprint();
        CheckRest();
        CheckUrge();
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
}
