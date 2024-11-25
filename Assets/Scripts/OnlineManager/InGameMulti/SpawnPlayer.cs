using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System;

public class SpawnPlayer : MonoBehaviour
{

    public static SpawnPlayer instance;
    public PhotonTeam team;

    public GameObject pacmanPlayer;
    public GameObject ghostPlayer;

    public float pacmanMinX, pacmanMinY, pacmanMaxX, pacmanMaxY;
    public float ghostMinX, ghostMinY, ghostMaxX, ghostMaxY;

    public static Action<Vector2> ghostposition;

    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            spawnPlayer();
        }
    }

    public void spawnPlayer()
    {
        team = PhotonNetwork.LocalPlayer.GetPhotonTeam();

        if (team.Code == 1)
        {
            Vector2 ghostRandomPosition = new Vector2(UnityEngine.Random.Range(ghostMinX, ghostMaxX), UnityEngine.Random.Range(ghostMinY, ghostMaxY));
            PhotonNetwork.Instantiate(ghostPlayer.name, ghostRandomPosition, Quaternion.identity);
            ghostposition?.Invoke(ghostRandomPosition);
        }
        else if (team.Code == 2)
        {
            Vector2 pacmanRandomPosition = new Vector2(UnityEngine.Random.Range(pacmanMinX, pacmanMaxX), UnityEngine.Random.Range(pacmanMinY, pacmanMaxY));
            PhotonNetwork.Instantiate(pacmanPlayer.name, pacmanRandomPosition, Quaternion.identity);
        }
        else
        {
            Debug.Log("No team was the player assigned to");
        }




    }
}
