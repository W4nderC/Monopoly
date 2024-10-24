using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public static DiceManager Instance { get; private set;}

    public event EventHandler OnRollDice;
    // public event EventHandler OnDiceResult;

    private int diceValue1; // default
    private int diceValue2;

    private void Awake() 
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        } 
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        GameManager.Instance.OnStandbyPhase += GameManager_OnStandbyPhase;
    }

    private void GameManager_OnStandbyPhase(object sender, EventArgs e)
    {
        ResetDiceValue();
    }

    void Update()
    {
        
    }

    public void InvokeOnRollDice()
    {
        OnRollDice?.Invoke(this, EventArgs.Empty);
    }

    // public void InvokeOnDiceResult()
    // {
    //     OnDiceResult?.Invoke(this, EventArgs.Empty);
    // }

    public int GetDiceValue()
    {
        print("Get dice value 1 "+diceValue1);
        print("Get dice value 2 "+diceValue2);
        return diceValue1 + diceValue2;
    }

    public int SetDiceValue(int diceValue)
    {
        // print("Set dice value "+diceValue);
        if (diceValue1 <= 0)
        {
            return diceValue1 = diceValue;
        }
        else
        {
            return diceValue2 = diceValue;
        }
    }

    public void ResetDiceValue()
    {
        diceValue1 = 0; 
        diceValue2 = 0;
        print("Get dice value 1 "+diceValue1);
        print("Get dice value 2 "+diceValue2);
        
    }

    public void Check2Dice()
    {
        // if both dice have value => change game state
        if (diceValue1 > 0 && diceValue2 > 0)
        {
            GameManager.Instance.InvokeOnUnitMoving();
        }
    }
}
