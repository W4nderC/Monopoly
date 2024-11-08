using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayerAsset : MonoBehaviour
{
    [SerializeField] private float currentMoney;

    public float spendMoney(float price)
    {
        return currentMoney -= price;
    }

    public float receiveMoney(float money)
    {
        return currentMoney += money;
    }
}
