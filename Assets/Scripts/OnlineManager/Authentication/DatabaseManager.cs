using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class DatabaseManager : MonoBehaviour
{
    //Login variables
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    //Register variables
    [Header("Register")]
    public TMP_InputField emailRegisterField;
    public TMP_InputField usernameRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_Text warningRegisterText;
    public TMP_Text confirmRegisterText;

    [Header("CurrentPlayer")]
    public GameObject currentPlayerObject;


    // This script is used for authentication
    private void Awake()
    {
        var currentPlayer = GameObject.FindGameObjectsWithTag("CurrentPlayer");
        foreach (var player in currentPlayer)
        {
            Destroy(player);
        }
    }


    public void ClearLoginFeilds()
    {
        emailLoginField.text = "";
        passwordLoginField.text = "";
        warningLoginText.text = "";
        confirmLoginText.text = "";
    }
    public void ClearRegisterFeilds()
    {
        usernameRegisterField.text = "";
        emailRegisterField.text = "";
        passwordRegisterField.text = "";
        confirmRegisterText.text = "";
        warningRegisterText.text = "";
    }

    public void RegisterButton()
    {
        StartCoroutine(Register());
    }

    public void LoginButton()
    {
        StartCoroutine(Login());
    }

    IEnumerator Register()
    {
        WWWForm webForm = new WWWForm();
        webForm.AddField("emailRegister", emailRegisterField.text);
        webForm.AddField("usernameRegister", usernameRegisterField.text);
        webForm.AddField("passwordRegister", passwordRegisterField.text);

        UnityWebRequest webRequest = UnityWebRequest.Post("https://wide-mouth-database.herokuapp.com/Signup.php", webForm); // Change this to the actual URL I have setup on heroku
        yield return webRequest.SendWebRequest();

        string result = webRequest.downloadHandler.text;
        if (webRequest.isNetworkError)
        {
            Debug.Log("Error While Sending: " + webRequest.error);
        }
        else
        {
            Debug.Log("Received: " + result);
            if (result != "Account Create Successfully")
            {
                warningRegisterText.text = result;
                yield return new WaitForSeconds(1);
                ClearRegisterFeilds();
            }
            else
            {
                ClearRegisterFeilds();
                confirmRegisterText.text = "Account Create Successfully";
                yield return new WaitForSeconds(2);
                UIManager.instance.LoginScreen();
            }
        }

    }


    IEnumerator Login()
    {

        WWWForm webForm = new WWWForm();
        webForm.AddField("emailLogin", emailLoginField.text);
        webForm.AddField("passwordLogin", passwordLoginField.text);
        UnityWebRequest webRequest = UnityWebRequest.Post("https://wide-mouth-database.herokuapp.com/Login.php", webForm); // Change this to the actual URL I have setup on heroku

        yield return webRequest.SendWebRequest();

        string result = webRequest.downloadHandler.text;

        if (webRequest.isNetworkError)
        {
            Debug.Log("Error While Sending: " + webRequest.error);
        }
        else
        {
            Debug.Log("Received: " + webRequest.downloadHandler.text);
            if (result == "Invalid email || Password" || result == "You cant leave Email || Password empty")
            {
                warningLoginText.text = result;
                yield return new WaitForSeconds(1);
                ClearLoginFeilds();
            }
            else
            {
                // Some data handles with the server response
                var currentPlayer = Instantiate(currentPlayerObject, new Vector3(0, 0, 0), Quaternion.identity);
                currentPlayer.GetComponent<CurrentPlayer>().userid = int.Parse(result.Split(':')[0]);
                currentPlayer.GetComponent<CurrentPlayer>().email = result.Split(':')[1];
                currentPlayer.GetComponent<CurrentPlayer>().username = result.Split(':')[2];
                currentPlayer.GetComponent<CurrentPlayer>().ballance = int.Parse(result.Split(':')[3]);
                confirmLoginText.text = "Login successfully";
                yield return new WaitForSeconds(2);
                FindObjectOfType<SceneSwitcher>().LoadMenuScene();
            }
        }
    }

    private void OnApplicationQuit()
    {
        WWWForm webForm = new WWWForm();
        webForm.AddField("logout", "logedout");
        UnityWebRequest webRequest = UnityWebRequest.Post("https://wide-mouth-database.herokuapp.com/Logout.php", webForm); // Change this to the actual URL I have setup on heroku
        webRequest.SendWebRequest();
    }

}
