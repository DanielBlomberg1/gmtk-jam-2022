using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePoolGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] tiles;
    private List<int> ids;
    public List<int> IDS => ids;


    void Start()
    {
        ids = new List<int>();
        GenerateTilePool();
    }

    public void GenerateTilePool()
    {
        foreach (Transform child in gameObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        Debug.Log("WTF");
        for(int i =0; i < 6; i++)
        {
            float randomamount = Screen.currentResolution.height / 16;
            GameObject randomOne = tiles[Random.Range(0, 6)];
            ids.Add(randomOne.GetInstanceID());
            Vector3 v = new( 80, 500 - i * randomamount, 0 );
            Instantiate(randomOne, v, Quaternion.identity, gameObject.transform);
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
