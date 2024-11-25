using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TeamController : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<PhotonTeam> _roomTeams;
    //[SerializeField] private int _teamSize;
    [SerializeField] private int _pacmanTeamSize;
    [SerializeField] private int _ghostTeamSize;

    public static Action<List<PhotonTeam>> OnCreateTeams = delegate { };
    public static Action<Player> OnRemovePlayer = delegate { };
    public static Action OnClearTeams = delegate { };

    public static Action OnRoomFull = delegate { };
    public static Action NewCommer = delegate { };


    private void Awake()
    {
        MultiplayerController.RoomJoined += HandleCreateTeams;
        MultiplayerController.RoomLeft += HandleLeaveRoom;
        MultiplayerController.OnOtherPlayerLeftRoom += HandleOtherPlayerLeftRoom;

        _roomTeams = new List<PhotonTeam>();
    }

    private void OnDestroy()
    {
        MultiplayerController.RoomJoined -= HandleCreateTeams;
        MultiplayerController.RoomLeft -= HandleLeaveRoom;
        MultiplayerController.OnOtherPlayerLeftRoom -= HandleOtherPlayerLeftRoom;
    }

    private void HandleCreateTeams(string roomName)
    {

        CreateTeams();

        OnCreateTeams?.Invoke(_roomTeams);
        NewCommer?.Invoke();

        //AutoAssignPlayerToTeam(PhotonNetwork.LocalPlayer);

    }

    private void HandleLeaveRoom()
    {
        PhotonNetwork.LocalPlayer.LeaveCurrentTeam();
        _roomTeams.Clear();
        _ghostTeamSize = 0;
        _pacmanTeamSize = 0;
        OnClearTeams?.Invoke();
    }

    private void HandleOtherPlayerLeftRoom(Player otherPlayer)
    {
        OnRemovePlayer?.Invoke(otherPlayer);
        NewCommer?.Invoke();
    }

    private void CreateTeams()
    {
        _ghostTeamSize = 2;
        _pacmanTeamSize = 4;

        _roomTeams.Add(new PhotonTeam
        {
            Name = "Ghost",
            Code = 1
        });

        _roomTeams.Add(new PhotonTeam
        {
            Name = "Widemouth",
            Code = 2
        });
    }

    /*    private void AutoAssignPlayerToTeam(Player player)
        {
            foreach (PhotonTeam team in _roomTeams)
            {
                int teamPlayerCount = PhotonTeamsManager.Instance.GetTeamMembersCount(team.Code);

                if (teamPlayerCount < _ghostTeamSize)
                {
                    Debug.Log($"Auto assigned {player.NickName} to {team.Name}");
                    if (player.GetPhotonTeam() == null)
                    {
                        player.JoinTeam(1);
                    }

                    break;
                }
                else if (teamPlayerCount < _pacmanTeamSize)
                {
                    Debug.Log($"Auto assigned {player.NickName} to {team.Name}");
                    if (player.GetPhotonTeam() == null)
                    {
                        player.JoinTeam(2);
                    }
                }
            }
        }*/

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        NewCommer?.Invoke();
    }
}
