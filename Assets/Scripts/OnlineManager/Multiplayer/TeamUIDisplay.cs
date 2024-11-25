using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TeamUIDisplay : MonoBehaviour
{

    [SerializeField] private TeamUI _ghostUIPrefab;
    [SerializeField] private TeamUI _pacmanUIPrefab;
    [SerializeField] private Transform _teamContainer;
    [SerializeField] private List<TeamUI> _uiTeams;

    public static Action<Player, PhotonTeam> OnAddPlayerToTeam = delegate { };
    public static Action<Player> OnRemovePlayerFromTeam = delegate { };


    private void Awake()
    {
        TeamController.OnCreateTeams += HandleCreateTeams;
        TeamController.OnRemovePlayer += HandleRemovePlayer;
        TeamController.OnClearTeams += HandleClearTeams;
        _uiTeams = new List<TeamUI>();
    }

    private void OnDestroy()
    {
        TeamController.OnCreateTeams -= HandleCreateTeams;
        TeamController.OnRemovePlayer -= HandleRemovePlayer;
        TeamController.OnClearTeams -= HandleClearTeams;
    }

    private void HandleCreateTeams(List<PhotonTeam> teams)
    {

        Player[] player = PhotonNetwork.PlayerList;


        TeamUI uiTeam = Instantiate(_ghostUIPrefab, _teamContainer);
        uiTeam.Initialize(teams[0], 2);
        _uiTeams.Add(uiTeam);

        TeamUI uiTeam2 = Instantiate(_ghostUIPrefab, _teamContainer);
        uiTeam2.Initialize(teams[1], 4);
        _uiTeams.Add(uiTeam2);



    }

    private void HandleRemovePlayer(Player otherPlayer)
    {
        OnRemovePlayerFromTeam?.Invoke(otherPlayer);
    }

    private void HandleClearTeams()
    {
        foreach (TeamUI uiTeam in _uiTeams)
        {
            Destroy(uiTeam.gameObject);
        }
        _uiTeams.Clear();
    }

}
