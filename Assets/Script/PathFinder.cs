using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class PathFinder 
{
    private GridManager _gridManager;

    public PathFinder(GridManager gridManager)
    {
        _gridManager = gridManager;
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        //そのマスクに来る前のマスを記録
        Dictionary<Vector2Int,Vector2Int> previous = new Dictionary<Vector2Int,Vector2Int>();
        //探索済みかどうか
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        queue.Enqueue(start);
        visited.Add(start);
        Vector2Int[] directions =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right,
        };

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            //プレイヤーに到達
            if (current == goal)
            {
                return CreatePath(previous, start, goal);
            }

            foreach (Vector2Int direction in directions)
            {
                Vector2Int next = current + direction;

                if (visited.Contains(next))
                {
                    continue;
                }

                Cell cell = _gridManager.GetCell(next.x, next.y);

                if (cell == null)
                {
                    continue;
                }

                // 壁は通れない
                if (cell.Type == CellType.Wall)
                {
                    continue;
                }

                // 他の敵がいる場所は通れない
                if (cell.Enemy != null && next != goal)
                {
                    continue;
                }

                visited.Add(next);
                previous[next] = current;
                queue.Enqueue(next);
            }
        }
        return null;
    }

    private List<Vector2Int> CreatePath(Dictionary<Vector2Int, Vector2Int> previous,Vector2Int start,Vector2Int goal)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int current = goal;
        while (current != start)
        {
            path.Add(current);
            current = previous[current];
        }

        path.Reverse();
        return path;
    }
}
