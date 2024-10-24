using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiceValueUI : UIBehaviour
{
    [SerializeField] private TextMeshProUGUI diceTxt;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnUnitMoving += GameManager_OnUnitMoving;
        Hide();
    }

    private void GameManager_OnUnitMoving(object sender, EventArgs e)
    {
        Show();
        diceTxt.text = DiceManager.Instance.GetDiceValue().ToString();
    }

    private void OnDestroy() 
    {
        GameManager.Instance.OnUnitMoving -= GameManager_OnUnitMoving;
    }

}
