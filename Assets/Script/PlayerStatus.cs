using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public enum ItemBuffState
{
    energyBoost,
    ballProtection,
    timeBomb,
    none
}

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    public int pulse = 300;
    public int urge = 100;
    public int sunflower = 0;
    public ItemSO heldItem;

    [SerializeField] bool isDead;
    [SerializeField] ItemBuffState itemBuffState = ItemBuffState.none;
    [SerializeField] float deadTime;

    [SerializeField] public bool isInteractUrge;

    private float sprintTimeCounter = 1, restTimeCounter = 3, pulseTimeCounter = 0.5f, urgeTimeCounter = 1;
    private float energyBoostBuffCounter = 45;
    private void Awake()
    {
        isInteractUrge = true;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.canPlayerMove)
        {
            if (!isDead)
            {
                CheckDead();
                UseItem();

                if (itemBuffState != ItemBuffState.energyBoost)
                    CheckSprint();
                else
                {
                    if (energyBoostBuffCounter > 0)
                        energyBoostBuffCounter -= Time.deltaTime;
                    else
                    {
                        energyBoostBuffCounter = 45;
                        itemBuffState = ItemBuffState.none;
                    }
                }

                CheckRest();

                if (isInteractUrge)
                    CheckUrge();

                Addsunflower();
            }
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

    void CheckDead()
    {
        if (!isDead)
        {
            if (pulse > 600)
                StartSetDead();

            if (urge <= 0)
                StartSetDead();
        }
    }

    public void StartSetDead()
    {
        StartCoroutine("SetDead");
    }

    IEnumerator SetDead()
    {
        Die();
        yield return new WaitForSeconds(deadTime);
        Respawn();
    }

    public void Die()
    {
        gameObject.GetComponent<PlayerController>().PlayerAnimator.SetBool("isDead", true);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        isDead = true;
    }

    public void Respawn()
    {
        pulse = 300;
        urge = 100;
        sunflower = 0;
        heldItem = null;
        itemBuffState = ItemBuffState.none;

        gameObject.GetComponent<PlayerController>().PlayerAnimator.SetBool("isDead", false);
        gameObject.GetComponent<PlayerController>().PlayerAnimator.Play("Idle");
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        isDead = false;
    }

    public void UseItem()
    {
        if (heldItem == null)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            switch (heldItem.itemID)
            {
                case 0:
                    itemBuffState = ItemBuffState.energyBoost;
                    break;
                case 1:
                    itemBuffState = ItemBuffState.ballProtection;
                    break;
                case 2:
                    urge += 50;
                    break;
                case 3:
                    break;
                case 4:
                    break;
            }
            heldItem = null;
        }

    }

}
