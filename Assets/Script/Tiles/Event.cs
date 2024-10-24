using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour, ITiles
{
    public void ChangeState()
    {
        GameManager.Instance.InvokeOnEvent();
        print("Event time");
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
