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
    public ItemSO heldItem;

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
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        GameManager.instance.canPlayerMove = false;
        yield return new WaitForSeconds(deadTime);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        GameManager.instance.canPlayerMove = true;
        isDead = false;
    }

    public void UseItem(ItemSO item)
    {
        switch (item.itemID)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }

}
