using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    public int pulse = 300;
    public int urge = 100;
    public int sunflower = 0;

    private float sprintTimeCounter = 1 , restTimeCounter = 3 , pulseTimeCounter = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
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
        }else{
            restTimeCounter -= Time.deltaTime;
        }

        if(restTimeCounter <= 0){
            pulseTimeCounter -= Time.deltaTime;

            if(pulse <= 300){
                pulse = 300;
                restTimeCounter = 3;
            }

            if(pulseTimeCounter <= 0){
                pulse -= 2;
                pulseTimeCounter = 1;
            }
        }
    }
}
