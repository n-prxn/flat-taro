using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using Cinemachine;
using Unity.VisualScripting;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Transform hamsterTF;
    private Vector2 moveDirection;
    private int faceDirection = 1;
    private bool isSprint = false;
    public bool IsSprint { get => isSprint; set => isSprint = value; }
    private bool isIdle = true;

    private bool canPlayerMove = true;
    public bool CanPlayerMove { get => canPlayerMove; set => canPlayerMove = value; }
    [SerializeField][SyncVar] bool flip;

    [SerializeField] GameObject GUIobj;
    [SerializeField] Animator playerAnimator;
    public Animator PlayerAnimator{
        get{return playerAnimator;}
        set{playerAnimator = value;}
    }

    public override void OnStartAuthority()
    {
        enabled = true;
    }

    // Update is called once per frame
    // [ClientCallback]

    [ClientCallback]
    void Update()
    {
        if (GameManager.instance.canPlayerMove && canPlayerMove)
            ProcessInput();
    }

    [ClientCallback]
    private void FixedUpdate()
    {
        if (GameManager.instance.canPlayerMove && canPlayerMove)
        {
            Move();
            FlipCmd();
        }
        else
        {
            playerAnimator.SetBool("isIdle", true);
        }
        //Physics Calculations
    }

    [Client]
    void ProcessInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
            isSprint = true;
        else
            isSprint = false;

        moveDirection = new Vector2(moveX, moveY);
        // Flip(moveX);

        if (moveDirection.magnitude == 0)
            isIdle = true;
        else
            isIdle = false;

        playerAnimator.SetBool("isIdle", isIdle);
        playerAnimator.SetBool("isSprint", isSprint);
    }

    [Client]
    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed * (isSprint ? 2f : 1f), moveDirection.y * moveSpeed * (isSprint ? 2 : 1f));
        Flip(moveDirection.x);
    }
    [Client]
    void Flip(float moveX)
    {
        if (moveX < 0)
        {
            // hamsterTF.localScale = new Vector3(1f, 1f, 1f);
            SetFlipCmd(true);
            // hamsterTF.gameObject.GetComponent<SpriteRenderer>().flipX = flip;
            faceDirection = -1;
        }
        else if (moveX > 0)
        {
            // hamsterTF.localScale = new Vector3(-1f, 1f, 1f);
            SetFlipCmd(false);
            // hamsterTF.gameObject.GetComponent<SpriteRenderer>().flipX = flip;
            faceDirection = 1;
        }
    }

    [Command]
    void SetFlipCmd(bool value)
    {
        flip = value;
    }

    [Command]
    void FlipCmd()
    {
        Flip();
    }
    [ClientRpc]
    void Flip()
    {
        hamsterTF.gameObject.GetComponent<SpriteRenderer>().flipX = flip;
    }
}
