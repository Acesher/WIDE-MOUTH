using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreUIManager : MonoBehaviour
{
    public GameObject widemouthList;
    public GameObject ghostList;

    public void closeMenus()
    {
        widemouthList.SetActive(false);
        ghostList.SetActive(false);
    }

    public void openPacmanList()
    {
        closeMenus();
        widemouthList.SetActive(true);
    }

    public void openGhostList()
    {
        closeMenus();
        ghostList.SetActive(true);
    }

}
