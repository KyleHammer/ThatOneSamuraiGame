﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    public List<Transform> currentEnemies;

    public void AddEnemy(Transform enemy)
    {
        currentEnemies.Add(enemy);
    }

    public void RemoveEnemy(Transform enemy)
    {
        if (currentEnemies.Contains(enemy))
        {
            currentEnemies.Remove(enemy);
            currentEnemies.TrimExcess();
        }
    }
}
