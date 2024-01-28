using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Mirror;
using UnityEngine;

public class PlayerInteractContoller : NetworkBehaviour
{
    [SerializeField] GameObject tempInteractOBJ;
    [SerializeField] GameObject fButton;
    [SerializeField] GameObject isUseButton;

    [Header("Interact Event")]
    [SerializeField] GameObject shopPanel;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    [ClientCallback]
    void Update()
    {
        if (tempInteractOBJ != null)
        {
            if (Input.GetKeyDown(KeyCode.F) && !tempInteractOBJ.GetComponent<InteractContoller>().isOnUse)
            {
                tempInteractOBJ.GetComponent<InteractContoller>().IsOnUseFilp();
                gameObject.GetComponent<PlayerController>().CanPlayerMove = false;
                gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                switch (tempInteractOBJ.GetComponent<InteractContoller>().interactType)
                {
                    case InteractType.Shop:
                        shopPanel.SetActive(true);
                        break;
                    case InteractType.Urge:
                        // gameObject.GetComponent<PlayerController>().PlayerAnimator.SetBool("isInteract", true);
                        SetAniCMD("Interact");
                        gameObject.GetComponent<PlayerStatus>().isInteractUrge = true;
                        StartAddUrge();
                        break;
                }
            }
        }
    }

    [Client]
    public void SetActiveInteract(GameObject obj, bool value)
    {
        if (isClient)
            obj.SetActive(value);

    }

    // private void OnTriggerStay2D(Collider2D other)
    // {
    //     if (isLocalPlayer)
    //         if (other.CompareTag("InteractOBJ"))
    //         {
    //             if (Input.GetKeyDown(KeyCode.F))
    //                 Debug.Log("Is on strry");
    //         }
    // }
    [Client]
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("InteractOBJ"))
        {
            if (!other.GetComponent<InteractContoller>().isOnUse)
            {
                SetActiveInteract(fButton, true);
                tempInteractOBJ = other.gameObject;
            }
            else
            {
                SetActiveInteract(isUseButton, true);
            }

        }
    }
    [Client]
    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("InteractOBJ"))
        {
            if (tempInteractOBJ != null)
                other.GetComponent<InteractContoller>().IsOnUseFilp(false);
            tempInteractOBJ = null;
            SetActiveInteract(isUseButton, false);
            SetActiveInteract(fButton, false);
        }
    }

    [Command]
    void SetActiveInteractCMD(GameObject obj, bool value)
    {
        SetActiveInteractRpc(obj, value);
    }
    [TargetRpc]
    void SetActiveInteractRpc(GameObject obj, bool value)
    {
        obj.SetActive(value);
    }

    public void StartAddUrge()
    {
        StartCoroutine(AddUrge());
    }

    IEnumerator AddUrge()
    {
        float targetTimer = 5f;
        float timer = 0f;
        int count = 0;
        while (timer < targetTimer)
        {
            timer += Time.deltaTime;
            if (timer >= count)
            {
                count++;
                gameObject.GetComponent<PlayerStatus>().urge += 6;
                if (gameObject.GetComponent<PlayerStatus>().urge >= 100)
                    gameObject.GetComponent<PlayerStatus>().urge = 100;
            }
            yield return null;
        }

        gameObject.GetComponent<PlayerController>().CanPlayerMove = true;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        // gameObject.GetComponent<PlayerController>().PlayerAnimator.SetBool("isInteract", false);
        SetAniCMD("Idle");
        gameObject.GetComponent<PlayerStatus>().isInteractUrge = false;
        tempInteractOBJ.GetComponent<InteractContoller>().IsOnUseFilp(false);
    }

    public void SetBodyTypeToDynamic()
    {
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
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
