using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneChange : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                CallLaodSceneCMD();
            }
        }
    }

    [Command]
    void CallLaodSceneCMD()
    {
        CallLaodScene();
    }

    [ClientRpc]
    void CallLaodScene()
    {
        SceneManager.LoadScene(1);
    }
}
