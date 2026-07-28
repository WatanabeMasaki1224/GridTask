using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int _width = 10;
    public int _height = 10;
    private Cell[,] _cells;
    [SerializeField] GameObject _floorPrefab;
    [SerializeField] GameObject _wallPrefab;
    [SerializeField] GameObject _goalPrefab;
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] GameObject _enemyPregfab;
    [SerializeField]
    private string[] _mapData =
    {
        "#####",
        "#..."
    };

    private Vector2Int _playerSpawn;
    private List<Vector2Int> _enemySpawns = new();


    private void Start()
    {
        _height = _mapData.Length;
        _width = _mapData[0].Length;
        CreateGrid();
        CreateMap();
        SpawmPlayer();
        SpawnEnemy();   
    }

    private void CreateGrid()
    {
        _cells = new Cell[_width, _height];

        for(int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                CellType@type = CellType.Floor;
                switch (_mapData[y][x])
                {
                    case '#':
                        type = CellType.Wall;
                        break;

                    case '.':
                        type = CellType.Floor;
                        break;

                    case 'G':
                        type = CellType.Goal;
                        break;

                    case 'P':
                        type = CellType.Floor;
                        _playerSpawn = new Vector2Int(x, y);
                        break;

                    case 'E':
                        type = CellType.Floor;
                        _enemySpawns.Add(new Vector2Int(x,y));
                        break;
                }
                _cells[x, y]= new Cell(new Vector2Int(x, y),type);
            }

        }
    }

    private void CreateMap()
    {
        for(int x = 0; x < _width; x++)
        {
            for(int y = 0; y < _height;y++)
            {
                Vector3 pos = new Vector3(x, 0, y);

                switch (_cells[x, y].Type)
                {
                    case CellType.Floor:
                        Instantiate(_floorPrefab, pos, Quaternion.identity);
                        break;

                    case CellType.Wall:
                        Instantiate(_wallPrefab, pos, Quaternion.identity);
                        break;

                    case CellType.Goal:
                        Instantiate(_goalPrefab, pos, Quaternion.identity);
                        break;
                }
            }
        }
    }

    private void SpawmPlayer()
    {
        GameObject player = Instantiate(_playerPrefab);
        PlayerController controller = player.GetComponent<PlayerController>();
        controller.Initialize(_playerSpawn,this);
    }

    private void SpawnEnemy()
    {
        foreach(Vector2Int pos in _enemySpawns)
        {
            Instantiate(
                _enemyPregfab,
                new Vector3(pos.x,0,pos.y),
                Quaternion.identity);
        }
    }

    public Cell GetCell(int x, int y)
    {
        if(x < 0 || x >= _width || y < 0 || y >= _height)
        {
            return null;
        }

        return _cells[x, y];
    }
}
