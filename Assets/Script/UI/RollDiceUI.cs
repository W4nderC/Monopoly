using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollDiceUI : UIBehaviour
{
    [SerializeField] private Button rollDiceBtn;

    private void Awake() {
        rollDiceBtn.onClick.AddListener(() => {
            DiceManager.Instance.InvokeOnRollDice();
        });

    }

    void Start()
    {
        GameManager.Instance.OnRollDice += GameManager_OnRollDice;
    }

    private void GameManager_OnRollDice(object sender, EventArgs e)
    {
        if(GameManager.Instance.GetLocalPlayerType() == GameManager.Instance.GetCurrentPlayablePlayerType()) {
            Show();
        }
        
    }


    void Update()
    {
        if 
        (!GameManager.Instance.CheckGameState(GameManager.GameState.RollDice)
        ||GameManager.Instance.GetLocalPlayerType() != GameManager.Instance.GetCurrentPlayablePlayerType())
        {
            Hide();
        }
    }
}
