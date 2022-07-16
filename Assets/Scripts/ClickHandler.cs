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
    private GameObject currently;
    private Camera cam;

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
            index = gameManager.LASTTHROWN - 1;
           // Debug.Log(tilePool.ORDERED.Count);
            for(int i = 0; i < tilePool.ORDERED.Count; i++)
            {
                //Debug.Log(tilePool.ORDERED[i].name);
            }
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
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 1f;
            int layerMask = 1 << 8;
            layerMask = ~layerMask;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Vector3 mouseWorldPos = cam.ScreenToWorldPoint(mousePos);

            if (Physics.Raycast(cam.transform.position, mouseWorldPos - cam.transform.position, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.transform.CompareTag("Unset"))
                { 
                    int index = hit.transform.GetSiblingIndex();

                    levelGen.path[index] = new Tile(levelGen.path[index].position, currently, levelGen.path[index].index);
                    levelGen.ReloadLevel();
                }
            }
        }
    }
}
