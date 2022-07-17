using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelection : MonoBehaviour
{
    GameManager gameManager;

    void Start() {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();    
    }

    public void SelectTile(){
        gameManager.TileHasBeenChosen(transform.GetSiblingIndex() + 1);
    }
}
