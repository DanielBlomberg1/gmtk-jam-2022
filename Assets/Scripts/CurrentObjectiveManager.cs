using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentObjectiveManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI tipText;
    [SerializeField] private GameObject dice;
    [SerializeField] private GameObject AdvanceButton;

    private GameManager GM;

    private const int RotationSpeed = 100;
    private Transform diceTransform;

    // Start is called before the first frame update
    private void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        diceTransform = dice.transform;
        GameManager.stateChange += GameStateHandler;
    }


    private void GameStateHandler(GameManager.GameState newState)
    {
        switch (newState)
        {
            case GameManager.GameState.PLACE_TILE:
                tipText.text = "Choose Tile";
                break;
            case GameManager.GameState.ROLL_DICE:
                tipText.text = "Roll Dice";

                // reset position and rotation and make the dice active
                dice.transform.position = diceTransform.position;
                dice.transform.rotation = diceTransform.rotation;

                dice.SetActive(true);
                break;
            case GameManager.GameState.ADVANCE_PLAYER:
                tipText.text = "Advance Player";
                break;
            default:
                break;

        }
    }

}
