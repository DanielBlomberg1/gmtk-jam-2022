using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerController : MonoBehaviour
{
    private bool isRightState = false;
    [SerializeField] private int currentTileIndex = 0;
    [SerializeField] private int playerHealthMax;
    [SerializeField] private int playerHealthCur;
    [SerializeField] private int rerolls;

    [SerializeField] private TextMeshProUGUI rerollText;

    private GameManager gameManager;
    private LevelGenerator levelGen;

    public int P_rerolls => rerolls;
    public int P_playerHealthMax => playerHealthMax;
    public int P_playerHealthCur => playerHealthCur;

    private void SetRerollText()
    {
        rerollText.text = "Rerolls left: " + rerolls;
    }
    public void RerollOnce()
    {
        rerolls -= 1;
        SetRerollText();
    }

    public void TryToAdvance()
    {
        if (!isRightState)
        {
            return;
        }

        List<Tile> curPath = levelGen.path;

        if (curPath.Count > currentTileIndex)
        {
            float diffX = curPath[currentTileIndex + 1].position.x - curPath[currentTileIndex].position.x;
            float diffZ = curPath[currentTileIndex + 1].position.y - curPath[currentTileIndex].position.y;

            Vector3 newPos = new(diffX + gameObject.transform.position.x, gameObject.transform.position.y, diffZ + gameObject.transform.position.z);
            gameObject.transform.position = newPos;

            // add tile behavior sowhere here 
            gameManager.PlayerHasAdvanced();

            currentTileIndex += 1;
        }
    }


    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        GameManager.stateChange += GameStateHandler;
        levelGen = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();
        SetRerollText();
    }

    private void GameStateHandler(GameManager.GameState newState)
    {
        if (newState == GameManager.GameState.ADVANCE_PLAYER)
        {
            isRightState = true;
        }
        else
        {
            isRightState = false;
        }
    }
}
