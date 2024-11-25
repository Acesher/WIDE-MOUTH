using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using TMPro;
using UnityEngine;

public class UIMultiplayer : MonoBehaviour
{

    public GameObject lobbyScreen;

    public GameObject roomScreen;
    public TMP_Text roomNameText;//, playerNameLabel;
    //private List<TMP_Text> allPLayerNames = new List<TMP_Text>();

    public GameObject loadingScreen;
    public TMP_Text loadingText;

    public GameObject matchMakingScreen;
    public TMP_Text matchMakingText;

    public GameObject errorScreen;

    public TMP_Text errorText;

    public GameObject _startButton;
    public GameObject _exitButton;

    public static Action OnCreateRoom = delegate { };
    public static Action OnLeaveRoom = delegate { };
    public static Action OnJoinRoom = delegate { };
    public static Action OnMatchMaking = delegate { };
    public static Action OnStartGame = delegate { };

    private void Awake()
    {
        MultiplayerController.PhotonJoined += HandlePhotonJoined;
        MultiplayerController.MasterJoined += HandleConnectedMaster;
        MultiplayerController.LobbyJoined += HandleLobbyJoined;
        MultiplayerController.CreateRoomFailed += HandleCreateRoomFailed;
        MultiplayerController.RoomJoined += HandleRoomJoined;
        MultiplayerController.RoomLeft += HandleRoomLeft;
        MultiplayerController.RoomJoinedFailed += HandleRoomJoinFailed;
        MultiplayerController.CreateRandomRoom += HandleCreateRandomRoom;
        MultiplayerController.OnMasterOfRoom += HandleMasterOfRoom;
        MultiplayerController.OnCountingDown += HandleCountingDown;
        TeamController.OnRoomFull += HandleRoomFull;
    }

    private void OnDestroy()
    {
        MultiplayerController.PhotonJoined -= HandlePhotonJoined;
        MultiplayerController.MasterJoined -= HandleConnectedMaster;
        MultiplayerController.LobbyJoined -= HandleLobbyJoined;
        MultiplayerController.CreateRoomFailed -= HandleCreateRoomFailed;
        MultiplayerController.RoomJoined -= HandleRoomJoined;
        MultiplayerController.RoomLeft -= HandleRoomLeft;
        MultiplayerController.RoomJoinedFailed -= HandleRoomJoinFailed;
        MultiplayerController.CreateRandomRoom -= HandleCreateRandomRoom;
        MultiplayerController.OnMasterOfRoom -= HandleMasterOfRoom;
        MultiplayerController.OnCountingDown -= HandleCountingDown;
    }

    private void Update()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient == true && PhotonNetwork.CurrentRoom.PlayerCount == 3)
        {
            _startButton.SetActive(true);
        }
        else
        {
            _startButton.SetActive(false);
        }
    }


    #region Handle Action From Controller

    private void HandlePhotonJoined()
    {
        closedMenus();
        loadingScreen.SetActive(true);
        loadingText.text = "Connecting to server...";
    }

    private void HandleConnectedMaster()
    {
        loadingText.text = "Joining Lobby...";
    }

    private void HandleLobbyJoined()
    {
        closedMenus();
        lobbyScreen.SetActive(true);
    }

    private void HandleCreateRoomFailed(string message)
    {
        errorText.text = "Failed to create room: " + message;
        closedMenus();
        errorScreen.SetActive(true);
    }

    private void HandleRoomJoined(string roomName)
    {
        closedMenus();
        roomScreen.SetActive(true);

        roomNameText.text = roomName;

    }

    private void HandleRoomLeft()
    {
        closedMenus();
        lobbyScreen.SetActive(true);
    }

    private void HandleRoomJoinFailed(string message)
    {
        errorText.text = "Failed to join room: " + message;
        closedMenus();
        errorScreen.SetActive(true);
    }

    private void HandleRoomFull()
    {
        errorText.text = "Room is full";
        closedMenus();
        errorScreen.SetActive(true);
    }

    private void HandleCreateRandomRoom()
    {
        closedMenus();
        loadingText.text = "Creating room...";
        loadingScreen.SetActive(true);
    }

    private void HandleMasterOfRoom(Player masterPlayer)
    {
        if (PhotonNetwork.LocalPlayer.Equals(masterPlayer))
        {
            _startButton.SetActive(true);
        }
        else
        {
            _startButton.SetActive(false);
        }
    }
    private void HandleCountingDown(float count)
    {
        _startButton.SetActive(false);
        _exitButton.SetActive(false);
        roomNameText.SetText(count.ToString("F0"));
    }

    #endregion



    #region self-Claimed Method
    private void closedMenus()
    {
        lobbyScreen.SetActive(false);
        roomScreen.SetActive(false);
        loadingScreen.SetActive(false);
        errorScreen.SetActive(false);
        matchMakingScreen.SetActive(false);
    }
    public void createRoom()
    {
        OnCreateRoom?.Invoke();

        closedMenus();
        loadingText.text = "Creating room...";
        loadingScreen.SetActive(true);

    }

    public void leaveRoom()
    {
        OnLeaveRoom?.Invoke();
        closedMenus();
        loadingText.text = "Leaving room...";
        loadingScreen.SetActive(true);
    }

    public void closeErrorScreen()
    {
        closedMenus();
        lobbyScreen.SetActive(true);
    }
    public void joinRoom()
    {
        OnJoinRoom?.Invoke();
        closedMenus();
        loadingText.text = "Joining Room...";
        loadingScreen.SetActive(true);
    }

    public void matchMaking()
    {
        OnMatchMaking?.Invoke();
        closedMenus();
        matchMakingText.text = "Finding Room...";
        matchMakingScreen.SetActive(true);
        Debug.Log("Searching for a room");
    }

    public void StartGame()
    {
        Debug.Log($"Starting game...");
        OnStartGame?.Invoke();
    }

    #endregion
}
