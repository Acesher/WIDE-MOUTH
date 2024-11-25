using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeAndDeathController : MonoBehaviour
{

    public PlayersList playerslist;
    private List<int> deathqueue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < playerslist.pacstatus.Length; i++) {
            if (playerslist.pacstatus[i] == false) {
                deathqueue.Add(i);
            }
        }
        for (int i = 0; i < playerslist.ghoststatus.Length; i++) {
            if (playerslist.ghoststatus[i] == false) {
                deathqueue.Add(i);
            }
        }
    }

    void kill (Pac player) {
        Destroy(player.GetComponent<Movement>());
        player.gameObject.SetActive(false);
    }

    void kill (Ghost player, Vector2 spawnPoint) {
        player.gameObject.SetActive(false);
        player.transform.position = spawnPoint;
    }

    void respawn (Ghost player) {
        player.gameObject.SetActive(true);
    }
}
