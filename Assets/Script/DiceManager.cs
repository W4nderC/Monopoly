using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public static DiceManager Instance { get; private set;}

    public event EventHandler OnRollDice;
    // public event EventHandler OnDiceResult;

    private int diceValue; // default

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
        // print("Get dice value "+diceValue);
        return diceValue;
    }

    public int SetDiceValue(int diceValue)
    {
        // print("Set dice value "+diceValue);
        return this.diceValue = diceValue;
    }
}
