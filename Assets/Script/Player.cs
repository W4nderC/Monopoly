using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpd;

    private Map worldMap;
    private Transform moveToPos;
    private int numOfStep; // num of tiles player can move
    private int currentTileIndex = 0;
    

    private void Awake() {
        worldMap = GameObject.Find("Map").GetComponent<Map>();
    }
    
    private void Start() 
    {
       DiceManager.Instance.OnDiceResult +=  DiceManager_OnDiceResult;
       gameObject.transform.position = worldMap.tilePos[currentTileIndex].position;
    }

    private void DiceManager_OnDiceResult(object sender, EventArgs e)
    {
        numOfStep = DiceManager.Instance.GetDiceValue();
        int numOfTilesInMap = worldMap.tilePos.Length;

        for (int i = 0; i < numOfStep; i++)
        {
            currentTileIndex++;
            if (currentTileIndex >= numOfTilesInMap)
            {
                currentTileIndex = 0;
                gameObject.transform.position = Vector3.MoveTowards
                (
                    gameObject.transform.position, 
                    worldMap.tilePos[currentTileIndex].position,
                    200f
                );
                SetCurrentTileIndex(currentTileIndex);
                // print("current tile index "+currentTileIndex);
            }
            else
            {
                gameObject.transform.position = Vector3.MoveTowards
                (
                    gameObject.transform.position, 
                    worldMap.tilePos[currentTileIndex].position,
                    200
                );
                SetCurrentTileIndex(currentTileIndex);
                // print("current tile index "+currentTileIndex);
            }

        }
    }

    private int SetCurrentTileIndex(int tileIndex)
    {
        return this.currentTileIndex = tileIndex;
    }
}
