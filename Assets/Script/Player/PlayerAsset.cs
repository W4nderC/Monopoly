using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;
using Unity.Collections; 

public class PlayerAsset : NetworkBehaviour
{
    public NetworkVariable<FixedString32Bytes> playerName = new NetworkVariable<FixedString32Bytes>("Player", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> currentMoney = new NetworkVariable<float>(200, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    private void Start(){
        CurrencyManager.Instance.OnPurchaseLand += CurrencyManager_OnPurchaseLand;
        CurrencyManager.Instance.OnPayRent += CurrencyManager_OnPayRent;
        CurrencyManager.Instance.OnUpgradeLand += CurrencyManager_OnUpgradeLand;
        CurrencyManager.Instance.OnBuyBackLand += CurrencyManager_OnBuyBackLand;
    }

    public override void OnNetworkSpawn()
    {
        // if (IsOwner)
        // {
        //     SetPlayerInfoServerRpc("Player_" + OwnerClientId, 200);
        // }
    }

    // [Rpc(SendTo.Server)]
    // public void SetPlayerInfoServerRpc(string newName, int newMoney)
    // {
    //     playerName.Value = newName;
    //     currentMoney.Value = newMoney;
    // }

    private void CurrencyManager_OnBuyBackLand(object sender, EventArgs e)
    {
        if(!IsOwner || !IsLocalPlayerTurn()) return;
        BuyBackLandRpc();
    }

    [Rpc(SendTo.Server, RequireOwnership = false)]
    private void BuyBackLandRpc(){
        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit raycastHit, 20f))
        {
            if (raycastHit.transform.gameObject.TryGetComponent(out Land land))
            {
                switch(land.landLevel.Value){
                    case Land.LandLevel.LandLevel1: SpendMoney(land.landObjectSO.buyBackPrice1);
                    break;
                    case Land.LandLevel.LandLevel2: SpendMoney(land.landObjectSO.buyBackPrice2);
                    break;
                    case Land.LandLevel.LandLevel3: SpendMoney(land.landObjectSO.buyBackPrice3);
                    break;
                    case Land.LandLevel.LandLevel4: SpendMoney(land.landObjectSO.buyBackPrice4);
                    break;
                }
                
                land.landOwnerId.Value = (int) GameManager.Instance.GetCurrentPlayablePlayerType();
            }
        }
    }

    private void CurrencyManager_OnUpgradeLand(object sender, EventArgs e)
    {
        if(!IsOwner || !IsLocalPlayerTurn()) return;
        UpgradeLandRpc();
    }

    [Rpc(SendTo.Server, RequireOwnership = false)]
    private void UpgradeLandRpc(){
        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit raycastHit, 20f))
        {
            if (raycastHit.transform.gameObject.TryGetComponent(out Land land))
            {
                switch(land.landLevel.Value){
                    case Land.LandLevel.LandLevel1:
                        SpendMoney(land.landObjectSO.price + land.landObjectSO.upgradeCost);
                        land.landLevel.Value = Land.LandLevel.LandLevel2;
                    break;
                    case Land.LandLevel.LandLevel2:
                        SpendMoney(land.landObjectSO.price + land.landObjectSO.upgradeCost*2);
                        land.landLevel.Value = Land.LandLevel.LandLevel3;
                    break;
                    case Land.LandLevel.LandLevel3:
                        SpendMoney(land.landObjectSO.price + land.landObjectSO.upgradeCost*3);
                        land.landLevel.Value = Land.LandLevel.LandLevel4;
                    break;
                    case Land.LandLevel.LandLevel4:
                    break;
                }   
            }
        }
    }

    private void CurrencyManager_OnPayRent(object sender, EventArgs e)
    {
        if(IsOwner && IsLocalPlayerTurn()) {
            PayRentRpc();
        }
        GameManager.Instance.TriggerOnBuyBackLandRpc();
    }

    [Rpc(SendTo.Server, RequireOwnership = false)]
    private void PayRentRpc(){
        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit raycastHit, 20f))
        {
            if (raycastHit.transform.gameObject.TryGetComponent(out Land land))
            {
                switch(land.landLevel.Value){
                    case Land.LandLevel.LandLevel1:
                        SpendMoney(land.landObjectSO.rent1);
                        CurrencyManager.Instance.rentMoney.Value = land.landObjectSO.rent1;
                        print("pay rent amount: "+land.landObjectSO.rent1);
                    break;
                    case Land.LandLevel.LandLevel2:
                        SpendMoney(land.landObjectSO.rent2);
                        CurrencyManager.Instance.rentMoney.Value = land.landObjectSO.rent2;
                        print("pay rent amount: "+land.landObjectSO.rent2);
                    break;
                    case Land.LandLevel.LandLevel3:
                        SpendMoney(land.landObjectSO.rent3);
                        CurrencyManager.Instance.rentMoney.Value = land.landObjectSO.rent3;
                        print("pay rent amount: "+land.landObjectSO.rent3);
                    break;
                    case Land.LandLevel.LandLevel4:
                        SpendMoney(land.landObjectSO.rent4);
                        CurrencyManager.Instance.rentMoney.Value = land.landObjectSO.rent4;
                        print("pay rent amount: "+land.landObjectSO.rent4);
                    break;
                }   
                // Owner of the land receive the rent money from this player
                PlayerAsset ownerAsset = NetworkManager.Singleton.ConnectedClients[(ulong) land.landOwnerId.Value - 1].PlayerObject.GetComponent<PlayerAsset>();
                ownerAsset.ReceiveMoney(CurrencyManager.Instance.rentMoney.Value);  
            }
        }
    }

    // When player puchase a land, spend money, set isOwned = true and land owner name 
    private void CurrencyManager_OnPurchaseLand(object sender, EventArgs e)
    {
        if(!IsOwner || !IsLocalPlayerTurn())
        {
            return;
        }
        
        PurchaseLandRpc();
    }

    [Rpc(SendTo.Server, RequireOwnership = false)]
    private void PurchaseLandRpc(){
        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit raycastHit, 20f))
        {
            if (raycastHit.transform.gameObject.TryGetComponent(out Land land))
            {
                SpendMoney(land.landObjectSO.price);
                land.isOwned.Value = true;
                land.landOwnerId.Value = (int) GameManager.Instance.GetCurrentPlayablePlayerType();
            }
        }
    }

    private bool IsLocalPlayerTurn()
    {
        return GameManager.Instance.GetLocalPlayerType() == GameManager.Instance.GetCurrentPlayablePlayerType();
    }

    public void SpendMoney(float price)
    {
        currentMoney.Value -= price;
    }

    public void ReceiveMoney(float money)
    {
        currentMoney.Value += money;
    }
}
