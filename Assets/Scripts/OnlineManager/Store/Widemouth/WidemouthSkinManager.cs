using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;

public class WidemouthSkinManager : MonoBehaviour
{
    public SpriteRenderer sr;
    public TMP_Text widemouthSkinName;
    public TMP_Text widemouthPrice;
    public Button confirmButton;

    public List<Sprite> widemouthSkins = new List<Sprite>();

    public List<string> widemouthSkinsNameList = new List<string>();

    public List<int> widemouthPricesList = new List<int>();

    public List<int> productPurchased = new List<int>();

    private int selectedSkin = 0;

    //public GameObject playerSkin;


    public static Action<int, int> OnConfirmBuy = delegate { };

    private void Start()
    {
        WidemouthBuy.OnUpdateNewestOption += HandlePlayerInventory;

        confirmButton.interactable = false;

        var currentPlayer = GameObject.FindGameObjectWithTag("CurrentPlayer");
        int currentPlayerUserid = currentPlayer.GetComponent<CurrentPlayer>().userid;
        int typeID = 1;
        StartCoroutine(GetPlayerData(currentPlayerUserid, typeID));
    }


    public void NextOption()
    {
        selectedSkin = selectedSkin + 1;
        if (selectedSkin == widemouthSkins.Count)
        {
            selectedSkin = 0;
        }


        updateUI(selectedSkin);


    }

    public void BackOption()
    {
        selectedSkin = selectedSkin - 1;
        if (selectedSkin < 0)
        {
            selectedSkin = widemouthSkins.Count - 1;
        }


        updateUI(selectedSkin);


    }

    public void updateUI(int selectedOption)
    {
        sr.sprite = widemouthSkins[selectedOption];
        widemouthSkinName.text = widemouthSkinsNameList[selectedOption];
        widemouthPrice.text = widemouthPricesList[selectedOption].ToString();
        bool check = productPurchased.Contains(selectedOption);
        if (check == false)
        {
            confirmButton.interactable = true;
        }
        else
        {
            confirmButton.interactable = false;
        }
        Debug.Log($"CURRENT SELECTED: {selectedSkin} + {check}");
    }

    public void ConfirmBuy()
    {
        int priceInInt = int.Parse(widemouthPrice.text);
        int selectedOptionDatabase = selectedSkin + 1; //Do database bat dau tu 1 va array c# bat dau tu 0
        //Debug.Log(selectedOptionDatabase);
        OnConfirmBuy?.Invoke(selectedOptionDatabase, priceInInt);
    }

    private void HandlePlayerInventory(int newestOption)
    {
        productPurchased.Add(newestOption);
        bool check = productPurchased.Contains(newestOption);
        if (check == false)
        {
            confirmButton.interactable = true;
        }
        else
        {
            confirmButton.interactable = false;
        }
        Debug.Log($"Hello just bought this: {productPurchased} + {newestOption}");
    }

    IEnumerator GetPlayerData(int userid, int typeid)
    {
        WWWForm webForm = new WWWForm();
        webForm.AddField("userid", userid);
        webForm.AddField("typeid", typeid);

        UnityWebRequest webRequest = UnityWebRequest.Post("https://wide-mouth-database.herokuapp.com/widemouthdata.php", webForm); // Change this to the actual URL I have setup on heroku

        yield return webRequest.SendWebRequest();

        string result = webRequest.downloadHandler.text;
        //Debug.Log(result);

        if (webRequest.isNetworkError)
        {
            Debug.Log("Error While Sending: " + webRequest.error);
        }
        else
        {
            Debug.Log("Received: " + webRequest.downloadHandler.text);
            if (result != "empty")
            {
                int numOfSkins = int.Parse(result.Split(':')[0]);
                for (int i = 1; i <= numOfSkins; i++)
                {
                    int itemID = int.Parse(result.Split(':')[i]) - 1;
                    productPurchased.Add(itemID);
                }
            }
            else
            {
                Debug.Log("bought nothing");
            }

        }
    }

    private void OnDestroy()
    {
        productPurchased.Clear();
    }

}
