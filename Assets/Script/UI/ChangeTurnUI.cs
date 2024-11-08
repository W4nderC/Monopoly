using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTurnUI : UIBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnChangeTurn += GameManager_OnChangeTurn;
        Hide();
    }

    private void GameManager_OnChangeTurn(object sender, EventArgs e)
    {
        Show();
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.Instance.CheckGameState(GameManager.GameState.ChangeTurn)) 
        {
            Hide();
        }
    }
}
