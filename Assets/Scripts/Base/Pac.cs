using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pac : MonoBehaviour
{
    public int score;
    public bool status;
    public float PowerPelletTimer;
    private float ppt;

    public static Action<int> OnUpdatedScore;
    public static Action OnUpdateEats;

    public static Pac instance;

    // Start is called before the first frame update
    void Start()
    {
        this.score = 0;
        this.ppt = 0;
        this.status = false; 
        instance = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ppt > 0)
        {
            ppt -= Time.fixedDeltaTime;
        }
        else if (ppt <= 0 && status)
        {
            LosePower();
        }
    }

    void addS(int val)
    {
        score += val;
        OnUpdatedScore?.Invoke(val);
        OnUpdateEats?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.tag == "Pellets")
        {
            UnityEngine.Object.Destroy(obj.gameObject);
            Item itemCollected = obj.GetComponentInParent<Item>();
            switch (itemCollected.type)
            {
                case 1:
                    GainPower();
                    break;
                default:
                    int addItemScore = itemCollected.Value;
                    addS(addItemScore);
                    break;
            }
        }

        if (obj.tag == "Ghosts" && status)
        {
            addS(100);
        }
        else if (obj.tag == "Ghosts")
        {
            this.gameObject.SetActive(false);
            this.gameObject.transform.position = new Vector3Int(1000, 1000, -30);
        }

    }

    void GainPower()
    {
        this.status = true;
        ppt = PowerPelletTimer;
    }

    void LosePower()
    {
        this.status = false;
        ppt = 0;
    }
}
