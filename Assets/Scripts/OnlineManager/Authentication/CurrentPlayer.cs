using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentPlayer : MonoBehaviour
{
    public int userid;
    public string email;
    public string username;
    public int ballance;
    public bool loggedin;


    // This will be our middle man between database server and local varriables 
    // Use to set || get data from database or from local
    private void Awake()
    {
        var players = FindObjectsOfType<CurrentPlayer>();
        if (players.Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
