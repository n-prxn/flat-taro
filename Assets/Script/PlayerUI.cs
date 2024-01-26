using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private TextMeshProUGUI pulseTxt;
    [SerializeField] private TextMeshProUGUI urgeTxt;
    [SerializeField] private TextMeshProUGUI sunflowerText;
    [SerializeField] private TextMeshProUGUI timeText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pulseTxt.text = "Pulse : " + playerStatus.pulse.ToString();
        urgeTxt.text = "Urge : " + playerStatus.urge.ToString();
        sunflowerText.text = "Sunflower Seed : " + playerStatus.sunflower.ToString();
        timeText.text = "Time : " + GameManager.instance.GetTimeNow().ToString("0.00");
    }
}
