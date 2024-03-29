using System;
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

public class PlayerStatus : NetworkBehaviour
{
    [SerializeField] private PlayerController playerController;
    public int pulse = 300;
    public int urge = 100;
    public int sunflower = 0;
    [SyncVar]
    public int deathCount = 0;
    public ItemSO heldItem;

    [SerializeField] bool isDead;
    [SerializeField] ItemBuffState itemBuffState = ItemBuffState.none;
    [SerializeField] float deadTime;

    [SerializeField] public bool isInteractUrge;

    private float sprintTimeCounter = 1, restTimeCounter = 3, pulseTimeCounter = 0.5f, urgeTimeCounter = 1;
    private float energyBoostBuffCounter = 45;
    private void Awake()
    {
        isInteractUrge = false;
    }

    public override void OnStartAuthority()
    {
        deathCount = 0;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    [ClientCallback]
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

                if (!isInteractUrge && !isDead)
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
        if (!isDead)
            deathCount++;
        Die();
        CmdAddDeathCount();
        yield return new WaitForSeconds(deadTime);
        Respawn();
    }

    [Command]
    private void CmdAddDeathCount()
    {
        RPCSetDeathCount(playerController.index, deathCount);
    }

    [ClientRpc]
    private void RPCSetDeathCount(int index, int deathCount)
    {
        var networkPlayers = FindObjectsOfType<NetworkGamePlayerLobby>();
        foreach (var i in networkPlayers)
        {
            if (playerController.playerName == i.GetComponent<NetworkGamePlayerLobby>().displayName)
            {
                i.GetComponent<NetworkGamePlayerLobby>().SetDeathCount(deathCount);
            }
        }
        //playerController.Room.GamePlayers[index].SetDeathCount(deathCount);
    }

    public void Die()
    {
        SetAniCMD("Dead");
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        isDead = true;
    }
    public void Respawn()
    {
        pulse = 300;
        if (urge <= 0)
        {
            urge = 30;
        }
        sunflower = 0;
        heldItem = null;
        itemBuffState = ItemBuffState.none;

        SetAniCMD("Idle");
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
                    if (urge >= 100)
                        urge = 100;
                    break;
                case 3:
                    break;
                case 4:
                    break;
            }
            heldItem = null;
        }

    }

    [Command]
    void SetAniCMD(String name)
    {
        SetAni(name);
    }
    [ClientRpc]
    void SetAni(String name)
    {
        gameObject.GetComponent<PlayerController>().PlayerAnimator.Play(name);
    }

}
