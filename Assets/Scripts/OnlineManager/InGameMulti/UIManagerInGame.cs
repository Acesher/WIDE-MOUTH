using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerInGame : MonoBehaviour
{

    public static UIManagerInGame Instance;

    public GameObject Ingame;
    public GameObject Ingamecanvas;
    public GameObject EndGameScreen;

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    public void CloseAllScreen()
    {
        Ingame.SetActive(false);
        Ingamecanvas.SetActive(false);
        EndGameScreen.SetActive(false);
    }

    public void OpenEndGameScreen()
    {
        CloseAllScreen();
        EndGameScreen.SetActive(true);
    }

}
