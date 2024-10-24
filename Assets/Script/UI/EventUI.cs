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
        });
    }

    private void GameManager_OnEvent(object sender, EventArgs e)
    {
        Show();
    }

    void Start()
    {
        GameManager.Instance.OnEvent += GameManager_OnEvent;   
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.CheckGameState(GameManager.GameState.Event))
        {
            Hide();
        }
    }

    private void OnDestroy() 
    {
        GameManager.Instance.OnEvent -= GameManager_OnEvent;  
    }
}
