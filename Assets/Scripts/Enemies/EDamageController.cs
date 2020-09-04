﻿using Enemies;
using UnityEngine;

public class EDamageController : MonoBehaviour, IDamageable
{
    StatHandler _enemyStats;
    AISystem aiSystem;

    [HideInInspector] public Guarding enemyGuard;

    private bool _isDamageDisabled = false;

    public void Init(StatHandler enemyStats) {
        _enemyStats = enemyStats;

        enemyGuard = this.gameObject.AddComponent<Guarding>();
        enemyGuard.Init(_enemyStats);
    }

    public void OnEntityDamage(float damage, GameObject attacker, bool unblockable)
    {
        if (!unblockable)
        {

            if (_isDamageDisabled) return;

            if (attacker.layer == LayerMask.NameToLayer("Player"))
            {
                if (enemyGuard.CheckIfEntityGuarding(damage)) return;

                aiSystem.ApplyHit(attacker);
            }
            else
            {
                Debug.Log(attacker.layer.ToString());
            }
        }
        else
        {
            aiSystem.ApplyHit(attacker);
        }
    }

    /* Summary: This disables the damage from this component.
     *          But can be only used when in a state that does
     *          not require it.*/
    //
    public void DisableDamage()
    {
        _isDamageDisabled = true;
    }

    public void EnableDamage()
    {
        _isDamageDisabled = false;
    }

    private void Start()
    {
        aiSystem = GetComponent<AISystem>();
    }
}