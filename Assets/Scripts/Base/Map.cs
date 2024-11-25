using UnityEngine;

[System.Serializable]
public class Map
{
    public Vector2Int top;
    public Vector2Int bot;
    public string nick;

    public Vector2Int Top { get => top; set => top = value; }
    public Vector2Int Bot { get => bot; set => bot = value; }
    public string Nickname { get => nick; }
}
