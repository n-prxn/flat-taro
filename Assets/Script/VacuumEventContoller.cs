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
    [SerializeField] float magnetSpeed;
    [SerializeField] Vector3 vectorX;

    private void Awake()
    {
        if (transform.position.x >= 0)
        {
            vectorX = Vector3.left;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            vectorX = Vector3.right;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (vectorX * Time.deltaTime) * moveSpeed;
        if (!(-90 < transform.position.x && transform.position.x < 90))
        {
            NetworkServer.Destroy(this.gameObject);
        }

    }

    [Client]
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetTarget(other.gameObject);
        }
    }

    [Client]
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ResetTarget();
        }
    }

    [ClientCallback]
    private void FixedUpdate()
    {
        if (hasTarget)
        {
            Vector2 targetDirection = (transform.position - targetPos).normalized;
            rb.velocity = new Vector2(targetDirection.x, targetDirection.y) * magnetSpeed;
        }
    }

    void SetTarget(GameObject obj)
    {
        rb = obj.GetComponent<Rigidbody2D>();
        targetPos = obj.transform.position;
        hasTarget = true;
    }

    public void ResetTarget()
    {
        rb.velocity = Vector2.zero;
        hasTarget = false;
    }

}
