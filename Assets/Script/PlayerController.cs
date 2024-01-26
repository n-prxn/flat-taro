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
    private Vector2 moveDirection;
    private bool isSprint = false;
    public bool IsSprint { get => isSprint; set => isSprint = value; }
    Camera cam;

    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!this.isLocalPlayer)
            cam.gameObject.SetActive(false);

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
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed * (isSprint ? 1.5f : 1f), moveDirection.y * moveSpeed * (isSprint ? 1.5f : 1f));
    }
}
