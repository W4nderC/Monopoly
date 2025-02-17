using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnUI : UIBehaviour
{


    void Start()
    {
        GameManager.Instance.OnEndTurn += GameManager_OnEndTurn;
        GameManager.Instance.OnChangeTurn += GameManager_OnChangeTurn;
        Hide();
    }

    private void GameManager_OnChangeTurn(object sender, EventArgs e)
    {
        Hide();
    }

    private void GameManager_OnEndTurn(object sender, EventArgs e)
    {
        ToggleUI();
    }

    // void Update()
    // {
    //     if(!GameManager.Instance.CheckGameState(GameManager.GameState.EndTurn))
    //     {
    //         Hide();
    //     }
    // }
}
