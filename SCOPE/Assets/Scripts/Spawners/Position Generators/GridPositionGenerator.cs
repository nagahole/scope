using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPositionGenerator : MonoBehaviour, IPositionGenerator
{
    [SerializeField] private Vector2Int grids;
    [SerializeField] private Vector2 gridSizes;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private int differentFromLastX = 3;
    [SerializeField] private int maxTriesInNewPos = 4;

    private List<Vector2Int> prevCoords = new List<Vector2Int>();

    private void DrawGrid(int x, int y) { //from 0 -> 1.. 2.., 1 below row/column count
        Vector3 v1 = new Vector3(gridSizes.x * x, gridSizes.y * y, 0); //bottom left
        Vector3 v2 = v1 + new Vector3(gridSizes.x,0, 0); //bottom right
        Vector3 v3 = v2 + new Vector3(0, gridSizes.y, 0); //top right
        Vector3 v4 = v1 + new Vector3(0,gridSizes.y, 0); //top left

        Quaternion r = Quaternion.Euler(this.rotation);
        Vector3 pos = transform.position;

        v1 = r * v1 + pos;
        v2 = r * v2 + pos;
        v3 = r * v3 + pos;
        v4 = r * v4 + pos;


        Gizmos.DrawLine(v1, v2);
        Gizmos.DrawLine(v2, v3);
        Gizmos.DrawLine(v3, v4);
        Gizmos.DrawLine(v4, v1);
    }

    /// <summary>
    /// Very inefficient, but can't be bothered optimising - only for visualization anyways
    /// 
    /// Draws grids individually, instead of column and row lines
    /// 
    /// __
    ///|  |
    /// --    
    /// </summary>
    private void OnDrawGizmos() {
        for(int x = 0; x < grids.x; x++) {
            for(int y = 0; y < grids.y; y++) {
                DrawGrid(x, y);
            }
        }
    }

    public Vector3 GeneratePosition() {
        Vector2Int coords = new Vector2Int(Random.Range(0, grids.x), Random.Range(0, grids.y));
        for (int i = 0; i < maxTriesInNewPos; i++) {
            if (!prevCoords.Contains(coords))
                break;
            coords = new Vector2Int(Random.Range(0, grids.x), Random.Range(0, grids.y));
            
        }
        prevCoords.Insert(0, coords);
        if (prevCoords.Count > differentFromLastX)
            prevCoords.RemoveAt(prevCoords.Count - 1);
        
        return Quaternion.Euler(rotation) * new Vector3((gridSizes.x / 2f) + (coords.x * gridSizes.x),
            (gridSizes.y / 2f) + (coords.y * gridSizes.y),
            0) + transform.position;
    }
}
