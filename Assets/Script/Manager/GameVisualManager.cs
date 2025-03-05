using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public class GameVisualManager : NetworkBehaviour
{
    [SerializeField] private Transform playablePlayerPrefab1;
    
    [SerializeField] private Transform playablePlayerPrefab2;

    void Start()
    {
        // GameManager.Instance.OnPlayerConnected += GameManager_OnPlayerConnected;

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        UpdatePlayerList();
    }

    // private void GameManager_OnPlayerConnected(object sender, GameManager.OnPlayerConnectedEventArgs e)
    // {
    //     // SpawnObjectRpc(e.playerType);
    // }

    // [Rpc(SendTo.Server)]
    // private void SpawnObjectRpc(GameManager.PlayerType playerType)
    // {
    //     Transform prefab;
    //     switch(playerType)
    //     {
    //         default:
    //         case GameManager.PlayerType.Player1:
    //             prefab = playablePlayerPrefab1;
    //         break;
    //         case GameManager.PlayerType.Player2:
    //             prefab = playablePlayerPrefab2;
    //         break;
    //     }
    //     Transform spawnedPlayerTransform = Instantiate(prefab);
    //     spawnedPlayerTransform.GetComponent<NetworkObject>().Spawn(true);
        
    //     // spawnedPlayerTransform.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
        
    //     // visualGameObjectList.Add(spawnedCrossTransform.gameObject);
    // }

    public TextMeshProUGUI playerTextPrefab;  // Prefab UI cho mỗi người chơi
    public Transform playerListParent;  // Panel chứa danh sách người chơi

    private Dictionary<ulong, TextMeshProUGUI> playerDisplays = new Dictionary<ulong, TextMeshProUGUI>();

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"Player {clientId} connected!");
        Invoke(nameof(UpdatePlayerList), 0.5f); // Delay để đảm bảo PlayerInfo đã spawn
    }

    private void OnClientDisconnected(ulong clientId)
    {
        if (playerDisplays.ContainsKey(clientId))
        {
            Destroy(playerDisplays[clientId].gameObject);
            playerDisplays.Remove(clientId);
        }
    }
    private void UpdatePlayerList()
    {
        // ✅ Corrected: Use FindObjectsByType instead of FindObjectsOfType
        PlayerAsset[] players = UnityEngine.Object.FindObjectsByType<PlayerAsset>(FindObjectsSortMode.None);

        foreach (var player in players)
        {
            ulong clientId = player.OwnerClientId;
            if (!playerDisplays.ContainsKey(clientId))
            {
                CreatePlayerUI(clientId, player);
            }
        }
    }

    private void CreatePlayerUI(ulong clientId, PlayerAsset playerInfo)
    {
        TextMeshProUGUI playerText = Instantiate(playerTextPrefab, playerListParent);
        playerDisplays[clientId] = playerText;

        playerInfo.playerName.OnValueChanged += (prev, curr) => { UpdateText(clientId, playerInfo); };
        playerInfo.currentMoney.OnValueChanged += (prev, curr) => { UpdateText(clientId, playerInfo); };

        UpdateText(clientId, playerInfo);
    }

    private void UpdateText(ulong clientId, PlayerAsset playerInfo)
    {
        if (playerDisplays.ContainsKey(clientId))
        {
            playerDisplays[clientId].text = $"{playerInfo.playerName.Value} - ${playerInfo.currentMoney.Value}";
        }
    }
}
