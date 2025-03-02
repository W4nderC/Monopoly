using System;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeLandUI : UIBehaviour
{
    [SerializeField] private Button returnBtn;
    [SerializeField] private Button upgradeBtn;
 
    private void Awake() 
    {
        returnBtn.onClick.AddListener(() => {
            GameManager.Instance.TriggerOnEndTurnRpc();
            Hide();
        });
        upgradeBtn.onClick.AddListener(() => {
            CurrencyManager.Instance.TriggerOnUpgradeLandRpc();
            GameManager.Instance.TriggerOnEndTurnRpc();
            Hide();
        });
    }

    void Start()
    {
        GameManager.Instance.OnUpgradeLand += GameManager_OnUpgradeLand;

        Hide();
    }

    private void GameManager_OnUpgradeLand(object sender, EventArgs e)
    {
        ToggleUI();
    }
}
