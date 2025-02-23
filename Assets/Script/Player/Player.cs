using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] private float moveSpd;

    public Route currentRoute;
    private int routePos;
    public int steps;
    bool isMoving;
    private Vector3 nextPos;
    public string localPlayerName;
    
    private void Awake() 
    {
        currentRoute = GameObject.Find("Route").GetComponent<Route>();
    }
    
    // Single player code
    // private void Start() 
    // {
    //     GameManager.Instance.OnUnitMoving += GameManager_OnUnitMoving;
    //     GameManager.Instance.OnGameStarted += GameManager_OnGameStarted;
    //     GameManager.Instance.OnEndTurn += GameManager_OnEndTurn;
    //     transform.position = currentRoute.tilePos[0].position;
    // }

    public override void OnNetworkSpawn()
    {
        DiceManager.Instance.OnReceiveDiceValue += DiceManager_OnReceiveDiceValue;
        GameManager.Instance.OnGameStarted += GameManager_OnGameStarted;
        GameManager.Instance.OnChangeTurn += GameManager_OnChangeTurn;
        // GameManager.Instance.OnEndTurn += GameManager_OnEndTurn;
        transform.position = currentRoute.tilePos[0].position;
    }

    private void DiceManager_OnReceiveDiceValue(object sender, DiceManager.OnReceiveDiceValueEventArgs e)
    {
        steps = e.sumDiceValue;
    }

    private void GameManager_OnChangeTurn(object sender, EventArgs e)
    {
        localPlayerName = GameManager.Instance.GetLocalPlayerType().ToString();
        
    }

    private void GameManager_OnGameStarted(object sender, EventArgs e)
    {
        // print("LocalPlayerType: "+GameManager.Instance.GetLocalPlayerType());
        GameManager.Instance.ActivePlayerRpc(GameManager.Instance.GetLocalPlayerType());
    }

    private void Update()
    {
        if(!IsOwner || !IsLocalPlayerTurn())
        {
            return;
        }

        // PlayerMovementRpc();  
        if (GameManager.Instance.CheckGameState(GameManager.GameState.UnitMoving))
        {
            // Debug.Log("Dice rolled "+ steps);
            if (steps > 0)
            {
                TriggerStartMoveRpc();
            }
            else
            {
                // When player out of step, cast a raycast
                GameManager.Instance.TriggerOnStandbyPhaseRpc();
                if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit raycastHit, 20f))
                {
                    print(raycastHit.transform.name+ (" was hit"));
                    if (raycastHit.transform.gameObject.TryGetComponent(out ITiles tile))
                    {
                        // Change state when player token stop moving
                        tile.ChangeState();
                    }
                }
            }
        }      
    }

    private bool IsLocalPlayerTurn()
    {
        return GameManager.Instance.GetLocalPlayerType() == GameManager.Instance.GetCurrentPlayablePlayerType();
    }

    // [Rpc(SendTo.Server)]
    // public void PlayerMovementRpc()
    // {

    // }
    
    [Rpc(SendTo.ClientsAndHost)]
    public void TriggerStartMoveRpc(){
        StartCoroutine(Move());
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Vector3.down);
    }

    private IEnumerator Move()
    {
        if (isMoving)
        {
            yield break;
            
        }
        isMoving = true;

        while (steps > 0)
        {
            routePos++;
            // 
            if (routePos >= currentRoute.childTileList.Count)
            {
                routePos = 0;
                nextPos = currentRoute.childTileList[routePos].position;
            } 
            else
            {
                nextPos = currentRoute.childTileList[routePos ].position;
            }

            while (MoveToNextTile(nextPos))
            {
                yield return null;
            }

            yield return new WaitForSeconds(.1f);
            steps--;
            // routePos++;
        }

        isMoving = false;
    }
    
    private bool MoveToNextTile(Vector3 goal)
    {
        return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, 10f * Time.deltaTime));
    }


    // private void OnDestroy() {
    //     GameManager.Instance.OnUnitMoving -= GameManager_OnUnitMoving;
    // }
}
