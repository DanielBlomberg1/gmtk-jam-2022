using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTile : MonoBehaviour
{

    private bool isRightState = false;
    private GameManager gameManager;
    private TilePoolGenerator tilePool;
    private LevelGenerator levelGen;
    private int index;
    private GameObject currently;

    // Start is called before the first frame update
    private void Start()
    {
        GameObject GameController = GameObject.FindGameObjectWithTag("GameController");
        gameManager = GameController.GetComponent<GameManager>();
        levelGen = GameController.GetComponent<LevelGenerator>();
        tilePool = GameObject.FindGameObjectWithTag("tilePool").GetComponent<TilePoolGenerator>();
        GameManager.stateChange += GameStateHandler;
    }


    private void GameStateHandler(GameManager.GameState newState)
    {
        if(newState == GameManager.GameState.PLACE_TILE)
        {
            isRightState = true;
            index = gameManager.LASTTHROWN;
            currently = tilePool.ORDERED[index];
        }
        else
        {
            isRightState = false;
        }
    }

    private void Update()
    {
        if (!isRightState)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Debug.Log("try");
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.gameObject.name);
                //Select stage    
                if (hit.transform.name == "Unset Tile(Clone)" || hit.transform.name == "Unset Tile")
                {
                    Debug.Log("hit right target" + levelGen.level.GetLength(0));
                    for (int i = 0; i<levelGen.level.GetLength(0); i++)
                    {
                        for (int j = 0; j < levelGen.level.GetLength(1); j++)
                        {
                            Debug.Log(levelGen.level[i, j].name);
                            if (levelGen.level[i, j].GetInstanceID() != gameObject.GetInstanceID())
                            {
                                continue;
                            }
                            levelGen.level[i, j] = currently;
                            levelGen.ReloadLevel();
                        }
                    }
                }
            }
        }
    }
}
