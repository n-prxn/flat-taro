using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : NetworkBehaviour
{

    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerStatus playerStatus;
    [Header("UI")]
    [SerializeField] private Image pulseImg;
    [SerializeField] private Image urgeBar;
    [SerializeField] private TextMeshProUGUI sunflowerText;
    [SerializeField] private Image timerArm;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Image itemSlot;
    [SerializeField] private TextMeshProUGUI deatchCountText;

    [Header("Reward")]
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private TextMeshProUGUI bestCupWinnerText;
    [SerializeField] private TextMeshProUGUI heavenVIPWinnerText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isGameFinished)
        {
            sunflowerText.text = "x" + playerStatus.sunflower.ToString();
            timeText.text = GameManager.instance.timeCount.ToString("0");
            UpdateTime();
            UpdateUrge();
            UpdatePulse();
            UpdateHeldItem();
            UpdateDeathCount();
        }
        else
        {
            if (!rewardPanel.activeSelf)
            {
                rewardPanel.SetActive(true);
                RewardSummary();
            }
        }
    }

    void RewardSummary()
    {
        //Least Death Count
        string playerMinDeath = "";
        int min = 999;

        //Most Death Count
        string playerMaxDeath = "";
        int max = 0;

        var networkPlayers = FindObjectsOfType<NetworkGamePlayerLobby>();
        foreach (var i in networkPlayers)
        {
            if (i.GetComponent<NetworkGamePlayerLobby>().GetDeathCount() >= max)
            {
                max = i.GetComponent<NetworkGamePlayerLobby>().GetDeathCount();
                playerMaxDeath = i.GetComponent<NetworkGamePlayerLobby>().displayName;
            }

            if (i.GetComponent<NetworkGamePlayerLobby>().GetDeathCount() <= min)
            {
                max = i.GetComponent<NetworkGamePlayerLobby>().GetDeathCount();
                playerMinDeath = i.GetComponent<NetworkGamePlayerLobby>().displayName;
            }
        }

        bestCupWinnerText.text = playerMinDeath;
        heavenVIPWinnerText.text = playerMaxDeath;

        //Debug.Log("Best Hamster : " + playerMinDeath);
        //Debug.Log("Heaven VIP : " + playerMaxDeath);
    }

    void UpdateTime()
    {
        timerArm.transform.localRotation = Quaternion.Euler(0, 0, 90f - GameManager.instance.timeCount);
    }

    void UpdateUrge()
    {
        urgeBar.fillAmount = (float)playerStatus.urge / 100f;
    }

    void UpdatePulse()
    {
        Animator animator = pulseImg.GetComponent<Animator>();
        animator.speed = 1 + ((float)playerStatus.pulse - 300f) / 75f;

        /*float heartScale = ((float)playerStatus.pulse) / 600f;
        pulseImg.transform.localScale = new Vector3(heartScale,heartScale,heartScale);*/
    }

    void UpdateDeathCount()
    {
        deatchCountText.text = playerStatus.deathCount.ToString();
    }

    void UpdateHeldItem()
    {
        if (playerStatus.heldItem == null)
            itemSlot.gameObject.SetActive(false);
        else
        {
            itemSlot.gameObject.SetActive(true);
            itemSlot.transform.GetChild(0).GetComponent<Image>().sprite = playerStatus.heldItem.itemImage;
        }
    }

    [ClientRpc]
    void GetNowTime()
    {
        timeText.text = "Time : " + GameManager.instance.timeCount.ToString("0.00");
    }
}
