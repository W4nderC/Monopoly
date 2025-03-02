using System;
using Unity.Netcode;
using UnityEngine;

public class CurrencyManager : NetworkBehaviour
{
    public static CurrencyManager Instance { get; private set;}

    public event EventHandler OnPurchaseLand;
    public event EventHandler OnPayRent;
    public event EventHandler OnReceiveRent;
    public event EventHandler OnUpgradeLand;
    public event EventHandler OnBuyBackLand;

    public NetworkVariable<float> rentMoney = new NetworkVariable<float>();
    
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

    [Rpc(SendTo.ClientsAndHost)]
    public void TriggerOnPurchaseLandRpc(){
        OnPurchaseLand?.Invoke(this, EventArgs.Empty);
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void TriggerOnPayRentRpc(){
        OnPayRent?.Invoke(this, EventArgs.Empty);
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void TriggerOnReceiveRentRpc(){
        OnReceiveRent?.Invoke(this, EventArgs.Empty);
    }
    
    [Rpc(SendTo.ClientsAndHost)]
    public void TriggerOnUpgradeLandRpc(){
        OnUpgradeLand?.Invoke(this, EventArgs.Empty);
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void TriggerOnBuyBackLandRpc(){
        OnBuyBackLand?.Invoke(this, EventArgs.Empty);
    }
}
