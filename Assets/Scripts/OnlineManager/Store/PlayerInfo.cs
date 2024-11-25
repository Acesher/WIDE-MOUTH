using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class PlayerInfo : MonoBehaviour
{

    public TMP_Text ballanceInfo;
    //public Button buyButton;

    // Start is called before the first frame update
    void Start()
    {
        var currentPlayer = GameObject.FindGameObjectWithTag("CurrentPlayer");

        if (currentPlayer != null)
        {
            int currentPlayerBallance = currentPlayer.GetComponent<CurrentPlayer>().ballance;
            ballanceInfo.text = currentPlayerBallance.ToString();
            //buyButton.interactable = true;
        }
        else
        {
            ballanceInfo.text = "0";
            //buyButton.interactable = false;
        }

        WidemouthBuy.OnBallanceUpdated += HandleUserBallanceChanged;
        GhostSkinBuy.OnBallanceUpdated += HandleUserBallanceChanged;

    }

    private void HandleUserBallanceChanged(int newBallance)
    {
        ballanceInfo.text = newBallance.ToString();
    }


}
