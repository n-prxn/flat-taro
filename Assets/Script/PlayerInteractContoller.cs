using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerInteractContoller : NetworkBehaviour
{
    [SerializeField] GameObject tempInteractOBJ;
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
            }
        }
    }
    public void SetActiveInteract(GameObject obj, bool value)
    {
        if (isLocalPlayer)
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
                tempInteractOBJ = other.gameObject;
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
        }
    }
}
