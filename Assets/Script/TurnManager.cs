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
    public TurnState CurrentTurn {  get; private set; }

    private void Start()
    {
        CurrentTurn = TurnState.Player;
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
