using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePacmanObject : MonoBehaviour
{
    public Sprite sprite;


    private void Awake()
    {
        var players = FindObjectsOfType<SpritePacmanObject>();
        if (players.Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
