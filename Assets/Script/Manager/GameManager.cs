using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}



    public enum GameState
    {
        RollDice,
        UnitMoving,
        Event,
        Transaction,
        EndTurn,
        ChangeTurn,
        GameOver
    }

    public GameState gameState ;

    private void Awake() 
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        } 
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        GameStateHandler();
    }

    private void GameStateHandler()
    {
        switch(gameState)
        {
            case GameState.RollDice:
                break;
            case GameState.UnitMoving:
                break;
            case GameState.Event:
                break;
            case GameState.Transaction:
                break;
            case GameState.EndTurn:
                break;
            case GameState.ChangeTurn:
                break;
            case GameState.GameOver:
                break;
        }
    }

    public void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
    }

    public bool CheckGameState (GameState gameState) 
    {
        return this.gameState == gameState;
    }


}
