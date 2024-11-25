using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class MultiplayerController : MonoBehaviourPunCallbacks
{

    public TMP_InputField joinRoomInput;
    public TMP_InputField createRoomInput;
    public string levelToPlay;

    public GameObject CurrentPlayer;

    public static Action PhotonJoined = delegate { };
    public static Action MasterJoined = delegate { };
    public static Action LobbyJoined = delegate { };
    public static Action<string> CreateRoomFailed = delegate { };
    public static Action<string> RoomJoined = delegate { };
    public static Action<string> RoomJoinedFailed = delegate { };
    public static Action CreateRandomRoom = delegate { };


    public static Action<bool> OnRoomStatusChange = delegate { };
    public static Action RoomLeft = delegate { };

    public static Action<Player> OnOtherPlayerLeftRoom = delegate { };
    public static Action<Player> OnMasterOfRoom = delegate { };
    public static Action<float> OnCountingDown = delegate { };
    public static Action<int> GetCurrentPlayerTeam = delegate { };

    //[SerializeField] private GameMode _selectedGameMode;
    private bool _startGame;
    private float _currentCountDown;
    private const string GAME_MODE = "GAMEMODE";
    private const string START_GAME = "STARTGAME";
    private const float GAME_COUNT_DOWN = 10f;


    private void Awake()
    {
        PhotonJoined?.Invoke();
        CurrentPlayer = GameObject.FindGameObjectWithTag("CurrentPlayer");

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            PhotonNetwork.Disconnect();
            PhotonNetwork.ConnectUsingSettings();
        }

        UIMultiplayer.OnCreateRoom += HandleCreateRoom;
        UIMultiplayer.OnLeaveRoom += HandleLeaveRoom;
        UIMultiplayer.OnJoinRoom += HandleJoinRoom;
        UIMultiplayer.OnMatchMaking += HandleMatchMaking;
        UIMultiplayer.OnStartGame += HandleStartGame;
        UIPlayerSelection.OnKickPlayer += HandleKickPlayer;

    }

    private void OnDestroy()
    {
        UIMultiplayer.OnCreateRoom -= HandleCreateRoom;
        UIMultiplayer.OnLeaveRoom -= HandleLeaveRoom;
        UIMultiplayer.OnJoinRoom -= HandleJoinRoom;
        UIMultiplayer.OnMatchMaking -= HandleMatchMaking;
        UIMultiplayer.OnStartGame -= HandleStartGame;
        UIPlayerSelection.OnKickPlayer += HandleKickPlayer;
    }

    private void Update()
    {
        if (!_startGame) return;

        if (_currentCountDown > 0)
        {
            OnCountingDown?.Invoke(_currentCountDown);
            _currentCountDown -= Time.deltaTime;
        }
        else
        {
            _startGame = false;

            Debug.Log("Loading level!");
            //PhotonNetwork.LoadLevel(_gameSceneIndex);
        }
    }

    #region Handle Methods
    private void HandleCreateRoom()
    {
        Debug.Log(createRoomInput.text);
        if (!string.IsNullOrEmpty(createRoomInput.text))
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 6;

            PhotonNetwork.CreateRoom(createRoomInput.text.ToUpper(), roomOptions);
        }
        else
        {
            Debug.Log("Room name is empty");
        }
    }

    private void HandleLeaveRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            RoomLeft?.Invoke();
            PhotonNetwork.LeaveRoom();
        }
    }

    private void HandleGetRoomStatus()
    {
        OnRoomStatusChange?.Invoke(PhotonNetwork.InRoom);
    }

    private void HandleStartGame()
    {
        PhotonNetwork.LoadLevel(levelToPlay);
    }

    private void HandleJoinRoom()
    {
        PhotonNetwork.JoinRoom(joinRoomInput.text.ToUpper());
    }

    private void HandleMatchMaking()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    private void HandleKickPlayer(Player kickedPlayer)
    {
        if (PhotonNetwork.LocalPlayer.Equals(kickedPlayer))
        {
            HandleLeaveRoom();
        }
    }

    #endregion
    private void DebugPlayerList()
    {
        string players = "";
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            players += $"{player.Value.NickName}, ";
        }
        Debug.Log($"Current Room Players: {players}");
    }

    private void AutoStartGame()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 6)
            HandleStartGame();
    }

    #region Photon Calls Back 
    public override void OnConnectedToMaster()
    {
        MasterJoined?.Invoke();
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        LobbyJoined?.Invoke();

        string currentPlayerUsername = CurrentPlayer.GetComponent<CurrentPlayer>().username;
        PhotonNetwork.NickName = currentPlayerUsername;
        Debug.Log($"Made it to the lobby as {currentPlayerUsername}");

    }

    public override void OnCreatedRoom()
    {
        OnMasterOfRoom?.Invoke(PhotonNetwork.LocalPlayer);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        CreateRoomFailed?.Invoke(message);

    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"You have joined the Photon room {PhotonNetwork.CurrentRoom.Name}");
        DebugPlayerList();

        RoomJoined?.Invoke(PhotonNetwork.CurrentRoom.Name);
        OnRoomStatusChange?.Invoke(PhotonNetwork.InRoom);
    }



    public override void OnLeftRoom()
    {
        OnRoomStatusChange?.Invoke(PhotonNetwork.InRoom);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        RoomJoinedFailed?.Invoke(message);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        createRandomRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Another player has joined the room {newPlayer.NickName}");
        DebugPlayerList();
        AutoStartGame();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"Player has left the room {otherPlayer.NickName}");
        OnOtherPlayerLeftRoom?.Invoke(otherPlayer);
        DebugPlayerList();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log($"New Master Client is {newMasterClient.NickName}");
        OnMasterOfRoom?.Invoke(newMasterClient);
    }

    public void createRandomRoom()
    {
        CreateRandomRoom?.Invoke();

        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 6,
        };

        PhotonNetwork.CreateRoom(PhotonNetwork.NickName, roomOptions);

    }
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        object startGameObject;
        if (propertiesThatChanged.TryGetValue(START_GAME, out startGameObject))
        {
            _startGame = (bool)startGameObject;
            if (_startGame)
            {
                _currentCountDown = GAME_COUNT_DOWN;
            }
            if (_startGame && PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsVisible = false;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }

    #endregion

}
