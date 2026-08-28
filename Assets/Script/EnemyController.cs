using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    [SerializeField] private int _HP = 10;
    [SerializeField] private int _attack = 5;
    private GridManager _gridManager;
    private Vector2Int _gridPosition;
    private PathFinder _pathFinder;
    private TurnManager _turnManager;
    private int _currentHP;
    private PlayerController _player;
    private EnemyManager _enemyManager;
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
        _pathFinder = new PathFinder(_gridManager);
        _enemyManager = FindFirstObjectByType<EnemyManager>();
    }

    public void EnemyTurn()
    {
        int distance =
        Mathf.Abs(_player.GridPosition.x - _gridPosition.x) +
        Mathf.Abs(_player.GridPosition.y - _gridPosition.y);

        int dx = Mathf.Abs(_player.GridPosition.x - _gridPosition.x);
        int dy = Mathf.Abs(_player.GridPosition.y - _gridPosition.y);

        if (dx  <= 1  && dy <= 1)
        {
            Attack();
            return;
        }

        if (distance <= _searchRange)
        {
            List<Vector2Int> path =
                _pathFinder.FindPath(
                    _gridPosition,
                    _player.GridPosition
                );

            if (path != null && path.Count > 0)
            {
                Vector2Int nextPos = path[0];
                Vector2Int direction = nextPos - _gridPosition;
                Move(direction);
            }
            else
            {
                RandomMove();
            }
            return;
        }
            RandomMove();
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

        // 今いる場所から敵を消す
        Cell currentCell = _gridManager.GetCell(
            _gridPosition.x,
            _gridPosition.y
        );

        currentCell.Enemy = null;


        // 移動先に敵を登録
        nextCell.Enemy = this;
        _gridPosition = nextPos;
        transform.position = new Vector3(_gridPosition.x, 0, _gridPosition.y);
    }

    private void Attack()
    {
        StartCoroutine(AttackAnimation());
    }

    private IEnumerator AttackAnimation()
    {
        Vector3 startPosition = transform.position;

        // プレイヤーの方向を計算
        Vector3 direction =
            new Vector3(
                _player.GridPosition.x - _gridPosition.x,
                0,
                _player.GridPosition.y - _gridPosition.y
            ).normalized;

        // プレイヤーの方向を向く
        transform.rotation = Quaternion.LookRotation(direction);

        // 少し前に出る
        Vector3 attackPosition =
            startPosition + direction * 0.3f;

        yield return transform
            .DOMove(attackPosition, 0.1f)
            .WaitForCompletion();

        // ダメージ
        _player.Damage(_attack);

        // 元の位置に戻る
        yield return transform
            .DOMove(startPosition, 0.1f)
            .WaitForCompletion();
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
        _enemyManager.RemoveEnemy(this);
        Destroy(gameObject);
    }
}
