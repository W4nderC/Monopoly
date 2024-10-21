using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiceValueUI : UIMonoBehavior
{
    [SerializeField] private TextMeshProUGUI diceTxt;

    // Start is called before the first frame update
    void Start()
    {
        DiceManager.Instance.OnDiceResult += DiceManager_OnDiceResult;
        Hide();
    }

    private void DiceManager_OnDiceResult(object sender, EventArgs e)
    {
        Show();
        diceTxt.text = DiceManager.Instance.GetDiceValue().ToString();
    }

    private void OnDestroy() 
    {
        DiceManager.Instance.OnDiceResult -=  DiceManager_OnDiceResult;
    }

}
