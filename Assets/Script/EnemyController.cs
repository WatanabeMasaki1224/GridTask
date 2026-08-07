using UnityEngine;


public class EnemyController : MonoBehaviour
{
    [SerializeField] private int _HP = 10;
    [SerializeField] private int _attack = 5;
    private GridManager _gridManager;
    private Vector2Int _gridPosition;
    private int _currentHP;
    public Vector2Int GridPosition => _gridPosition;

    public void Initialize(Vector2Int startPos)
    {
        _gridPosition = startPos;
        transform.position = new Vector3(startPos.x, 0, startPos.y);
    }

    private void Start()
    {
        _gridManager = FindFirstObjectByType<GridManager>();
        _currentHP = _HP;
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
        Cell nextCell = _gridManager.GetCell(nextPos.x, nextPos.y);

        if (nextCell == null)
        {
            return;
        }
        if (nextCell.Type == CellType.Wall)
        {
            return;
        }

        // ç°Ç¢ÇÈèÍèäÇ©ÇÁìGÇè¡Ç∑
        Cell currentCell = _gridManager.GetCell(
            _gridPosition.x,
            _gridPosition.y
        );

        currentCell.Enemy = null;


        // à⁄ìÆêÊÇ…ìGÇìoò^
        nextCell.Enemy = this;
        _gridPosition = nextPos;
        transform.position = new Vector3(_gridPosition.x, 0, _gridPosition.y);
    }

    public void Damage(int damage)
    {
        _currentHP -= damage;
        Debug.Log(_currentHP);

        if (_currentHP <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        Cell cell = _gridManager.GetCell(
            _gridPosition.x,
            _gridPosition.y
        );

        cell.Enemy = null;

        Destroy(gameObject);
    }
}
