using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private List<EnemyController> _enemies = new();


    public void AddEnemy(EnemyController enemy)
    {
        _enemies.Add(enemy);
    }


    public void EnemyTurn()
    {
        foreach (EnemyController enemy in _enemies)
        {
            enemy.EnemyTurn();
        }
    }
}
