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
    private List<TileHashMap> ordered;
    public List<TileHashMap> ORDERED => ordered;


    void Start()
    {
        ordered = new List<TileHashMap>(6);
        GenerateTilePool();
    }

    public void GenerateTilePool()
    {
        ordered.Clear();
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for(int i = 0; i < 6; i++)
        {
            float randomamount = Screen.currentResolution.height / gap;
            int randomizer = Random.Range(0, 6);
            GameObject uiTile = tiles[randomizer];
            GameObject actualTile = actualTiles[randomizer];
            ordered.Add(new TileHashMap(uiTile, actualTile));



            Vector3 v = new(Screen.currentResolution.width / widthToTheLeft, Screen.currentResolution.height / heightToTheUp - i * randomamount, 0);


            Instantiate(uiTile, v, Quaternion.identity, gameObject.transform);
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
