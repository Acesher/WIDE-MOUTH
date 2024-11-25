using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System;

public class GhostSkinBuy : MonoBehaviour
{
    public TMP_Text warningBuyText;
    public TMP_Text confirmBuyText;
    public int typeid = 2;

    public static Action<int> OnBallanceUpdated = delegate { };
    public static Action<int> OnUpdateNewestOption = delegate { };

    private void Awake()
    {
        GameObject[] ghostObject = GameObject.FindGameObjectsWithTag("itemInGhostStore");
        DontDestroyOnLoad(this.gameObject.transform.root);
        if (ghostObject.Length > 1)
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        GhostSkinManager.OnConfirmBuy += HandleConfirmBuy;
    }

    private void HandleConfirmBuy(int itemid, int price)
    {
        var currentPlayer = GameObject.FindGameObjectWithTag("CurrentPlayer");
        int currentPlayerBallance = currentPlayer.GetComponent<CurrentPlayer>().ballance;
        int currentPlayerUserid = currentPlayer.GetComponent<CurrentPlayer>().userid;


        //Debug.Log($"ITEMID: {itemid}, USERID: {currentPlayerBallance}, PRICE: {price}");
        if (currentPlayerBallance >= price)
        {
            //Update UI + Update Database + Update CurrentPlayerObj
            int newBallance = currentPlayerBallance - price;
            currentPlayer.GetComponent<CurrentPlayer>().ballance = newBallance;
            StartCoroutine(Purchase(currentPlayerUserid, newBallance, itemid));
        }
        else
        {
            warningBuyText.text = "Not enough money";
            Invoke("ClearField", 3);
        }

    }

    private void ClearField()
    {
        warningBuyText.text = "";
        confirmBuyText.text = "";
    }

    IEnumerator Purchase(int userid, int ballance, int productid)
    {
        WWWForm webForm = new WWWForm();
        webForm.AddField("userid", userid);
        webForm.AddField("ballance", ballance);
        webForm.AddField("productid", productid);
        webForm.AddField("typeid", typeid);
        UnityWebRequest webRequest = UnityWebRequest.Post("https://wide-mouth-database.herokuapp.com/Store.php", webForm); // Change this to the actual URL I have setup on heroku

        yield return webRequest.SendWebRequest();

        string result = webRequest.downloadHandler.text;

        if (webRequest.isNetworkError)
        {
            Debug.Log("Error While Sending: " + webRequest.error);
        }
        else
        {
            Debug.Log("Received: " + result);
            if (result != "Item Purchased")
            {
                warningBuyText.text = result;
                yield return new WaitForSeconds(1);
                ClearField();
            }
            else
            {
                confirmBuyText.text = "Item Purchased";
                OnBallanceUpdated?.Invoke(ballance);
                OnUpdateNewestOption?.Invoke(productid - 1);
                yield return new WaitForSeconds(2);
                confirmBuyText.text = " ";
            }
        }
    }

}
