using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int _attack = 3;
    [SerializeField] private int _HP = 10;
    private GridManager _gridManager;
    private TurnManager _turnManager;
    private Vector2Int _gridPosition;
    private Vector2Int _lookDirection = Vector2Int.down;

    public void Initialize(Vector2Int startPos)
    {
        _gridPosition = startPos;
        transform.position = new Vector3(startPos.x, 0, startPos.y);
    }

    private void Start()
    {
        _gridManager = FindFirstObjectByType<GridManager>();
        _turnManager = FindFirstObjectByType<TurnManager>();
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
        Debug.Log("Move");
        Vector2Int nextPos = _gridPosition + direction;
        Cell cell = _gridManager.GetCell(nextPos.x, nextPos.y);

        if(cell == null)
        {
            Debug.Log("null");
            return;     
        }
        if (cell.Type == CellType.Wall)
        {
            Debug.Log("Wall");
            return;
        }
        Debug.Log("nextpos");
        _gridPosition = nextPos;
        _lookDirection = direction;
        transform.position = new Vector3(_gridPosition.x,0,_gridPosition.y);

        _turnManager.ChangeTurn();
    }

    private void  Look()
    {

    }

    private void Attack()
    {

        Vector2Int targetPos = _gridPosition + _lookDirection;
        Cell targetCell = _gridManager.GetCell(targetPos.x, targetPos.y);

        if (targetCell != null && targetCell.Occupant != null)
        {
            targetCell.Occupant.Damage(_attack);
        }

        _turnManager.ChangeTurn();
    }
}
