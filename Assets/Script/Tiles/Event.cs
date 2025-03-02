using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour, ITiles
{
    [SerializeField] private EventObjectSO eventObjectSO;

    public void ChangeState()
    {
        GameManager.Instance.TriggerOnEventRpc();
        print("Event time");
    }

    public EventObjectSO GetLandScriptableObject(){
        return eventObjectSO;
    }
}
