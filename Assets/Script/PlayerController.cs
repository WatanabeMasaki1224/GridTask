using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int _attack = 3;
    [SerializeField] private int _HP = 10;
    [SerializeField] private GridManager _gridManager;
    private Vector2Int _gridPosition;

    public void Initialize(Vector2Int startPos, GridManager gridManager)
    {
        _gridManager = gridManager;
        _gridPosition = startPos;
        transform.position = new Vector3(startPos.x, 0, startPos.y);
    }

    private void Update()
    {
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
        transform.position = new Vector3(_gridPosition.x,0,_gridPosition.y);
    }
}
