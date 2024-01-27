using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Examples.Billiards;
using UnityEngine;

public class VacuumEventContoller : NetworkBehaviour
{
    Rigidbody2D rb;
    [SerializeField] bool hasTarget;
    Vector3 targetPos;
    [SerializeField] float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [Client]
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.canPlayerMove = false;
            SetTarget(other.gameObject);
        }
    }

    [ClientCallback]
    private void FixedUpdate()
    {
        if (hasTarget)
        {
            Vector2 targetDirection = (transform.position - targetPos).normalized;
            rb.velocity = new Vector2(targetDirection.x, targetDirection.y) * moveSpeed;
        }
    }

    void SetTarget(GameObject obj)
    {
        rb = obj.GetComponent<Rigidbody2D>();
        targetPos = obj.transform.position;
        hasTarget = true;
    }
}