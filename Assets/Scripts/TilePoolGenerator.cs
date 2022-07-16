using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePoolGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] tiles;
    //private GameObject[] ordered;

    void Start()
    {
        GenerateRandomSixNumbers();
    }

    void GenerateRandomSixNumbers()
    {
        Debug.Log("WTF");
        foreach (var tile in tiles)
        {
            //tiles.CopyTo(ordered, Random.Range(0, 6));
            GameObject randomOne = tiles[Random.Range(0, 6)];
            Vector3 v = new( 0, 0, 0 );
            Instantiate(randomOne, v, Quaternion.identity, gameObject.transform);
        }

    }
}
