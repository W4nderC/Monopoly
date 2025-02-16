using System;
using Unity.Netcode;
using UnityEngine;

public class GameVisualManager : NetworkBehaviour
{
    [SerializeField] private Transform playablePlayerPrefab1;
    
    [SerializeField] private Transform playablePlayerPrefab2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.OnPlayerConnected += GameManager_OnPlayerConnected;
    }

    private void GameManager_OnPlayerConnected(object sender, GameManager.OnPlayerConnectedEventArgs e)
    {
        SpawnObjectRpc(e.playerType);
    }

    [Rpc(SendTo.Server)]
    private void SpawnObjectRpc(GameManager.PlayerType playerType)
    {
        Transform prefab;
        switch(playerType)
        {
            default:
            case GameManager.PlayerType.Player1:
                prefab = playablePlayerPrefab1;
            break;
            case GameManager.PlayerType.Player2:
                prefab = playablePlayerPrefab2;
            break;
        }
        Transform spawnedCrossTransform = Instantiate(prefab);
        spawnedCrossTransform.GetComponent<NetworkObject>().Spawn(true);
        
        // visualGameObjectList.Add(spawnedCrossTransform.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
