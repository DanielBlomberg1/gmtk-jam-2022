using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePoolGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] tiles;
    [SerializeField] private int gap;
    private List<GameObject> ordered;
    public List<GameObject> ORDERED => ordered;


    void Start()
    {
        ordered = new List<GameObject>();
        GenerateTilePool();
    }

    public void GenerateTilePool()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for(int i = 0; i < 6; i++)
        {
            float randomamount = Screen.currentResolution.height / gap;
            GameObject randomOne = tiles[Random.Range(0, 6)];
            ordered.Add(randomOne);

            // saat ite korjata
            //Vector3 v = new( 80, 500 - i * randomamount, 0 );

            //Instantiate(randomOne, v, Quaternion.identity, transform);
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
