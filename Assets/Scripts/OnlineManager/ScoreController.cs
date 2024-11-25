using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreController : MonoBehaviour
{

    private void Start()
    {
        GameManager.OnUpdateScore += HandleUpdateLocalScore;
    }

    private void OnDestroy()
    {
        GameManager.OnUpdateScore -= HandleUpdateLocalScore;
    }

    private void HandleUpdateLocalScore(int score)
    {
        var currentPlayer = GameObject.FindGameObjectWithTag("CurrentPlayer");
        if (currentPlayer != null)
        {
            int currentPlayerBallance = currentPlayer.GetComponent<CurrentPlayer>().ballance;
            int currentPlayerUserid = currentPlayer.GetComponent<CurrentPlayer>().userid;

            int newBallance = currentPlayerBallance + score;
            currentPlayer.GetComponent<CurrentPlayer>().ballance = newBallance;
            StartCoroutine(UpdateLocalToDatabase(currentPlayerUserid, newBallance));
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
