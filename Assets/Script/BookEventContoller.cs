using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class BookEventContoller : NetworkBehaviour
{
    [SerializeField] GameObject basePos;
    [SerializeField] Vector2 nowPos;
    [SerializeField] GameObject tagetPos;
    [SerializeField] float elapsedTime;
    [SerializeField] float targetTime;
    [SerializeField][SyncVar] public bool isDrop;
    [SerializeField] GameObject targetCollider;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void StartDropBook()
    {
        if (isServer)
        {
            nowPos = new Vector2(basePos.transform.position.x, basePos.transform.position.y);
            StartCoroutine(DropEvent());
        }
    }

    public void DropUpdate(float percent)
    {
        basePos.transform.position = Vector2.Lerp(nowPos, tagetPos.transform.position, Mathf.SmoothStep(0, 1, percent));
    }


    IEnumerator DropEvent()
    {
        while (elapsedTime <= targetTime)
        {
            elapsedTime += Time.deltaTime;
            float percentComplete = elapsedTime / targetTime;
            DropUpdate(percentComplete);
            // Debug.Log(Vector2.Distance(basePos.transform.position, tagetPos.transform.position));
            yield return null;
        }
        isDrop = true;
        SetBookActive(true);
        StartDestroy();
    }

    [ClientRpc]
    void SetBookActive(bool value)
    {
        targetCollider.SetActive(value);
    }

    public void StartDestroy()
    {
        StartCoroutine("SetDestroy");
    }

    IEnumerator SetDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        NetworkServer.Destroy(this.gameObject);
    }
}
