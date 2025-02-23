using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour, ITiles
{
    public void ChangeState()
    {
        GameManager.Instance.InvokeOnTransactionRpc();
        print("Transaction time");
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
