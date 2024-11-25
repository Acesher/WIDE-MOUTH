using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class PlayerScript : MonoBehaviour
{

    public TMP_Text usernameInfo;
    public TMP_Text ballanceInfo;
    public GameObject logout;

    public GameObject connected;
    public GameObject notConnected;

    public Button singleplayerButton;
    public Button loginButton;
    public Button storeButton;

    // This script is used for our menu
    // To handle player state: login, logout, online or offline

    private void Start()
    {
        var currentPlayer = GameObject.FindGameObjectWithTag("CurrentPlayer");
        if (currentPlayer != null)
        {
            string currentPlayerUsername = currentPlayer.GetComponent<CurrentPlayer>().username;
            int currentPlayerBallance = currentPlayer.GetComponent<CurrentPlayer>().ballance;

            usernameInfo.text = "Hello " + currentPlayerUsername;
            ballanceInfo.text = currentPlayerBallance.ToString();
            logout.SetActive(true);

            connected.SetActive(true);
            notConnected.SetActive(false);
            storeButton.interactable = true;

            singleplayerButton.interactable = true;
            loginButton.interactable = false;
        }
        else
        {
            usernameInfo.text = "Hello Newbie";
            ballanceInfo.text = "0";

            connected.SetActive(false);
            notConnected.SetActive(true);
            storeButton.interactable = false;

            singleplayerButton.interactable = true;
            loginButton.interactable = true;
        }

    }

    public void Signout()
    {
        WWWForm webForm = new WWWForm();
        webForm.AddField("logout", "logedout");
        UnityWebRequest webRequest = UnityWebRequest.Post("https://wide-mouth-database.herokuapp.com/Logout.php", webForm); // Change this to the actual URL I have setup on heroku
        webRequest.SendWebRequest();

        var currentPlayer = GameObject.FindGameObjectsWithTag("CurrentPlayer");
        foreach (var player in currentPlayer)
        {
            Destroy(player);
        }
        FindObjectOfType<SceneSwitcher>().LoadMenuScene();
    }
    private void OnApplicationQuit()
    {
        WWWForm webForm = new WWWForm();
        webForm.AddField("logout", "logedout");
        UnityWebRequest webRequest = UnityWebRequest.Post("https://wide-mouth-database.herokuapp.com/Logout.php", webForm); // Change this to the actual URL I have setup on heroku
        webRequest.SendWebRequest();

    }
}
