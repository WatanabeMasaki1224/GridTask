using UnityEngine;

public enum TurnState
{
    Player,
    Enemy
}

public class TurnManager : MonoBehaviour
{
    public EnemyManager EnemyManager;
    public TurnState CurrentTurn {  get; private set; }

    private void Start()
    {
        CurrentTurn = TurnState.Player;
    }

    public bool PlayerTurn()
    {
        return CurrentTurn == TurnState.Player;
    }

    public bool EnemyTurn()
    {
        return CurrentTurn == TurnState.Enemy;
    }

    public void ChangeTurn()
    {
        if(CurrentTurn == TurnState.Player)
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
}
