using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Extensions;
public class ObstacleSpawner : MonoBehaviour
{
    public string ObstacleDirectory = "ObstaclePrefabs";
    public GameObject[] obstaclePool;
    GameObject[,] obstacleSpawns;
    // Initialized outside of this component
    public Grid obstacleGrid;
    Vector3Int currentCell;
    // Start is called before the first frame update
    void Start()
    {
        obstacleSpawns = new GameObject[100, 100];
        obstaclePool = Resources.LoadAll<GameObject>(ObstacleDirectory);
    }

    // Update is called once per frame
    void Update()
    {
        var newCell = obstacleGrid.WorldToCell(transform.position);
        if (currentCell.x > newCell.x)
        {
            // We do not want to cache obstacles for places we've already been, because we want to discourage using landmarks, and player is not likely to go backwards anyways.
            DeleteObstacles(new Vector2Int[] {
                new Vector2Int(currentCell.x + 1, currentCell.y + 1),
                new Vector2Int(currentCell.x + 1, currentCell.y),
                new Vector2Int(currentCell.x + 1, currentCell.y - 1) });
            SpawnObstacles(new Vector2Int[] {
                new Vector2Int(newCell.x - 1, newCell.y + 1),
                new Vector2Int(newCell.x - 1, newCell.y),
                new Vector2Int(newCell.x - 1, newCell.y - 1) });
        }
        else if (currentCell.x < newCell.x)
        {
            DeleteObstacles(new Vector2Int[] {
                new Vector2Int(currentCell.x - 1, currentCell.y + 1),
                new Vector2Int(currentCell.x - 1, currentCell.y),
                new Vector2Int(currentCell.x - 1, currentCell.y - 1) });
            SpawnObstacles(new Vector2Int[] {
                new Vector2Int(newCell.x + 1, newCell.y + 1),
                new Vector2Int(newCell.x + 1, newCell.y),
                new Vector2Int(newCell.x + 1, newCell.y - 1) });
        }

        if (currentCell.y > newCell.y)
        {
            // We do not want to cache obstacles for places we've already been, because we want to discourage using landmarks, and player is not likely to go backwards anyways.
            DeleteObstacles(new Vector2Int[] {
                new Vector2Int(currentCell.x + 1, currentCell.y + 1),
                new Vector2Int(currentCell.x, currentCell.y + 1),
                new Vector2Int(currentCell.x - 1, currentCell.y + 1) });
            SpawnObstacles(new Vector2Int[] {
                new Vector2Int(newCell.x + 1, newCell.y - 1),
                new Vector2Int(newCell.x, newCell.y - 1),
                new Vector2Int(newCell.x - 1, newCell.y - 1) });
        }
        else if (currentCell.y < newCell.y)
        {
            DeleteObstacles(new Vector2Int[] {
                new Vector2Int(currentCell.x + 1, currentCell.y - 1),
                new Vector2Int(currentCell.x, currentCell.y - 1),
                new Vector2Int(currentCell.x - 1, currentCell.y - 1) });
            SpawnObstacles(new Vector2Int[] {
                new Vector2Int(newCell.x + 1, newCell.y + 1),
                new Vector2Int(newCell.x, newCell.y + 1),
                new Vector2Int(newCell.x - 1, newCell.y + 1) });
        }
        currentCell = newCell;
    }
    private void DeleteObstacles(Vector2Int[] cells)
    {
        foreach (var cell in cells)
        {
            if (obstacleSpawns.In2DArrayBounds(cell))
            {
                var gobj = obstacleSpawns[cell.x, cell.y];
                if (gobj != null)
                {
                    Destroy(gobj);
                }
            }
        }
    }
    private void SpawnObstacles(Vector2Int[] cells)
    {
        foreach (var cell in cells)
        {
            if (obstacleSpawns.In2DArrayBounds(cell))
            {
                var gobj = obstacleSpawns[cell.x, cell.y];
                if (gobj == null)
                {
                    obstacleSpawns[cell.x, cell.y] = Instantiate(obstaclePool[(int)(Random.value * obstaclePool.Length)], obstacleGrid.CellToWorld(new Vector3Int(cell.x, cell.y, 0)), Quaternion.identity);
                }
            }
        }
    }
}
