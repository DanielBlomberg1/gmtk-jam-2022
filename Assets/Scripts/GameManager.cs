using System;
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

    public static Action<GameState> stateChange;

    //public Action OnStateChange stateChange;

    private GameState curState;

    public GameState CurrentState { 
        get { return curState; } 
        set
        {
            curState = value;
            Action<GameState> stateChange1 = stateChange;
            stateChange1(value);
        } 
    }

    void Start()
    {
        //CurrentState = GameState.MENU;
        CurrentState = GameState.ROLL_DICE;
        stateChange(GameState.ROLL_DICE);
        DontDestroyOnLoad(this.gameObject);
    }


    public void DiceHasBeenRolled(int rolledAmount)
    {
        if (!(CurrentState == GameState.ROLL_DICE)) { return; }

        CurrentState = GameState.PLACE_TILE;
        Debug.Log(gameObject.name + " finished rolling on face " + rolledAmount);
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
