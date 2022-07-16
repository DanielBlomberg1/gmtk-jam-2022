using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public int width;
    public int height;

    public Transform tileParent;

    public GameObject[] randomTiles;
    public GameObject neutralTile;
    public GameObject startTile;
    public GameObject endTile;

    GameObject[,] level;

    private void Start() {
        GenerateLevel();
    }

    public void GenerateLevel(){
        DeleteLevel();

        level = new GameObject[width, height];

        GeneratePath();

        level[0, 0] = startTile;
        level[height - 1, width - 1] = endTile;

        InstantiateLevel();
    }

    Vector2[] PickCorners(){
        Vector2[] corners = new Vector2[2];

        corners[0] = new Vector2(Random.Range(2, (int)(width / 2) - 1), Random.Range(2, height));
        corners[1] = new Vector2(Random.Range((int)(width / 2) + 1, width - 2), Random.Range(0, height - 2));

        return corners;
    }

    void GeneratePath(){
        Vector2[] corners = PickCorners();
        Vector2 currentTile = Vector2.zero;
        Vector2 target = corners[0];
        Vector2 end = new Vector2(width - 1, height - 1);

        while (currentTile != end){
            if (currentTile == corners[0]){
                target = corners[1];
            } else if (currentTile == corners[1]) {
                target = end;
            }

            if (currentTile.y < target.y){
                currentTile.y++;
            } else if (currentTile.y > target.y){
                currentTile.y--;
            } else if (currentTile.x < target.x){
                currentTile.x++;
            } else if (currentTile.x > target.x){
                currentTile.x--;
            }

            level[(int)currentTile.x, (int)currentTile.y] = neutralTile;
        }
    }

    void InstantiateLevel(){
        for (int y = height - 1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                if (level[x, y]) {
                    GameObject tile = Instantiate(level[x, y], new Vector3(x, 0, y), Quaternion.identity);

                    tile.transform.SetParent(tileParent);
                }
            }
        }
    }

    public void DeleteLevel(){
        while (tileParent.childCount > 0)
        {
            DestroyImmediate(tileParent.GetChild(0).gameObject);
        }
    }
}
