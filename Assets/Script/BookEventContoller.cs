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
    [SerializeField][SyncVar] bool isDrop;

    // Start is called before the first frame update
    void Start()
    {
        // if (isServer)
        // {
        //     nowPos = new Vector2(basePos.transform.position.x, basePos.transform.position.y);
        //     StartCoroutine(GoToHomeSoft());
        // }
    }

    // Update is called once per frame
    void Update()
    {
        if (isServer && Input.GetKeyDown(KeyCode.M))
        {
            nowPos = new Vector2(basePos.transform.position.x, basePos.transform.position.y);
            StartCoroutine(GoToHomeSoft());
        }
    }

    public void DropEvent(float percent)
    {
        basePos.transform.position = Vector2.Lerp(nowPos, tagetPos.transform.position, Mathf.SmoothStep(0, 1, percent));
    }


    IEnumerator GoToHomeSoft()
    {
        while (elapsedTime <= targetTime)
        {
            elapsedTime += Time.deltaTime;
            float percentComplete = elapsedTime / targetTime;
            DropEvent(percentComplete);
            // Debug.Log(Vector2.Distance(basePos.transform.position, tagetPos.transform.position));
            yield return null;
        }
        isDrop = true;
        NetworkServer.Destroy(this.gameObject);
    }
}
