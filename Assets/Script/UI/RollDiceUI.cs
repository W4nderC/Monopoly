using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollDiceUI : UIBehaviour
{
    [SerializeField] private Button rollDiceBtn;

    // Start is called before the first frame update
    void Start()
    {
        rollDiceBtn.onClick.AddListener(() => {
            DiceManager.Instance.InvokeOnRollDice();
        });

        GameManager.Instance.OnRollDice += GameManager_OnRollDice;
    }

    private void GameManager_OnRollDice(object sender, EventArgs e)
    {
        Show();
    }


    void Update()
    {
        if (!GameManager.Instance.CheckGameState(GameManager.GameState.RollDice))
        {
            Hide();
        }
    }
}
