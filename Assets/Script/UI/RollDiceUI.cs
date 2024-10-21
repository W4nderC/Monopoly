using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollDiceUI : MonoBehaviour
{
    [SerializeField] private Button rollDiceBtn;

    // Start is called before the first frame update
    void Start()
    {
        rollDiceBtn.onClick.AddListener(() => {
            DiceManager.Instance.InvokeOnRollDice();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
