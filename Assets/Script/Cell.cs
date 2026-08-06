using UnityEngine;

public class Cell 
{
    public Vector2Int Position { get; private set; }
    public CellType Type { get; private set; }
    public EnemyController Enemy { get; set; }

    public Cell(Vector2Int position, CellType type)
    {
        Position = position;
        Type = type;
    }
}
