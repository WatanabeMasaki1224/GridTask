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
        _enemies.RemoveAll(enemy => enemy == null);
        foreach (EnemyController enemy in _enemies)
        {
            enemy.EnemyTurn();
        }
    }

    public void RemoveEnemy(EnemyController enemy)
    {
        _enemies.Remove(enemy);
    }
}
