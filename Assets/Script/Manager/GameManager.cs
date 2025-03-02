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
    public event EventHandler OnBuyLand;
    public event EventHandler OnUpgradeLand;
    
    public event EventHandler OnBuyBackLand;
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
        BuyLand,
        UpgradeLand,
        BuyBackLand,
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
        Player4
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


    private NetworkVariable<float> endTurnTimer = new NetworkVariable<float>(); //default
    private NetworkVariable<float> changeTurnTimer = new NetworkVariable<float>(); //default
    [SerializeField] private float endTurnDuration = 1f; //default
    [SerializeField] private float changeTurnDuration = 1f; //default
    private PlayerType localPlayerType;
    [SerializeField] private NetworkVariable<PlayerType> currentPlayablePlayerType = new NetworkVariable<PlayerType>();

// Code for single player
    // private void Start() 
    // {
    //     endTurnTimer.Value = endTurnDuration;
    //     changeTurnTimer.Value = changeTurnDuration;
    // }

    void Update()
    {
        if(!IsServer) return;
        

        switch(gameState)
        {
            case GameState.RollDice:
                break;
            case GameState.UnitMoving:
                break;
            case GameState.Event:
                break;
            case GameState.BuyLand:
                break;
            case GameState.EndTurn:
                endTurnTimer.Value -= Time.deltaTime;
                if (endTurnTimer.Value <= 0)
                {
                    TriggerOnChangeTurnRpc();
                    endTurnTimer.Value = endTurnDuration;
                }
                break;
            case GameState.ChangeTurn:
                changeTurnTimer.Value -= Time.deltaTime;
                if (changeTurnTimer.Value <= 0)
                {
                    TriggerOnRollDiceRpc();
                    changeTurnTimer.Value = changeTurnDuration;
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
            endTurnTimer.Value = endTurnDuration;
            changeTurnTimer.Value = changeTurnDuration;

            // this code run everytime client connected
            NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;    
        }

    }

    [Rpc(SendTo.Server)]
    public void TriggerPlayerConnectedRpc(PlayerType playerType)
    {
        // print(playerType+" call event time");
        OnPlayerConnected?.Invoke(this, new OnPlayerConnectedEventArgs {
            playerType = playerType
        });
    }

    private void NetworkManager_OnClientConnectedCallback(ulong obj)
    {
        if(NetworkManager.Singleton.ConnectedClientsList.Count == 2) {
            // if there are 2 client connected, start the game
            currentPlayablePlayerType.Value = PlayerType.Player1;
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
// print("currentPlayablePlayerType turn: "+currentPlayablePlayerType.Value);
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

    [Rpc(SendTo.ClientsAndHost)]
    public void TriggerOnUnitMovingRpc()
    {
        SetGameState(GameState.UnitMoving);
        OnUnitMoving?.Invoke(this, EventArgs.Empty);
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void TriggerOnBuyLandRpc()
    {
        SetGameState(GameState.BuyLand);
        OnBuyLand?.Invoke(this, EventArgs.Empty);
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void TriggerOnUpgradeLandRpc()
    {
        SetGameState(GameState.UpgradeLand);
        OnUpgradeLand?.Invoke(this, EventArgs.Empty);
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void TriggerOnBuyBackLandRpc()
    {
        SetGameState(GameState.BuyBackLand);
        OnBuyBackLand?.Invoke(this, EventArgs.Empty);
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void TriggerOnEventRpc()
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

    [Rpc(SendTo.ClientsAndHost)]
    public void TriggerOnEndTurnRpc()
    {
        SetGameState(GameState.EndTurn);
        OnEndTurn?.Invoke(this, EventArgs.Empty);
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void TriggerOnChangeTurnRpc()
    {
        SetGameState(GameState.ChangeTurn);
        OnChangeTurn?.Invoke(this, EventArgs.Empty);

        SwitchPlayerRpc(currentPlayablePlayerType.Value);
        // print("local player "+localPlayerType);
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
