using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : MonoBehaviour
{
    public Sprite itemSprite;
    public string n;
    public int type;
    public int value;

    public Sprite ItemSprite { get => itemSprite; }
    public string Name { get => n; }
    public int Type { get => type; set => type = value; }
    public int Value { get => value; set => this.value = value; }

    private void Awake() {
        GetComponent<SpriteRenderer>().sprite = itemSprite;
    }
}
