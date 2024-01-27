using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private Image pulseImg;
    [SerializeField] private Image urgeBar;
    [SerializeField] private TextMeshProUGUI sunflowerText;
    [SerializeField] private Image timerArm;
    [SerializeField] private TextMeshProUGUI timeText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        sunflowerText.text = "x" + playerStatus.sunflower.ToString();
        timeText.text = GameManager.instance.timeCount.ToString("0");
        UpdateTime();
        UpdateUrge();
        UpdatePulse();
    }

    void UpdateTime(){
        timerArm.transform.localRotation =  Quaternion.Euler(0,0,90f - GameManager.instance.timeCount);
    }

    void UpdateUrge(){
        urgeBar.fillAmount = (float)playerStatus.urge / 100f;
    }

    void UpdatePulse(){
        Animator animator = pulseImg.GetComponent<Animator>();
        animator.speed = 1 + ((float)playerStatus.pulse - 300f) / 75f;
    }

    [ClientRpc]
    void GetNowTime()
    {
        timeText.text = "Time : " + GameManager.instance.timeCount.ToString("0.00");
    }
}
