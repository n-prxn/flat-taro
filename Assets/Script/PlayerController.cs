using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    private Vector2 moveDirection;

    // Start is called before the first frame update
    void Start()
    {

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

        moveDirection = new Vector2(moveX, moveY);
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }
}
