using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using Cinemachine;
using Unity.VisualScripting;
using System;
using UnityEngine.SceneManagement;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    public int index;
    public string playerName = "";
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
    public Animator PlayerAnimator
    {
        get { return playerAnimator; }
        set { playerAnimator = value; }
    }

    public override void OnStartAuthority()
    {
        enabled = true;
    }

    [ClientCallback]
    private void Awake()
    {
        SearchForThisPlayer();
    }

    // Update is called once per frame
    // [ClientCallback]
    private NetworkManagerLobby room;
    public NetworkManagerLobby Room
    {
        get
        {
            if (room != null)
                return room;
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    [ClientCallback]
    void Update()
    {
        if (GameManager.instance.canPlayerMove && canPlayerMove)
            ProcessInput();

        if (Input.GetKeyDown(KeyCode.X))
        {
            // var gamePlayers = FindObjectsOfType<NetworkGamePlayerLobby>();
            // foreach (var player in gamePlayers)
            // {
            //     // Debug.Log(player.GetDisplayName());
            //     Debug.Log(player.displayName);
            // }

        }
    }

    private void SearchForThisPlayer()
    {
        var players = FindObjectsOfType<PlayerController>();
        int gameobjIndex = 0;

        foreach (var i in players)
        {
            if (i.gameObject == this.gameObject)
            {
                //Debug.Log("This gameobj is index " + gameobjIndex);
                break;
            }
            gameobjIndex++;
        }

        int networkIndex = 0;
        var networkPlayers = FindObjectsOfType<NetworkGamePlayerLobby>();
        foreach (var i in networkPlayers)
        {
            //Debug.Log(i.GetComponent<NetworkGamePlayerLobby>().displayName + " is index " + networkIndex);
            if (gameobjIndex == networkIndex)
            {
                playerName = i.GetComponent<NetworkGamePlayerLobby>().displayName;
                index = networkIndex;
                break;
            }
            networkIndex++;
        }
        Debug.Log(playerName);
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
            SetAniBoolCMD("isIdle", true);
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

        SetAniBoolCMD("isIdle", isIdle);
        SetAniBoolCMD("isSprint", isSprint);
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

    public void ClientDisconnect()
    {
        Room.StopClient();
        SceneManager.LoadScene("Main Menu");
    }

    public void HostDisconnect()
    {
        Room.StopHost();
        SceneManager.LoadScene("Main Menu");
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

    [Command]
    void SetAniBoolCMD(String name, bool value)
    {
        SetAniBool(name, value);
    }
    [ClientRpc]
    void SetAniBool(String name, bool value)
    {
        playerAnimator.SetBool(name, value);
    }
}
