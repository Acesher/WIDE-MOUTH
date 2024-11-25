using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TeamUI : MonoBehaviour
{

    [SerializeField] private int _pacmanTeamSize;
    [SerializeField] private int _ghostTeamSize;
    [SerializeField] private int _maxTeamSize;

    [SerializeField] private Transform _pacmanSelectionContainer;
    [SerializeField] private Transform _ghostSelectionContainer;
    [SerializeField] private UIPlayerSelection _pacmanSelectionPrefab;
    [SerializeField] private UIPlayerSelection _ghostSelectionPrefab;
    private List<UIPlayerSelection> playerList = new List<UIPlayerSelection>();

    private void Awake()
    {
        //TeamUIDisplay.OnAddPlayerToTeam += HandleAddPlayerToTeam;
        PhotonTeamsManager.PlayerJoinedTeam += OnPlayerJoinedTeam;

        TeamController.NewCommer += HandleNewCommer;
        MultiplayerController.RoomLeft += HandleLeaveRoom;

    }

    private void Update()
    {
        if (PhotonNetwork.LocalPlayer.GetPhotonTeam() != null)
        {
            Debug.Log(PhotonNetwork.LocalPlayer.GetPhotonTeam().Name);
        }
    }

    private void OnPlayerJoinedTeam(Player player, PhotonTeam team)
    {
        Debug.Log($"{player.NickName} joined {team.Name} team!");
        print(PhotonTeamsManager.Instance.GetTeamMembersCount(1));

        Player[] teamMembers;

        if (PhotonTeamsManager.Instance.TryGetTeamMembers(1, out teamMembers))
        {
            Debug.Log(teamMembers.Length);
        }

    }

    private void OnDestroy()
    {
        //TeamUIDisplay.OnAddPlayerToTeam -= HandleAddPlayerToTeam;
        PhotonTeamsManager.PlayerJoinedTeam -= OnPlayerJoinedTeam;

        TeamController.NewCommer -= HandleNewCommer;
        MultiplayerController.RoomLeft -= HandleLeaveRoom;
    }

    public void Initialize(PhotonTeam team, int teamSize)
    {

        _maxTeamSize = teamSize;


        if (team.Code == 1)
        {
            Player[] teamMembers;

            if (PhotonTeamsManager.Instance.TryGetTeamMembers(1, out teamMembers))
            {
                //Debug.Log(PhotonNetwork.PlayerList.Length);

                for (int i = 0; i < 2; i++)
                {
                    AddPlayerToTeamGhost(PhotonNetwork.PlayerList[i]);
                    PhotonNetwork.PlayerList[i].JoinTeam(1);
                    //HandleNewCommer();
                    if (PhotonNetwork.PlayerList.Length == 1)
                    {
                        break;
                    }
                }
            }

        }
        else if (team.Code == 2)
        {
            Player[] teamMembers;
            if (PhotonTeamsManager.Instance.TryGetTeamMembers(2, out teamMembers))
            {

                for (int i = 2; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    AddPlayerToTeamPacman(PhotonNetwork.PlayerList[i]);
                    PhotonNetwork.PlayerList[i].JoinTeam(2);
                    //HandleNewCommer();
                }
            }

        }
    }

    /*    public void HandleAddPlayerToTeam(Player player, PhotonTeam team)
        {
            if (player.GetPhotonTeam() == team.Code)
            {
                Debug.Log($"Updating {_team.Name} UI to add {player.NickName}");
                AddPlayerToTeam(player);
            }
        }*/

    public void HandleNewCommer()
    {
        //DontDestroyOnLoad(gameObject);
        foreach (UIPlayerSelection player in playerList)
        {
            //Debug.Log($"IN ROOM: {player.gameObject.transform.GetChild(1).GetChild(0)}");
            Debug.Log($"IN ROOM: {player.gameObject.name}");
            Destroy(player.gameObject);

        }
        playerList.Clear();

        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Length; i++)
        {
            UIPlayerSelection newuiPlayerSelection = Instantiate(_ghostSelectionPrefab, _ghostSelectionContainer);
            newuiPlayerSelection.Initialize(players[i]);
            playerList.Add(newuiPlayerSelection);
            /*            if (i == 0)
                        {
                            UIPlayerSelection newuiPlayerSelection = Instantiate(_ghostSelectionPrefab, _ghostSelectionContainer);
                            newuiPlayerSelection.Initialize(players[i]);
                            playerList.Add(newuiPlayerSelection);
                        }
                        else if (i == 1)
                        {
                            UIPlayerSelection newuiPlayerSelection = Instantiate(_ghostSelectionPrefab, _ghostSelectionContainer);
                            newuiPlayerSelection.Initialize(players[i]);
                            playerList.Add(newuiPlayerSelection);
                        }
                        else
                        {
                            UIPlayerSelection newuiPlayerSelection = Instantiate(_pacmanSelectionPrefab, _pacmanSelectionContainer);
                            newuiPlayerSelection.Initialize(players[i]);
                            playerList.Add(newuiPlayerSelection);
                        }*/
        }
    }

    private void AddPlayerToTeamGhost(Player player)
    {
        //UIPlayerSelection uiPlayerSelection = Instantiate(_ghostSelectionPrefab, _ghostSelectionContainer);
        //uiPlayerSelection.Initialize(player);
        //UpdateTeamUI();
        //playerList.Add(uiPlayerSelection);
        HandleNewCommer();
    }

    private void AddPlayerToTeamPacman(Player player)
    {
        //UIPlayerSelection uiPlayerSelection = Instantiate(_pacmanSelectionPrefab, _pacmanSelectionContainer);
        //uiPlayerSelection.Initialize(player);
        //UpdateTeamUI();
        //playerList.Add(uiPlayerSelection);
        HandleNewCommer();
    }

    private void HandleLeaveRoom()
    {
        Debug.Log($"LEAVE ROOM: {gameObject.transform.GetChild(1).GetChild(0)}");
        Destroy(gameObject.transform.GetChild(1).GetChild(0));
    }

}
