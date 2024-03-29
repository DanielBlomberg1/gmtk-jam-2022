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
        MENU,
        DEATH
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
            stateChange?.Invoke(value);
        } 
    }

    private int chosenTile;
    public int CHOSENTILE => chosenTile;

    public string deathMessage;

    void Start()
    {
        tileGen = GameObject.FindGameObjectWithTag("tilePool").GetComponent<TilePoolGenerator>();
        //CurrentState = GameState.MENU;
        diceFaces = new List<int>();
        CurrentState = GameState.ROLL_DICE;

        SpawnDice();
        SpawnDice();

    }

    public void DiceHasBeenRolled(int rolledAmount)
    {
        if (!(CurrentState == GameState.ROLL_DICE)) { return; }

        rolledDice++;

        diceFaces.Add(rolledAmount);

        // one dice
        if (diceParent.childCount == 1){
            TileHasBeenChosen(rolledAmount);

            tileParent.GetChild(rolledAmount - 1).GetChild(0).gameObject.SetActive(true);

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

        foreach (Transform dice in diceParent)
        {
            Destroy(dice.gameObject);
        }

        foreach (Transform child in tileParent)
        {
            if (child.GetSiblingIndex() != chosenTile - 1){
                child.GetChild(0).gameObject.SetActive(false);
            }

            child.GetComponent<Button>().enabled = false;
        }

        CurrentState = GameState.PLACE_TILE;
    }

    public void TileHasBeenPlaced()
    {
        foreach (Transform child in tileParent)
        {
            child.GetChild(0).gameObject.SetActive(false);
        }

        CurrentState = GameState.ADVANCE_PLAYER;
    }
    
    public void PlayerHasAdvanced()
    {
        if(!(CurrentState == GameState.ADVANCE_PLAYER)) { return; }

        
        tileGen.GenerateTilePool();
        List<Tile> pathlol = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>().path;
        bool something = true;
        foreach(Tile tile in pathlol)
        {
            if (tile.tilePrefab.name.StartsWith("Unset Tile"))
            { 
                something = false;
            }
        }

        if (!something)
        {
            CurrentState = GameState.ROLL_DICE;
            SpawnDice();
        }
    }

    public void PlayerHasDied(Enemy enemy)
    {
        deathMessage = "You were slain by " + enemy.name + ". Would you like to Try Again?";
        UnityEngine.SceneManagement.SceneManager.LoadScene("scene_death");
        CurrentState = GameState.DEATH;
    }

    public void StartGame()
    {
        CurrentState = GameState.LOADING;

        // load main scene here
        UnityEngine.SceneManagement.SceneManager.LoadScene("scene_main");

        CurrentState = GameState.ROLL_DICE;
    }

    public void SpawnDice(){
        var dice = Instantiate(dicePrefab, diceSpawnLocation, Quaternion.identity);

        dice.transform.SetParent(diceParent);
    }

    public void GotoMenu()
    {
        CurrentState = GameState.LOADING;
        UnityEngine.SceneManagement.SceneManager.LoadScene("scene_menu");
        CurrentState = GameState.MENU;
    }
}
