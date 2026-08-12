using UnityEditor.Rendering;
using UnityEngine;

public enum TurnState
{
    Player,
    Enemy
}

public class TurnManager : MonoBehaviour
{
    public EnemyManager EnemyManager;
    private bool _gameEnded = false;
    [SerializeField] private GameUI _gameUI;
    public TurnState CurrentTurn {  get; private set; }
    public int TurnCount { get; private set; } = 1;

    private void Start()
    {
        CurrentTurn = TurnState.Player;
        _gameUI.SetTurn(TurnCount);
    }

    public bool PlayerTurn()
    {
        return !_gameEnded && CurrentTurn == TurnState.Player;
    }

    public bool EnemyTurn()
    {
        return !_gameEnded && CurrentTurn == TurnState.Enemy;
    }

    public void ChangeTurn()
    {
        if (_gameEnded)
        {
            return;
        }

        if (CurrentTurn == TurnState.Player)
        {
            CurrentTurn = TurnState.Enemy;
            EnemyManager.EnemyTurn();
            CurrentTurn = TurnState.Player;
            TurnCount++;
            _gameUI.SetTurn(TurnCount);
        }
        else
        {
            CurrentTurn = TurnState.Player;
        }
    }

    public void GameEnd()
    {
        _gameEnded = true;
    }
}
