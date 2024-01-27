using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerLobby networkManager;

    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel;

    public void HostLobby(){
        networkManager.StartHost();
        landingPagePanel.SetActive(false);
    }

    public void Play(){
        SceneManager.LoadScene("Gameplay");
    }

    public void Quit(){
        Application.Quit();
    }
}
