using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnDicc : MonoBehaviour
{
    [SerializeField] public Transform[] respawnPoints;
    private int index;

    void Awake() {
        index = 0;
    }

    public Transform getNextRespawnPoint() {
        if (index == respawnPoints.Length - 1) {
            index = 0;
        } 
        else {
            index++;
        }
        return respawnPoints[index];
    } 
}
