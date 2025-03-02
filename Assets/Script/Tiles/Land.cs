using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Land : NetworkBehaviour, ITiles
{
    public enum LandLevel {
        LandLevel1,
        LandLevel2,
        LandLevel3,
        LandLevel4,
    }
    public NetworkVariable<LandLevel> landLevel;

    public LandObjectSO landObjectSO;
    public NetworkVariable<bool> isOwned = new NetworkVariable<bool>();
    public NetworkVariable<int> landOwnerId = new NetworkVariable<int>();


    private void Awake() {
        isOwned.Value = false;

    }

    public bool IsThisLandNotOwnedByPlayer(){
        return isOwned.Value && (int) GameManager.Instance.GetLocalPlayerType() != landOwnerId.Value;
    }

    public bool IsThisLandOwnedByPlayer(){
        return (int) GameManager.Instance.GetLocalPlayerType() == landOwnerId.Value;
    }

    public void ChangeState()
    {
        if(isOwned.Value == false){
            GameManager.Instance.TriggerOnBuyLandRpc();
            print("Buy land time");
        } 
        else if(isOwned.Value && IsThisLandOwnedByPlayer()){
            GameManager.Instance.TriggerOnUpgradeLandRpc();
            print("Upgrade land time");
        }
        else{
            GameManager.Instance.TriggerOnEndTurnRpc();
        }
        
    }

    public LandObjectSO GetLandScriptableObject(){
        return landObjectSO;
    }
}
