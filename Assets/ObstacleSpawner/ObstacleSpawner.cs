using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Extensions;
public class ObstacleSpawner : MonoBehaviour
{
    public string EasyObstacleDirectory = "ObstaclePrefabs/Easy Levels";
    public string MediumObstacleDirectory = "ObstaclePrefabs/Medium Levels";
    public string HardObstacleDirectory = "ObstaclePrefabs/Hard Levels";
    public float[] difficultyThresholds;
    public List<GameObject[]> obstaclePool = new List<GameObject[]>();
    GameObject[,] obstacleSpawns;
    // Initialized outside of this component
    public Grid obstacleGrid;
    Vector3Int currentCell;
    //public Vector3 LighthouseLocation;
    //float LighthouseNoSpawnRadius = 20f;
    public NoSpawn[] NoSpawns = new NoSpawn[2]; //Size 2 to accomadate boat and lighthouse.
    public struct NoSpawn
    {
        public Transform gameObjectLocation;
        public float noSpawnRadius;
    }
    // Start is called before the first frame update
    void Start()
    {
        obstacleSpawns = new GameObject[100, 100];
        obstaclePool.Add(Resources.LoadAll<GameObject>(HardObstacleDirectory));
        obstaclePool.Add(Resources.LoadAll<GameObject>(MediumObstacleDirectory));
        obstaclePool.Add(Resources.LoadAll<GameObject>(EasyObstacleDirectory));
    }

    // Update is called once per frame
    void Update()
    {
        var newCell = obstacleGrid.WorldToCell(transform.position);
        if (currentCell.x > newCell.x)
        {
            // We do not want to cache obstacles for places we've already been, because we want to discourage using landmarks, and player is not likely to go backwards anyways.
            DeleteObstacles(GetDestroyCells(new Vector2Int(currentCell.x, currentCell.y), false, false));
            SpawnObstacles(GetSpawnCells(new Vector2Int(newCell.x, newCell.y), true, false));
        }
        else if (currentCell.x < newCell.x)
        {
            DeleteObstacles(GetDestroyCells(new Vector2Int(currentCell.x, currentCell.y), true, false));
            SpawnObstacles(GetSpawnCells(new Vector2Int(newCell.x, newCell.y), false, false));
        }

        if (currentCell.y > newCell.y)
        {
            DeleteObstacles(GetDestroyCells(new Vector2Int(currentCell.x, currentCell.y), false, true));
            SpawnObstacles(GetSpawnCells(new Vector2Int(newCell.x, newCell.y), true, true));
        }
        else if (currentCell.y < newCell.y)
        {
            DeleteObstacles(GetDestroyCells(new Vector2Int(currentCell.x, currentCell.y), true, true));
            SpawnObstacles(GetSpawnCells(new Vector2Int(newCell.x, newCell.y), false, true));
        }
        currentCell = newCell;
    }
    private Vector2Int[] GetSpawnCells(Vector2Int originCell, bool flip, bool rotate)
    {
        var cells = new Vector2Int[5];
        var xFactor = (flip) ? -1 : 1;
        cells[0] = originCell + ((rotate) ? new Vector2Int(-2, 2 * xFactor) : new Vector2Int(2 * xFactor, -2));
        cells[1] = originCell + ((rotate) ? new Vector2Int(-1, 2 * xFactor) : new Vector2Int(2 * xFactor, -1));
        cells[2] = originCell + ((rotate) ? new Vector2Int(0, 2 * xFactor) : new Vector2Int(2 * xFactor, 0));
        cells[3] = originCell + ((rotate) ? new Vector2Int(1, 2 * xFactor) : new Vector2Int(2 * xFactor, 1));
        cells[4] = originCell + ((rotate) ? new Vector2Int(2, 2 * xFactor) : new Vector2Int(2 * xFactor, 2));
        return cells;
    }
    private Vector2Int[] GetDestroyCells(Vector2Int originCell, bool flip, bool rotate)
    {
        var cells = new Vector2Int[5];
        var xFactor = (flip) ? -1 : 1;
        cells[0] = originCell + ((rotate) ? new Vector2Int(-2, 2 * xFactor) : new Vector2Int(2 * xFactor, -2));
        cells[1] = originCell + ((rotate) ? new Vector2Int(-1, 2 * xFactor) : new Vector2Int(2 * xFactor, -1));
        cells[2] = originCell + ((rotate) ? new Vector2Int(0, 2 * xFactor) : new Vector2Int(2 * xFactor, 0));
        cells[3] = originCell + ((rotate) ? new Vector2Int(1, 2 * xFactor) : new Vector2Int(2 * xFactor, 1));
        cells[4] = originCell + ((rotate) ? new Vector2Int(2, 2 * xFactor) : new Vector2Int(2 * xFactor, 2));
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
            var worldPosition = obstacleGrid.CellToWorld(new Vector3Int(cell.x, cell.y, 0));

            if (obstacleSpawns.In2DArrayBounds(cell))
            {
                foreach (var noSpawn in NoSpawns)
                {
                    if (Vector3.Distance(worldPosition, noSpawn.gameObjectLocation.position) < noSpawn.noSpawnRadius)
                    {
                        goto EndOfFirstForEach;
                    }
                }
                var gobj = obstacleSpawns[cell.x, cell.y];
                if (gobj == null)
                {
                    // Get appropriate obstacle pool
                    var i = 0;
                    var d = Vector3.Distance(transform.position, NoSpawns[0].gameObjectLocation.position);
                    foreach (var t in difficultyThresholds)
                    {
                        if (d < t) break;
                        i += 1;
                    }
                    //Debug.Log(d);
                    //Debug.Log(i);
                    GameObject[] pool = obstaclePool[i];
                    // Instantiate object
                    var ob = obstacleSpawns[cell.x, cell.y] = Instantiate(pool[(int)(Random.value * pool.Length)], worldPosition, Quaternion.identity);
                    for (i = 0; i < ob.transform.childCount; i++)
                    {
                        var child = ob.transform.GetChild(i);
                        if (child.transform.position.z != 0f)
                        {
                            Debug.LogError(ob.name + " has children that are not zeroed along z axis.");
                            child.position = new Vector3(child.position.x, child.position.y, 0f);
                        }
                    }
                }
            }
        EndOfFirstForEach:;
        }
    }
}
