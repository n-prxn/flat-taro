using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class InteractContoller : NetworkBehaviour
{
    [SerializeField][SyncVar] public bool isOnUse;
    [SerializeField] GameObject fButton;
    [SerializeField] GameObject isUseButton;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         if (isOnUse)
    //             other.GetComponent<PlayerInteractContoller>().SetActiveInteract(isUseButton, true);
    //         else
    //             other.GetComponent<PlayerInteractContoller>().SetActiveInteract(fButton, true);
    //     }
    // }

    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         other.GetComponent<PlayerInteractContoller>().SetActiveInteract(isUseButton, false);
    //         other.GetComponent<PlayerInteractContoller>().SetActiveInteract(fButton, false);
    //     }
    // }

    [Command(requiresAuthority = false)]
    public void IsOnUseFilp()
    {
        isOnUse = !isOnUse;
    }
    [Command(requiresAuthority = false)]
    public void IsOnUseFilp(bool value)
    {
        isOnUse = value;
    }

}
