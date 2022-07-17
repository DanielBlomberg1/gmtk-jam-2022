using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentObjectiveManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI tipText;
    [SerializeField] private Transform diceParent;
    [SerializeField] private GameObject dicePrefab;
    [SerializeField] private GameObject AdvanceButton;
    [SerializeField] private Vector3 diceSpawnLocation;

    // Start is called before the first frame update
    private void Start()
    {
        GameManager.stateChange += GameStateHandler;
    }


    private void GameStateHandler(GameManager.GameState newState)
    {
        switch (newState)
        {
            case GameManager.GameState.PLACE_TILE:
                tipText.text = "Place Tile";
                break;
            case GameManager.GameState.ROLL_DICE:
                tipText.text = "Roll Dice";
                break;
            case GameManager.GameState.CHOOSE_TILE:
                tipText.text = "Choose Tile";
                break;
            case GameManager.GameState.ADVANCE_PLAYER:
                tipText.text = "Advance";
                break;
            default:
                break;

        }
    }

}
