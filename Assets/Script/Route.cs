using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    public Transform[] tilePos;
    public List<Transform> childTileList = new List<Transform>();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        FillTiles();

        for (int i = 0; i < childTileList.Count; i++)
        {
            Vector3 currentPos = childTileList[i].position;
            if(i > 0) 
            {
                Vector3 prevPos = childTileList[i - 1].position;
                Gizmos.DrawLine(prevPos, currentPos);
            }
        }
    }

    private void FillTiles()
    {
        childTileList.Clear();

        // tilePos = GetComponentsInChildren<Transform>();

        foreach(Transform child in tilePos)
        {
            if(child != this.transform)
            {
                childTileList.Add(child);
            }
        }
    }
}
