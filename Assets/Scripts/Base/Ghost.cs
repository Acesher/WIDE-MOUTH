using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ghost : MonoBehaviour
{

    public int score;
    public float respawnTimer;
    //public RespawnDicc rsp;

    public bool status;
    public float timer;

    Vector2 spawnpoints;

    public static Action<int> OnUpdatedScore;
    public static Action OnUpdateEats;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        status = false;

        SpawnPlayer.ghostposition += HandleGetGhostPosition;
    }

    private void OnDestroy()
    {
        SpawnPlayer.ghostposition -= HandleGetGhostPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.status && timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (this.status)
            {
                respawnSelf();
            }
        }
    }

    private void HandleGetGhostPosition(Vector2 position)
    {
        spawnpoints = position;
    }

    void addS(int val)
    {
        score += val;
        OnUpdatedScore?.Invoke(score);
        OnUpdateEats?.Invoke();

    }

    private void OnTriggerEnter2D(Collider2D obj)
    {

        if (obj.tag == "Player")
        {
            if (Pac.instance.status)
            {
                killSelf();
            }
            else
            {
                addS(500);
                //Object.Destroy(obj.gameObject);
            }
        }


    }

    private void killSelf()
    {
        this.status = true;
        this.gameObject.transform.position = spawnpoints;
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.timer = respawnTimer;
    }

    private void respawnSelf()
    {
        this.status = false;
        this.GetComponent<SpriteRenderer>().enabled = true;
        this.timer = 0;
    }

}
