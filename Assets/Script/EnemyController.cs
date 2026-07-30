using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GridManager _gridManager;
    private Vector2Int _gridPosition;

    public void Initialize(Vector2Int startPos)
    {
        _gridPosition = startPos;
        transform.position = new Vector3(startPos.x, 0, startPos.y);
    }

    private void Start()
    {
        _gridManager = FindFirstObjectByType<GridManager>();
    }

    public void EnemyTurn()
    {
        Vector2Int[] direction =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right,
        };

        Vector2Int move = direction[Random.Range(0, direction.Length)];
        Move(move);
    }

    private void Move(Vector2Int direction)
    {
        Vector2Int nextPos = _gridPosition + direction;
        Cell cell = _gridManager.GetCell(nextPos.x, nextPos.y);

        if (cell == null)
        {
            return;
        }
        if (cell.Type == CellType.Wall)
        {
            return;
        }
        _gridPosition = nextPos;
        transform.position = new Vector3(_gridPosition.x, 0, _gridPosition.y);
    }
}
