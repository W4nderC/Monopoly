using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnUI : UIBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnEndTurn += GameManager_OnEndTurn;
        Hide();
    }

    private void GameManager_OnEndTurn(object sender, EventArgs e)
    {
        Show();
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.Instance.CheckGameState(GameManager.GameState.EndTurn))
        {
            Hide();
        }
    }
}
