using UnityEngine;

public class Cell 
{
    public Vector2Int Position;
    public CellType Type;

    public Cell(Vector2Int position, CellType type)
    {
        Position = position;
        Type = type;
    }
}
