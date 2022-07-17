using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        ROLL_DICE,
        CHOOSE_TILE,
        PLACE_TILE,
        ADVANCE_PLAYER,
        PLAYER_IN_COMBAT,
        PLAYER_HAS_WON,
        PAUSED,
        LOADING,
        MENU
    }

    public static Action<GameState> stateChange;

    //public Action OnStateChange stateChange;

    private TilePoolGenerator tileGen;
    private GameState curState;
    public Transform diceParent;
    public Transform tileParent;
    public GameObject dicePrefab;
    public Vector3 diceSpawnLocation;

    private int rolledDice;
    private List<int> diceFaces;

    public GameState CurrentState { 
        get { return curState; } 
        set
        {
            curState = value;
            Action<GameState> stateChange1 = stateChange;
            stateChange1(value);
        } 
    }

    private int chosenTile;
    public int CHOSENTILE => chosenTile;

    void Start()
    {
        tileGen = GameObject.FindGameObjectWithTag("tilePool").GetComponent<TilePoolGenerator>();
        //CurrentState = GameState.MENU;
        diceFaces = new List<int>();
        CurrentState = GameState.ROLL_DICE;
        stateChange(GameState.ROLL_DICE);

        SpawnDice();
        SpawnDice();

        DontDestroyOnLoad(this.gameObject);
    }

    public void DiceHasBeenRolled(int rolledAmount)
    {
        if (!(CurrentState == GameState.ROLL_DICE)) { return; }

        rolledDice++;

        diceFaces.Add(rolledAmount);

        // one dice
        if (diceParent.childCount == 1){
            TileHasBeenChosen(rolledAmount);

            rolledDice = 0;
            diceFaces.Clear();
        } 
        // multiple dice
        else if (rolledDice == diceParent.childCount) {
            for (int i = 0; i < diceFaces.Count; i++)
            {   
                tileParent.GetChild(diceFaces[i] - 1).GetComponent<Button>().enabled = true;
                tileParent.GetChild(diceFaces[i] - 1).GetChild(0).gameObject.SetActive(true);
            }
            
            rolledDice = 0;
            diceFaces.Clear();
            
            CurrentState = GameState.CHOOSE_TILE;
        }
    }

    public void TileHasBeenChosen(int p_chosenTile){
        chosenTile = p_chosenTile;

        foreach (Transform child in tileParent)
        {
            child.GetComponent<Button>().enabled = false;
            child.GetChild(0).gameObject.SetActive(false);
        }

        CurrentState = GameState.PLACE_TILE;
    }

    public void TileHasBeenPlaced()
    {
        if (!(CurrentState == GameState.PLACE_TILE)) { return; }

        CurrentState = GameState.ADVANCE_PLAYER;
    }
    
    public void PlayerHasAdvanced()
    {
        if(!(CurrentState == GameState.ADVANCE_PLAYER)) { return; }

        SpawnDice();

        tileGen.GenerateTilePool();
        CurrentState = GameState.ROLL_DICE;
    }

    public void StartGame()
    {
        CurrentState = GameState.LOADING;

        // load main scene here

        CurrentState = GameState.ROLL_DICE;
    }

    public void SpawnDice(){
        var dice = Instantiate(dicePrefab, diceSpawnLocation, Quaternion.identity);

        dice.transform.SetParent(diceParent);   
    }
}
