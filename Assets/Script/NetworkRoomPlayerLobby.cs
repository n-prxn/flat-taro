using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkRoomPlayerLobby : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject lobbyUI;
    [SerializeField] private TMP_Text addressText;
    [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];
    [SerializeField] private Image[] playerReadyButtons = new Image[4];
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button readyButton;
    [SerializeField] private Sprite[] readyButtonImages;
    [SerializeField] private Sprite[] readySignImages;

    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string DisplayName = "Loading...";

    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool IsReady = false;
    private bool isLeader;
    public bool IsLeader
    {
        set
        {
            isLeader = value;
            startGameButton.gameObject.SetActive(value);
        }
    }

    private NetworkManagerLobby room;
    private NetworkManagerLobby Room
    {
        get
        {
            if (room != null)
                return room;
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    public override void OnStartAuthority()
    {
        CmdSetDisplayName(PlayerNameInput.DisplayName);
        lobbyUI.SetActive(true);
    }

    public override void OnStartClient()
    {
        Room.RoomPlayers.Add(this);
        UpdateDisplay();
    }

    public override void OnStopClient()
    {
        Room.RoomPlayers.Remove(this);
        UpdateDisplay();
    }

    private void HandleDisplayNameChanged(string oldValue, string newValue)
    {
        UpdateDisplay();
    }

    private void HandleReadyStatusChanged(bool oldValue, bool newValue)
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (!isOwned)
        {
            foreach (var player in Room.RoomPlayers)
            {
                if (player.isOwned)
                {
                    player.UpdateDisplay();
                    break;
                }
            }
            return;
        }

        addressText.text = "Address : " + room.networkAddress;

        for (int i = 0; i < playerNameTexts.Length; i++)
        {
            playerNameTexts[i].text = "Waiting for Player...";
            playerReadyButtons[i].sprite = readySignImages[1];
        }

        for (int i = 0; i < Room.RoomPlayers.Count; i++)
        {
            playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
            playerReadyButtons[i].sprite = Room.RoomPlayers[i].IsReady ?
                readySignImages[0]:
                readySignImages[1];
        }
    }

    public void HandleReadyToStart(bool readyToStart)
    {
        if (!isLeader)
        {
            startGameButton.gameObject.SetActive(false);
            return;
        }

        startGameButton.interactable = readyToStart;
    }

    [Command]
    private void CmdSetDisplayName(string displayName)
    {
        DisplayName = displayName;
    }

    [Command]
    public void CmdReadyUp()
    {
        IsReady = !IsReady;
        if (IsReady)
            readyButton.GetComponent<Image>().sprite = readyButtonImages[0];
        else
            readyButton.GetComponent<Image>().sprite = readyButtonImages[1];
        Room.NotifyPlayersOfReadyState();
    }

    [Command]
    public void CmdStartGame()
    {
        if (Room.RoomPlayers[0].connectionToClient != connectionToClient)
            return;

        Room.StartGame();
    }

}
