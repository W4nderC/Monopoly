using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventUI : UIBehaviour
{
    [SerializeField] private Button returnBtn;

    private void Awake() 
    {
        returnBtn.onClick.AddListener(() => {
            GameManager.Instance.InvokeOnEndTurn();
            Hide();
        });
    }

    private void GameManager_OnEvent(object sender, EventArgs e)
    {
        ToggleUI();
    }

    void Start()
    {
        GameManager.Instance.OnEvent += GameManager_OnEvent;   
        Hide();
    }

    // void Update()
    // {
    //     if (!GameManager.Instance.CheckGameState(GameManager.GameState.Event))
    //     {
    //         Hide();
    //     }
    // }

    private void OnDestroy() 
    {
        GameManager.Instance.OnEvent -= GameManager_OnEvent;  
    }
}
