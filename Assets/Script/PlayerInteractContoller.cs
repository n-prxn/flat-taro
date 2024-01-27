using System.Collections;
using System.Collections.Generic;
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
            if (Input.GetKeyDown(KeyCode.F))
            {
                tempInteractOBJ.GetComponent<InteractContoller>().IsOnUseFilp();
                if(tempInteractOBJ.gameObject.name.StartsWith("InteractShopPrefab")){
                    shopPanel.SetActive(true);
                    gameObject.GetComponent<PlayerController>().CanPlayerMove = false;
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
}
