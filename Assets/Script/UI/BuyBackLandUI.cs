using System;
using UnityEngine;
using UnityEngine.UI;

public class BuyBackLandUI : UIBehaviour
{
    [SerializeField] private Button buyBackBtn;
    [SerializeField] private Button returnBtn;

    private void Awake() 
    {
        returnBtn.onClick.AddListener(() => {
            GameManager.Instance.TriggerOnEndTurnRpc();
            Hide();
        });
        buyBackBtn.onClick.AddListener(() => {
            CurrencyManager.Instance.TriggerOnBuyBackLandRpc();
            GameManager.Instance.TriggerOnEndTurnRpc();
            Hide();
        });
    }
    
    private void Start() {
        GameManager.Instance.OnBuyBackLand += GameManager_OnBuyBackLand;

        Hide();
    }

    private void GameManager_OnBuyBackLand(object sender, EventArgs e)
    {
        ToggleUI();
    }
}
