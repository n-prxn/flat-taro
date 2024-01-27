using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

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
    Camera cam;
    [SerializeField] GameObject GUIobj;
    [SerializeField] Animator animator;

    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!this.isLocalPlayer)
        {
            cam.gameObject.SetActive(false);
            GUIobj.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (this.isLocalPlayer)
        {
            ProcessInput();
        }
    }

    private void FixedUpdate()
    {
        if (this.isLocalPlayer)
        {
            Move();
        }
        //Physics Calculations
    }

    void ProcessInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
            isSprint = true;
        else
            isSprint = false;

        moveDirection = new Vector2(moveX, moveY);
        Flip(moveX);

        if (moveDirection.magnitude == 0)
            isIdle = true;
        else
            isIdle = false;

        animator.SetBool("isIdle",isIdle);
        animator.SetBool("isSprint",isSprint);
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed * (isSprint ? 1.5f : 1f), moveDirection.y * moveSpeed * (isSprint ? 1.5f : 1f));
    }

    void Flip(float moveX)
    {
        if (moveX < 0)
        {
            hamsterTF.rotation = Quaternion.Euler(0, 180, 0);
            faceDirection = -1;
        }
        else if (moveX > 0)
        {
            hamsterTF.rotation = Quaternion.Euler(0, 0, 0);
            faceDirection = 1;
        }
    }

}
