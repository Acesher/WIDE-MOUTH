using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Store : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    void Update()
    {
        //Debug.Log($"{transform.parent.gameObject}: Current Tag: {gameObject.tag}");
    }

    public void OnSelect(BaseEventData eventData)
    {
        gameObject.tag = "itemInPacmanStore";
    }

    public void OnDeselect(BaseEventData eventData)
    {
        gameObject.tag = "Untagged";
    }
}
