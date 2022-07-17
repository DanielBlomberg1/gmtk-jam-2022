using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TileHashMap
{
    public GameObject uiPrefab;
    public GameObject actualGameObject;
    public TileHashMap(GameObject uiPrefab, GameObject actualGameObject)
    {
        this.uiPrefab = uiPrefab;
        this.actualGameObject = actualGameObject;
    }
}
public class TilePoolGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] tiles;
    [SerializeField] private GameObject[] actualTiles;
    [SerializeField] private int gap;
    [SerializeField] private float widthToTheLeft;
    [SerializeField] private float heightToTheUp;

    private List<Vector3> positionsUI;
    private List<TileHashMap> ordered;
    public List<TileHashMap> ORDERED => ordered;


    void Start()
    {
        positionsUI = new List<Vector3>(6);
        GetUIPos();
        ordered = new List<TileHashMap>(6);
        GenerateTilePool();
    }

    public void GenerateTilePool()
    {
        ordered.Clear();
        foreach (Transform child in transform)
        {
            if (!child.gameObject.name.StartsWith("TilePos1"))
            {
                GameObject.Destroy(child.gameObject);
            } 
        }

        GetUIPos();

        for (int i = 0; i < 6; i++)
        {
            float randomamount = Screen.currentResolution.height / gap;
            int randomizer = Random.Range(0, 6);
            GameObject uiTile = tiles[randomizer];
            GameObject actualTile = actualTiles[randomizer];
            ordered.Add(new TileHashMap(uiTile, actualTile));

            Instantiate(uiTile, positionsUI[i], Quaternion.identity, gameObject.transform);
        }

    }
    private void GetUIPos()
    {
        positionsUI.Clear();
        foreach (Transform child in transform)
        {
            positionsUI.Add(child.transform.position);
        }
    }

    public void RerollPool()
    {
        // if player.getcomponent<Stats>().rerolls > 0
        if (true == true)
        {
            GenerateTilePool();
        }
    }

}
