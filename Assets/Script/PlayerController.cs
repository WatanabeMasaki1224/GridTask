using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int _attack = 3;
    [SerializeField] private int _HP = 10;
    private GridManager _gridManager;
    private TurnManager _turnManager;
    private GameManager _gameManager;
    private GameUI _gameUI;
    private Vector2Int _gridPosition;
    private Vector2Int _lookDirection = Vector2Int.down;
    private int _currentHP;
    private bool _isAttacking;
    public Vector2Int GridPosition => _gridPosition;

    public void Initialize(Vector2Int startPos)
    {
        _gridPosition = startPos;
        transform.position = new Vector3(startPos.x, 0, startPos.y);
    }

    private void Start()
    {
        _gridManager = FindFirstObjectByType<GridManager>();
        _turnManager = FindFirstObjectByType<TurnManager>();
        _gameManager = FindFirstObjectByType<GameManager>();
        _gameUI = FindFirstObjectByType<GameUI>();
        _currentHP = _HP;
        _gameUI.SetHP(_currentHP, _HP);
    }

    private void Update()
    {
        if (!_turnManager.PlayerTurn())
        {
            return;
        }

        Vector2Int move = Vector2Int.zero;

        if (Keyboard.current.wKey.wasPressedThisFrame)
            move = Vector2Int.up;
        else if (Keyboard.current.sKey.wasPressedThisFrame)
            move = Vector2Int.down;
        else if (Keyboard.current.aKey.wasPressedThisFrame)
            move = Vector2Int.left;
        else if (Keyboard.current.dKey.wasPressedThisFrame)
            move = Vector2Int.right;

        if (move != Vector2Int.zero)
        {
            Move(move);
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Attack();
        }
    }

    private void Move(Vector2Int direction)
    {
        Vector2Int nextPos = _gridPosition + direction;
        Cell cell = _gridManager.GetCell(nextPos.x, nextPos.y);

        if(cell == null)
        {
            return;     
        }
        if (cell.Type == CellType.Wall)
        {
            return;
        }

        if(cell.Enemy != null)
        {
            return;
        }

        _gridPosition = nextPos;
        Look(direction);
        transform.position = new Vector3(_gridPosition.x,1.0f,_gridPosition.y);

        if (cell.Type == CellType.Goal)
        {
            _turnManager.GameEnd();
            _gameManager.GameClear();
            return;
        }

        _turnManager.ChangeTurn();
    }

    private void  Look(Vector2Int direction)
    {
        _lookDirection = direction;

        transform.rotation = Quaternion.LookRotation(
            new Vector3(direction.x, 0, direction.y)
        );
    }

    private void Attack()
    {
        if (_isAttacking)
        {
            return;
        }

        _isAttacking = true;
        Vector2Int[] directions = {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right,
        new Vector2Int(1, 1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, 1),
        new Vector2Int(-1, -1)
    };

        List<EnemyController> targets = new();

        foreach (Vector2Int direction in directions)
        {
            Vector2Int targetPos = _gridPosition + direction;

            Cell cell = _gridManager.GetCell(targetPos.x, targetPos.y);

            if (cell == null)
                continue;

            if (cell.Enemy != null)
            {
                targets.Add(cell.Enemy);
            }
        }

        EnemyController target = null;

        if (targets.Count > 0)
        {
            target = targets[Random.Range(0, targets.Count)];
            Look(target.GridPosition - _gridPosition);
        }

        StartCoroutine(AttackAnimation(target));
    }

    private IEnumerator AttackAnimation(EnemyController target)
    {
        Vector3 startPosition = transform.position;
        Vector3 direction = transform.forward;
        Vector3 attackPosition =startPosition + direction * 0.3f;

        // 前に出る
        yield return transform
            .DOMove(attackPosition, 0.1f)
            .WaitForCompletion();

        // ダメージ
        if (target != null)
        {
            target.Damage(_attack);
        }
      

        // 元の位置に戻る
        yield return transform
            .DOMove(startPosition, 0.1f)
            .WaitForCompletion();

        _isAttacking = false;
        // アニメーションが終わってから次のターン
        _turnManager.ChangeTurn();
    }

    public void Damage(int damage)
    {
        _currentHP -= damage;
        _gameUI.SetHP(_currentHP ,_HP);
        if (_currentHP <= 0 )
        {
            Die();
        }
    }

    public void Die()
    {
        _turnManager.GameEnd();
        _gameManager.GameOver();
        transform.DOKill();
        Destroy(gameObject);
    }
}
