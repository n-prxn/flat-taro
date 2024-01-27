using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlaySFX : MonoBehaviour
{
    [SerializeField] private AudioSource SFXPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(){
        SFXPlayer.Play();
    }

    public void PlayThisSound(AudioClip audioClip){
        SFXPlayer.clip = audioClip;
        SFXPlayer.Play();
    }
}
