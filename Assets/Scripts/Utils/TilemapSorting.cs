//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
//using System.IO;

public static class TilemapSorting
{

    private static Tilemap tilemap;
    private static BoundsInt pathBounds; // REFERENCING THE SCENE, X IS HORIZONTAL, Y IS VERTICAL, STARTING FROM BOTTOM LEFT TO UPPER RIGHT
    private static TileBase[] allTiles;

    private static void Init() {
        tilemap = GameObject.Find("TilePath").GetComponent<Tilemap>();
        pathBounds = tilemap.cellBounds;
        allTiles = tilemap.GetTilesBlock(pathBounds);
    }

    /*void Start() {
        tilemap = GameObject.Find("TilePath").GetComponent<Tilemap>();
        pathBounds = tilemap.cellBounds;
        allTiles = tilemap.GetTilesBlock(pathBounds);

        // for (int i = 0; i < pathBounds.size.x; i++) {
        //     for (int j = 0; j < pathBounds.size.y; j++) {
        //         // if (tilemap.HasTile(new Vector3Int(i, j, 1))) {
        //         //     test[i,j] =  
        //         // }
        //         TileBase tile = allTiles[i + j * pathBounds.size.x];
        //         if (tile != null) {
        //             Debug.Log("x:" + i + " y:" + j + " tile:" + tile.name);
        //         } else {
        //             Debug.Log("x:" + i + " y:" + j + " tile: (null)");
        //         }
        //     }
        // }

        // Flushing
        FileStream fs = File.Open("Assets/test.txt", FileMode.Open);
        fs.SetLength(0);
        fs.Close();

        // This one has the normal co-ord looping
        
        // StreamWriter writer = new StreamWriter("Assets/test.txt", true);
        // for (int i = pathBounds.size.y - 1; i >= 0; i--) {
        //     for (int j = 0; j < pathBounds.size.x; j++) {
        //         TileBase tile = allTiles[i * pathBounds.size.x + j];
        //         if (tile != null) {
        //             writer.Write('#');
        //         } 
        //         else {
        //             writer.Write("_");
        //         }
        //     }
        //     writer.WriteLine();
        // }
        // writer.Close();

        // int count = 0;
        // foreach (TileBase i in allTiles) {
        //     if (i != null) {
        //         writer.Write('#');
        //     } 
        //     else {
        //         writer.Write("_");
        //     }
        //     if (count == pathBounds.size.x - 1){
        //         writer.WriteLine();
        //         count = 0;
        //     }
        //     else { count++; }
        // }
        // writer.Close();

    }*/

    public static TileBase[,] LayoutMaker(Vector2Int topBound, Vector2Int botBound) {
        Init();

        int sizeX = Mathf.Abs(topBound.x - botBound.x) + 1;
        int sizeY = Mathf.Abs(topBound.y - botBound.y) + 1;

        TileBase[,] layout = new TileBase[sizeY, sizeX];

        for (int i = sizeY - 1; i >= 0; i--) {
            for (int j = 0; j < sizeX; j++) {
                // Debug.Log((i + botBound.y) * pathBounds.size.x + (j + topBound.x));
                // layout[i,j] = allTiles[i + topBound.x + (j + botBound.y) * pathBounds.size.x];
                layout[Mathf.Abs(i - sizeY + 1),j] = allTiles[(i + botBound.y) * pathBounds.size.x + (j + topBound.x)];
            }
        }
        Debug.Log("Size: " + layout.Length);
        return layout;
    }

}
