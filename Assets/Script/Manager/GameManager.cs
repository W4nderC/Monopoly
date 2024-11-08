using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}

    public event EventHandler OnRollDice;
    public event EventHandler OnUnitMoving;
    public event EventHandler OnTransaction;
    public event EventHandler OnEvent;
    public event EventHandler OnEndTurn;
    public event EventHandler OnChangeTurn;
    public event EventHandler OnStandbyPhase;

    private float endTurnTimer ; //default
    private float changeTurnTimer ; //default
    [SerializeField] private float endTurnDuration = 1f; //default
    [SerializeField] private float changeTurnDuration = 1f; //default

    public enum GameState
    {
        RollDice,
        UnitMoving,
        Event,
        Transaction,
        EndTurn,
        ChangeTurn,
        GameOver,
        StanbyPhase
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

    private void Start() 
    {
        endTurnTimer = endTurnDuration;
        changeTurnTimer = changeTurnDuration;
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
                endTurnTimer -= Time.deltaTime;
                if (endTurnTimer <= 0)
                {
                    InvokeOnChangeTurn();
                    endTurnTimer = endTurnDuration;
                }
                break;
            case GameState.ChangeTurn:
                changeTurnTimer -= Time.deltaTime;
                if (changeTurnTimer <= 0)
                {
                    InvokeOnRollDice();
                    changeTurnTimer = changeTurnDuration;
                }
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

    public void InvokeOnUnitMoving()
    {
        SetGameState(GameState.UnitMoving);
        OnUnitMoving?.Invoke(this, EventArgs.Empty);
    }

    public void InvokeOnTransaction()
    {
        SetGameState(GameState.Transaction);
        OnTransaction?.Invoke(this, EventArgs.Empty);
    }

    public void InvokeOnEvent()
    {
        SetGameState(GameState.Event);
        OnEvent?.Invoke(this, EventArgs.Empty);
    }

    public void InvokeOnStandbyPhase()
    {
        SetGameState(GameState.StanbyPhase);
        OnStandbyPhase?.Invoke(this, EventArgs.Empty);
    }

    public void InvokeOnRollDice()
    {
        SetGameState(GameState.RollDice);
        OnRollDice?.Invoke(this, EventArgs.Empty);
    }

    public void InvokeOnEndTurn()
    {
        SetGameState(GameState.EndTurn);
        OnEndTurn?.Invoke(this, EventArgs.Empty);
    }

    public void InvokeOnChangeTurn()
    {
        SetGameState(GameState.ChangeTurn);
        OnChangeTurn?.Invoke(this, EventArgs.Empty);
    }


}
