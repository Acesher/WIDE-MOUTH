using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersList : MonoBehaviour
{
    public Pac[] pacs;
    public bool[] pacstatus;
    public Ghost[] ghosts;
    public bool[] ghoststatus;
    public Vector2[] spawnPoints;

    void Awake() {
        pacstatus = GetInitializedArray(pacs.Length, true);
        ghoststatus = GetInitializedArray(ghosts.Length, true);
    }

    public static T[] GetInitializedArray<T>(int length, T initialValue)
    {
        T[] result = new T[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = initialValue;
        }
        return result;
    } 

    public void removePlayer() {

    }
}
