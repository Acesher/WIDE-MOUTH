using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapDicc
{
    public static readonly MapInfo mapWeird = new MapInfo(1, 30, 26, 2, "MapWeird");

    public static MapInfo fetch(string nickname) {
        switch (nickname)
        {
            case "MapWeird": return mapWeird;

            default: return null;
        }
    }
}

public class MapInfo
{
    public Vector2Int top;
    public Vector2Int bot;
    public string nick;

    public Vector2Int Top { get => top; set => top = value; }
    public Vector2Int Bot { get => bot; set => bot = value; }
    public string Nickname { get => nick; }

    public MapInfo(int x1, int y1, int x2, int y2, string nickname) {
        this.top.x = x1;
        this.top.y = y1;
        this.bot.x = x2;
        this.bot.y = y2;
        this.nick = nickname;
    }
}