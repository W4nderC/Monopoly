using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set;}

    public event EventHandler OnRollDice;
    public event EventHandler OnUnitMoving;
    public event EventHandler OnTransaction;
    public event EventHandler OnEvent;
    public event EventHandler OnEndTurn;
    public event EventHandler OnChangeTurn;
    public event EventHandler OnStandbyPhase;
    public event EventHandler OnGameStarted;
    public event EventHandler<OnPlayerConnectedEventArgs> OnPlayerConnected;
    public class OnPlayerConnectedEventArgs : EventArgs{
        public PlayerType playerType;
    }

    public enum GameState
    {
        RollDice,
        UnitMoving,
        Event,
        Transaction,
        EndTurn,
        ChangeTurn,
        GameOver,
        StanbyPhase,
    }

    public enum PlayerType {
        None,
        Player1,
        Player2,
        Player3,
        LastPlayer
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


    private float endTurnTimer ; //default
    private float changeTurnTimer ; //default
    [SerializeField] private float endTurnDuration = 1f; //default
    [SerializeField] private float changeTurnDuration = 1f; //default
    private PlayerType localPlayerType;
    private NetworkVariable<PlayerType> currentPlayablePlayerType = new NetworkVariable<PlayerType>();


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
                    TriggerOnRollDiceRpc();
                    changeTurnTimer = changeTurnDuration;
                }
                break;
            case GameState.GameOver:
                break;
        }
    }

    public override void OnNetworkSpawn()
    {
        if (NetworkManager.Singleton.LocalClientId == 0) //server
        {
            localPlayerType = PlayerType.Player1;
            // print("server call trigger!");
        } 
        else if(NetworkManager.Singleton.LocalClientId == 1)
        {
            localPlayerType = PlayerType.Player2; // Client
            // print("client call trigger! "+localPlayerType);
        }

        TriggerPlayerConnectedRpc(localPlayerType);
        if(IsServer) {
            
            // this code run everytime client connected
            NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;    
        }

    }

    [Rpc(SendTo.Server)]
    public void TriggerPlayerConnectedRpc(PlayerType playerType)
    {
        print(playerType+" call event time");
        OnPlayerConnected?.Invoke(this, new OnPlayerConnectedEventArgs {
            playerType = playerType
        });
    }

    private void NetworkManager_OnClientConnectedCallback(ulong obj)
    {
        if(NetworkManager.Singleton.ConnectedClientsList.Count == 2) {
            // if there are 2 client connected, start the game
            currentPlayablePlayerType.Value = PlayerType.Player1;
            print("current player type: "+currentPlayablePlayerType.Value);
            TriggerGameStartedRpc();
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void TriggerGameStartedRpc()
    {
        OnGameStarted?.Invoke(this, EventArgs.Empty);
    }

    [Rpc(SendTo.Server)]
    public void ActivePlayerRpc(PlayerType playerType)
    {
        if(playerType != currentPlayablePlayerType.Value) {
            // check is player turn, if not then do nothing
            // TriggerOnStandbyPhaseRpc();
            return;
        }
        
        TriggerOnRollDiceRpc();
    }

    [Rpc(SendTo.Server)]
    public void SwitchPlayerRpc(PlayerType playerType)
    {
        // change player turn
        switch (playerType) 
        {
            default:
            case PlayerType.Player1:
                currentPlayablePlayerType.Value = PlayerType.Player2;
                break;
            case PlayerType.Player2:
                currentPlayablePlayerType.Value = PlayerType.Player1;
                break;
            // case PlayerType.Player3:
            //     currentPlayablePlayerType.Value = PlayerType.LastPlayer;
            //     break;
            // case PlayerType.LastPlayer:
            //     currentPlayablePlayerType.Value = PlayerType.Player1;
            //     break;
        }

        // TestWinner();
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

    [Rpc(SendTo.ClientsAndHost)]
    public void TriggerOnStandbyPhaseRpc()
    {
        SetGameState(GameState.StanbyPhase);
        OnStandbyPhase?.Invoke(this, EventArgs.Empty);
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void TriggerOnRollDiceRpc()
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

    public PlayerType GetLocalPlayerType()
    {
        return localPlayerType;
    }

    public PlayerType GetCurrentPlayablePlayerType()
    {
        return currentPlayablePlayerType.Value;
    }
}
