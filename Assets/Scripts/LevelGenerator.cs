using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Tile {
    public Vector2 position;
    public GameObject tilePrefab;
    public int index;

    public Tile(Vector2 pos, GameObject tilePrefab, int index){
        this.position = pos;
        this.tilePrefab = tilePrefab;
        this.index = index;
    }
}

public class LevelGenerator : MonoBehaviour
{
    public int width;
    public int height;
    public int maxRandomTiles;
    public int randomTileChance;

    public Transform tileParent;

    public GameObject[] randomTiles;
    public GameObject neutralTile;
    public GameObject combatTile;
    public GameObject startTile;
    public GameObject endTile;

    public List<Tile> path;
    public List<GameObject> pathObjects;

    private void Start() {
        GenerateLevel();
    }

    public void GenerateLevel(){
        DeleteLevel();
        
        path = new List<Tile>();

        GeneratePath();

        // set start and end tiles
        path[0] = new Tile(path[0].position, startTile, path[0].index);
        path[path.Count - 1] = new Tile(path[path.Count - 1].position, endTile, path[path.Count - 1].index);

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

        int tileIndex = 0;
        int randomTileCount = 0;
        int previousRandomTile = -1;

        while (true) {
            if (currentTile == corners[0]){
                target = corners[1];
            } else if (currentTile == corners[1]) {
                target = end;
            }

            // choose tile
            if (tileIndex == 1){
                path.Add(new Tile(currentTile, combatTile, tileIndex));
            } 
            else if (Random.Range(0, 100) < randomTileChance && randomTileCount <= maxRandomTiles){
                randomTileCount++;

                int randomTileIndex = Random.Range(0, randomTiles.Length);

                if (randomTileIndex == previousRandomTile){
                    if (randomTileIndex == randomTiles.Length - 1){
                        randomTileIndex--;
                    } else {
                        randomTileIndex++;
                    }
                }

                previousRandomTile = randomTileIndex;
                
                path.Add(new Tile(currentTile, randomTiles[randomTileIndex], tileIndex));
            }
            else {
                path.Add(new Tile(currentTile, neutralTile, tileIndex));      
            }

            if (currentTile == end){
                break;
            }

            // move to next position
            if (currentTile.y < target.y){
                currentTile.y++;
            } else if (currentTile.y > target.y){
                currentTile.y--;
            } else if (currentTile.x < target.x){
                currentTile.x++;
            } else if (currentTile.x > target.x){
                currentTile.x--;
            }

            tileIndex++;
        }
    }

    public void ChangeTileFromIndex(int index, GameObject gm)
    {
        path[index] = new Tile(path[index].position, gm, path[index].index);
    }

    public int getPathLength()
    {
        return path.Count;
    }

    public void ReloadLevel()
    {
        DeleteLevel();
        InstantiateLevel();
    }

    void InstantiateLevel(){
        pathObjects.Clear();
        foreach (Tile tile in path)
        {
            GameObject tileObject = Instantiate(tile.tilePrefab, new Vector3(tile.position.x, 0, tile.position.y), Quaternion.identity);
            pathObjects.Add(tileObject);
            tileObject.transform.SetParent(tileParent);
        }
    }

    public void DeleteLevel(){
        while (tileParent.childCount > 0)
        {
            DestroyImmediate(tileParent.GetChild(0).gameObject);
        }
    }
}
