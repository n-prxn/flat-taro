using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : NetworkBehaviour
{
    public GameObject cam;
    public GameObject GUIobj;

    public override void OnStartAuthority()

    {
        cam.SetActive(true);
        GUIobj.SetActive(true);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Gameplay")
        {
            cam.transform.position = transform.position;
        }
    }
}
