using System;
using Unity.Netcode;
using UnityEngine;

public class GameVisualManager : NetworkBehaviour
{
    [SerializeField] private Transform playablePlayerPrefab1;
    
    [SerializeField] private Transform playablePlayerPrefab2;

    void Start()
    {
        GameManager.Instance.OnPlayerConnected += GameManager_OnPlayerConnected;
    }

    private void GameManager_OnPlayerConnected(object sender, GameManager.OnPlayerConnectedEventArgs e)
    {
        // SpawnObjectRpc(e.playerType);
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
        Transform spawnedPlayerTransform = Instantiate(prefab);
        // spawnedPlayerTransform.GetComponent<NetworkObject>().Spawn(true);
        
        spawnedPlayerTransform.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
        print("Owner client Id: ");
        
        // visualGameObjectList.Add(spawnedCrossTransform.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
