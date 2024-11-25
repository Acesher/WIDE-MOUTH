using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneSwitcher : MonoBehaviour
{

    public static SceneSwitcher Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }


    public void LoadAccountScene()
    {
        SceneManager.LoadScene("Authentication");
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }

    public void LoadMultiplayerScene()
    {
        SceneManager.LoadScene("Lobby and Host Room");
    }

    public void LoadStoreScene()
    {
        SceneManager.LoadScene("Store");
    }

    public void LoadSingleScene()
    {
        SceneManager.LoadScene("Single_Lobby");
    }

    public void LoadSingleOneScene()
    {
        SceneManager.LoadScene("SinglePLayer_1");
    }
    public void LoadSingleTwoScene()
    {
        SceneManager.LoadScene("SinglePLayer_2");
    }

    public void LoadSingleThreeScene()
    {
        SceneManager.LoadScene("SinglePLayer_3");
    }

    public void LoadEndScene()
    {
        SceneManager.LoadScene("End game");
    }
}
