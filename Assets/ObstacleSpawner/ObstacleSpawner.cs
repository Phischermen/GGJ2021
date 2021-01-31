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
            //DeleteObstacles(new Vector2Int[] {
            //    new Vector2Int(currentCell.x + 1, currentCell.y + 1),
            //    new Vector2Int(currentCell.x + 1, currentCell.y),
            //    new Vector2Int(currentCell.x + 1, currentCell.y - 1) });
            DeleteObstacles(GetDestroyCells(new Vector2Int(currentCell.x, currentCell.y), false, false));
            //SpawnObstacles(new Vector2Int[] {
            //    new Vector2Int(newCell.x - 1, newCell.y + 1),
            //    new Vector2Int(newCell.x - 1, newCell.y),
            //    new Vector2Int(newCell.x - 1, newCell.y - 1) });
            SpawnObstacles(GetSpawnCells(new Vector2Int(newCell.x, newCell.y), true, false));
        }
        else if (currentCell.x < newCell.x)
        {
            //DeleteObstacles(new Vector2Int[] {
            //    new Vector2Int(currentCell.x - 1, currentCell.y + 1),
            //    new Vector2Int(currentCell.x - 1, currentCell.y),
            //    new Vector2Int(currentCell.x - 1, currentCell.y - 1) });
            DeleteObstacles(GetDestroyCells(new Vector2Int(currentCell.x, currentCell.y), true, false));
            //SpawnObstacles(new Vector2Int[] {
            //    new Vector2Int(newCell.x + 1, newCell.y + 1),
            //    new Vector2Int(newCell.x + 1, newCell.y),
            //    new Vector2Int(newCell.x + 1, newCell.y - 1) });
            SpawnObstacles(GetSpawnCells(new Vector2Int(newCell.x, newCell.y), false, false));
        }

        if (currentCell.y > newCell.y)
        {
            // We do not want to cache obstacles for places we've already been, because we want to discourage using landmarks, and player is not likely to go backwards anyways.
            //DeleteObstacles(new Vector2Int[] {
            //    new Vector2Int(currentCell.x + 1, currentCell.y + 1),
            //    new Vector2Int(currentCell.x, currentCell.y + 1),
            //    new Vector2Int(currentCell.x - 1, currentCell.y + 1) });
            DeleteObstacles(GetDestroyCells(new Vector2Int(currentCell.x, currentCell.y), false, true));
            //SpawnObstacles(new Vector2Int[] {
            //    new Vector2Int(newCell.x + 1, newCell.y - 1),
            //    new Vector2Int(newCell.x, newCell.y - 1),
            //    new Vector2Int(newCell.x - 1, newCell.y - 1) });
            SpawnObstacles(GetSpawnCells(new Vector2Int(newCell.x, newCell.y), true, true));
        }
        else if (currentCell.y < newCell.y)
        {
            //DeleteObstacles(new Vector2Int[] {
            //    new Vector2Int(currentCell.x + 1, currentCell.y - 1),
            //    new Vector2Int(currentCell.x, currentCell.y - 1),
            //    new Vector2Int(currentCell.x - 1, currentCell.y - 1) });
            DeleteObstacles(GetDestroyCells(new Vector2Int(currentCell.x, currentCell.y), true, true));
            //SpawnObstacles(new Vector2Int[] {
            //    new Vector2Int(newCell.x + 1, newCell.y + 1),
            //    new Vector2Int(newCell.x, newCell.y + 1),
            //    new Vector2Int(newCell.x - 1, newCell.y + 1) });
            SpawnObstacles(GetSpawnCells(new Vector2Int(newCell.x, newCell.y), false, true));
        }
        currentCell = newCell;
    }
    private Vector2Int[] GetSpawnCells(Vector2Int originCell, bool flip, bool rotate)
    {
        var cells = new Vector2Int[8];
        var xFactor = (flip) ? -1 : 1;
        cells[0] = originCell + new Vector2Int(1 * xFactor, 1);
        cells[1] = originCell + new Vector2Int(1 * xFactor, 0);
        cells[2] = originCell + new Vector2Int(1 * xFactor, -1);
        cells[3] = originCell + new Vector2Int(2 * xFactor, -2);
        cells[4] = originCell + new Vector2Int(2 * xFactor, -1);
        cells[5] = originCell + new Vector2Int(2 * xFactor, 0);
        cells[6] = originCell + new Vector2Int(2 * xFactor, 1);
        cells[7] = originCell + new Vector2Int(2 * xFactor, 2);
        if (rotate)
        {
            for (var i = 0; i < cells.Length; i++)
            {
                var temp = cells[i].x;
                cells[i].x = cells[i].y;
                cells[i].y = temp;
            }
        }
        return cells;
    }
    private Vector2Int[] GetDestroyCells(Vector2Int originCell, bool flip, bool rotate)
    {
        var cells = new Vector2Int[5];
        var xFactor = (flip) ? -1 : 1;
        cells[0] = originCell + new Vector2Int(2 * xFactor, -2);
        cells[1] = originCell + new Vector2Int(2 * xFactor, -1);
        cells[2] = originCell + new Vector2Int(2 * xFactor, 0);
        cells[3] = originCell + new Vector2Int(2 * xFactor, 1);
        cells[4] = originCell + new Vector2Int(2 * xFactor, 2);
        if (rotate)
        {
            for (var i = 0; i < cells.Length; i++)
            {
                var temp = cells[i].x;
                cells[i].x = cells[i].y;
                cells[i].y = temp;
            }
        }
        return cells;
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
