using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;
using TMPro;
using UnityEngine.Networking;
using Photon.Pun.UtilityScripts;

public class GameController : MonoBehaviour
{

    //public PlayersList players;

    public int PScore;
    public int GScore;
    public Text time;

    public TMP_Text ghostScore;
    public TMP_Text pacScore;

    public PhotonTeam team;

    PhotonView view;

    public float TimeLimit = DoE.TIMELIMIT;

    public GameObject currentPac;

    public GameObject currentPellets;

    public static Action OnDoneGame = delegate { };

    void Awake()
    {
        this.PScore = 0;
        this.GScore = 0;

        Ghost.OnUpdatedScore += HandleUpdatedGhostScore;
        Pac.OnUpdatedScore += HandleUpdatedPacScore;
        Ghost.OnUpdateEats += HandleUpdateGhostEats;
        Pac.OnUpdateEats += HandleUpdatePacEats;

        team = PhotonNetwork.LocalPlayer.GetPhotonTeam();

    }

    private void OnDestroy()
    {
        Ghost.OnUpdatedScore -= HandleUpdatedGhostScore;
        Pac.OnUpdatedScore -= HandleUpdatedPacScore;
        Ghost.OnUpdateEats -= HandleUpdateGhostEats;
        Pac.OnUpdateEats -= HandleUpdatePacEats;
    }

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        // TileBase[,] test = TilemapSorting.LayoutMaker(new Vector2Int(1, 30), new Vector2Int(26, 2));     
        // for (int i = 0; i < 29; i++) {
        //     for (int j = 0; j < 26; j++) {
        //         if (test[i,j] != null) {
        //             Debug.Log("x: " + i + "y: " + j + ", Tile: " + test[i,j].name);
        //         }
        //         else {
        //             Debug.Log("x: " + i + "y: " + j + ", Tile: NULL");
        //         }
        //     }
        // }
    }


    private void HandleUpdatedGhostScore(int newScore)
    {
        view.RPC("HandleUpdateGhostScoreRPC", RpcTarget.All, newScore);
    }

    [PunRPC]
    private void HandleUpdateGhostScoreRPC(int newScore)
    {

        this.GScore += newScore;


        //updateScore();
        Debug.Log($"LOCAL SCORE: {this.GScore} NEW SCORE RECEIVED: {newScore}");

    }

    private void HandleUpdatedPacScore(int newScore)
    {
        view.RPC("HandleUpdatePacScore", RpcTarget.All, newScore);
    }

    [PunRPC]
    private void HandleUpdatePacScore(int newScore)
    {

        this.PScore += newScore;

        //updateScore();
        Debug.Log($"LOCAL SCORE: {this.GScore} NEW SCORE RECEIVED: {newScore}");
    }

    private void HandleUpdateGhostEats()
    {
        Debug.Log("1");
        view.RPC("HandleUpdateGhostEatsRPC", RpcTarget.All);
        Debug.Log("2");
    }

    [PunRPC]
    private void HandleUpdateGhostEatsRPC()
    {
        Debug.Log("3");
        currentPac = GameObject.FindGameObjectWithTag("Player");
        currentPellets = GameObject.FindGameObjectWithTag("Pellets");

        //Debug.Log($"IS PAC ACTIVE: {currentPac.activeSelf}");
        if (currentPac == null)
        {
            //OnDoneGame?.Invoke();
            //SceneSwitcher.Instance.LoadEndScene();
            HandleScore(GScore, PScore);
        }
        Debug.Log("4");
    }

    private void HandleUpdatePacEats()
    {
        view.RPC("HandleUpdatePacEatsRPC", RpcTarget.All);
    }

    [PunRPC]
    private void HandleUpdatePacEatsRPC()
    {
        currentPac = GameObject.FindGameObjectWithTag("Player");
        currentPellets = GameObject.FindGameObjectWithTag("Pellets");

        if (currentPellets == null)
        {
            //OnDoneGame?.Invoke();
            //SceneSwitcher.Instance.LoadEndScene();
            HandleScore(GScore, PScore);
        }
    }


    private void FixedUpdate()
    {
        if (this.TimeLimit <= 0)
        {
            //OnDoneGame?.Invoke();
            //SceneSwitcher.Instance.LoadEndScene();
            HandleScore(GScore, PScore);
        }
        this.TimeLimit -= Time.fixedDeltaTime;
        time.text = Mathf.FloorToInt(this.TimeLimit).ToString();
    }

    public void HandleScore(int gScore, int pScore)
    {
        Debug.Log($"GHOST SCORE {gScore} \n PAC SCORE {pScore}");

        gScore = gScore / (PhotonNetwork.PlayerList.Length);
        pScore = pScore / (PhotonNetwork.PlayerList.Length);

        ghostScore.text = gScore.ToString();
        pacScore.text = pScore.ToString();

        UIManagerInGame.Instance.OpenEndGameScreen();

        var currentPlayer = GameObject.FindGameObjectWithTag("CurrentPlayer");
        if (currentPlayer != null)
        {
            int currentPlayerBallance = currentPlayer.GetComponent<CurrentPlayer>().ballance;
            int currentPlayerUserid = currentPlayer.GetComponent<CurrentPlayer>().userid;

            if (team.Code == 1)
            {
                int newBallance = currentPlayerBallance + gScore;
                currentPlayer.GetComponent<CurrentPlayer>().ballance = newBallance;
                StartCoroutine(UpdateLocalToDatabase(currentPlayerUserid, newBallance));
            }
            else if (team.Code == 2)
            {
                int newBallance = currentPlayerBallance + pScore;
                currentPlayer.GetComponent<CurrentPlayer>().ballance = newBallance;
                StartCoroutine(UpdateLocalToDatabase(currentPlayerUserid, newBallance));
            }

        }
    }

    IEnumerator UpdateLocalToDatabase(int userid, int newBallance)
    {
        WWWForm webForm = new WWWForm();
        webForm.AddField("userid", userid);
        webForm.AddField("ballance", newBallance);

        UnityWebRequest webRequest = UnityWebRequest.Post("https://wide-mouth-database.herokuapp.com/updateUser.php", webForm); // Change this to the actual URL I have setup on heroku

        yield return webRequest.SendWebRequest();

        string result = webRequest.downloadHandler.text;

        if (webRequest.isNetworkError)
        {
            Debug.Log("Error While Sending: " + webRequest.error);
        }
        else
        {
            Debug.Log("Received: " + result);
            if (result != "User Ballance Updated")
            {
                Debug.Log(result);
            }
            else
            {
                Debug.Log($"Update Success: {result}");
            }
        }
    }
}
