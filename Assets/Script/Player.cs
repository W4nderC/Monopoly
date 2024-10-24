using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpd;

    public Route currentRoute;
    private int routePos;
    public int steps;
    bool isMoving;
    private Vector3 nextPos;

    private void Awake() 
    {
        currentRoute = GameObject.Find("Route").GetComponent<Route>();
    }
    
    private void Start() 
    {
        GameManager.Instance.OnUnitMoving += GameManager_OnUnitMoving;
        transform.position = currentRoute.tilePos[0].position;
    }

    private void GameManager_OnUnitMoving(object sender, EventArgs e)
    {
        steps = DiceManager.Instance.GetDiceValue();

    }

    private void Update()
    {
        if (GameManager.Instance.CheckGameState(GameManager.GameState.UnitMoving))
        {
            // Debug.Log("Dice rolled "+ steps);
            if (steps > 0)
            {
                StartCoroutine(Move());
            }
            else
            {
                GameManager.Instance.InvokeOnStandbyPhase();
                if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit raycastHit, 20f))
                {

                    print(raycastHit.transform.name+ (" was hit"));
                    if (raycastHit.transform.gameObject.TryGetComponent(out ITiles tile))
                    {
                        print("it's worked");
                        tile.ChangeState();
                    }
                }
            }

        }


        

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


    private void OnDestroy() {
        GameManager.Instance.OnUnitMoving -= GameManager_OnUnitMoving;
    }
}
