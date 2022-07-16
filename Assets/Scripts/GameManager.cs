using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        ROLL_DICE,
        PLACE_TILE,
        ADVANCE_PLAYER,
        PLAYER_IN_COMBAT,
        PLAYER_HAS_WON,
        PAUSED,
        LOADING,
        MENU
    }

    public GameState CurrentState { get => CurrentState; set => CurrentState = value; }

    void Start()
    {
        CurrentState = GameState.MENU;
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    { 

    }
    

    public void DiceHasBeenRolled()
    {
        if (!(CurrentState == GameState.ROLL_DICE)) { return; }

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

        // some check to see if current tile is a combat tile;
        if(true == false)
        {
            CurrentState = GameState.PLAYER_IN_COMBAT;
        } 
        // some check to see if palyer has won
        else if(true == false)
        {
            CurrentState = GameState.PLAYER_HAS_WON;
        }
        else
        {
            CurrentState = GameState.PLACE_TILE;
        }
        
    }

    public void StartGame()
    {
        CurrentState = GameState.LOADING;

        // load main scene here

        CurrentState = GameState.ROLL_DICE;
    }
}
