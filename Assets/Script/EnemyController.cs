using UnityEngine;


public class EnemyController : MonoBehaviour
{
    [SerializeField] private int _HP = 10;
    [SerializeField] private int _attack = 5;
    private GridManager _gridManager;
    private Vector2Int _gridPosition;
    private TurnManager _turnManager;
    private int _currentHP;
    private PlayerController _player;
    [SerializeField] private int _searchRange = 10;
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
        _player = FindFirstObjectByType<PlayerController>();
        _turnManager = FindFirstObjectByType<TurnManager>();
    }

    public void EnemyTurn()
    {
        int distance =
        Mathf.Abs(_player.GridPosition.x - _gridPosition.x) +
        Mathf.Abs(_player.GridPosition.y - _gridPosition.y);

        if (distance <= 1)
        {
            Attack();
        }

        if (distance <= _searchRange)
        {
            Chase();
        }
        else
        {
            RandomMove();
        }
    }

    private void Chase()
    {
        Vector2Int direction = Vector2Int.zero;

        int dx = _player.GridPosition.x - _gridPosition.x;
        int dy = _player.GridPosition.y - _gridPosition.y;

        if (Mathf.Abs(dx) > Mathf.Abs(dy))
        {
            direction = dx > 0 ? Vector2Int.right : Vector2Int.left;
        }
        else
        {
            direction = dy > 0 ? Vector2Int.up : Vector2Int.down;
        }

        Move(direction);
    }

    private void RandomMove()
    {
        Vector2Int[] directions =
        {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

        Move(directions[Random.Range(0, directions.Length)]);
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

        if(nextCell.Enemy != null)
        {
            return ;
        }

        if (nextPos == _player.GridPosition)
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

    private void Attack()
    {
        _player.Damage(_attack);
        _turnManager.ChangeTurn();
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
