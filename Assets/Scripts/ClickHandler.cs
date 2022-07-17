using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{

    private bool isRightState = false;
    private GameManager gameManager;
    private TilePoolGenerator tilePool;
    private LevelGenerator levelGen;
    private int index;
    private int previousChildIndex;
    private GameObject currently;
    private Camera cam;

    private bool spaceLeft = true;

    // Start is called before the first frame update
    private void Start()
    {
        GameObject GameController = GameObject.FindGameObjectWithTag("GameController");
        gameManager = GameController.GetComponent<GameManager>();
        levelGen = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();
        tilePool = GameObject.FindGameObjectWithTag("tilePool").GetComponent<TilePoolGenerator>();
        GameManager.stateChange += GameStateHandler;
        cam = Camera.main;
    }


    private void GameStateHandler(GameManager.GameState newState)
    {
        if(newState == GameManager.GameState.PLACE_TILE)
        {
            isRightState = true;
            index = gameManager.CHOSENTILE - 1;
            currently = tilePool.ORDERED[index].actualGameObject;
        }
        else
        {
            isRightState = false;
        }
    }
    private void IsThereSpace()
    {
        spaceLeft = false;
        foreach (Tile tile in levelGen.path)
        {
            if (tile.tilePrefab.name.StartsWith("Unset Tile"))
            {
                spaceLeft = true;
            }
        }
        if (spaceLeft == false)
        {
            previousChildIndex = 9999;
            gameManager.TileHasBeenPlaced();
        }
    }

    private void Update()
    {
        if (!isRightState)
        {
            return;
        }

        IsThereSpace();
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 1f;
        int layerMask = 1 << 9;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(mousePos);

        if (Physics.Raycast(cam.transform.position, mouseWorldPos - cam.transform.position, out hit, Mathf.Infinity, layerMask)){

            // if mouse hovered on unset tile
            if (hit.transform.CompareTag("Unset"))
            {
                // change tile to new one
                int childIndex = hit.transform.GetSiblingIndex();

                if(previousChildIndex == 9999) { previousChildIndex = childIndex; } 

                levelGen.path[childIndex] = new Tile(levelGen.path[childIndex].position, currently, levelGen.path[childIndex].index);
                levelGen.ReloadLevel();

                // if different child index
                if (childIndex != previousChildIndex){

                    // reset previous tile to neutral
                    levelGen.path[previousChildIndex] = new Tile(levelGen.path[previousChildIndex].position, levelGen.neutralTile, levelGen.path[previousChildIndex].index);
                    levelGen.ReloadLevel();
                }

                previousChildIndex = childIndex;
            }

            // hovering same tile as prev frame
            else if (hit.transform.GetSiblingIndex() == previousChildIndex){
                if (Input.GetMouseButtonDown(0))
                {
                    gameManager.TileHasBeenPlaced();

                    previousChildIndex = 9999;
                }
            }
            else {
                if(previousChildIndex == 9999) { return; }
                levelGen.path[previousChildIndex] = new Tile(levelGen.path[previousChildIndex].position, levelGen.neutralTile, levelGen.path[previousChildIndex].index);
                levelGen.ReloadLevel();
            }
        }
    }
}
