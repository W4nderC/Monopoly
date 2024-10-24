using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LandUI : UIBehaviour
{
    [SerializeField] private Button returnBtn;

    private void Awake() 
    {
        returnBtn.onClick.AddListener(() => {
            GameManager.Instance.InvokeOnEndTurn();
        });
    }

    private void GameManager_OnTransaction(object sender, EventArgs e)
    {
        Show();
    }

    void Start()
    {
        GameManager.Instance.OnTransaction += GameManager_OnTransaction;    
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.CheckGameState(GameManager.GameState.Transaction))
        {
            Hide();
        }
    }

    private void OnDestroy() 
    {
        GameManager.Instance.OnTransaction -= GameManager_OnTransaction;  
    }
}
