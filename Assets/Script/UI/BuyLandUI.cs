using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyLandUI : UIBehaviour
{
    [SerializeField] private Button purchaseBtn;
    [SerializeField] private Button returnBtn;

    private void Awake() 
    {
        returnBtn.onClick.AddListener(() => {
            GameManager.Instance.TriggerOnEndTurnRpc();
            Hide();
        });
        purchaseBtn.onClick.AddListener(() => {
            CurrencyManager.Instance.TriggerOnPurchaseLandRpc();
            GameManager.Instance.TriggerOnEndTurnRpc();
            Hide();
        });
    }

    void Start()
    {
        GameManager.Instance.OnBuyLand += GameManager_OnBuyLand;    
        Hide();
    }

    private void GameManager_OnBuyLand(object sender, EventArgs e)
    {
        ToggleUI();
    }

    private void OnDestroy() 
    {
        GameManager.Instance.OnBuyLand -= GameManager_OnBuyLand;  
    }
}
