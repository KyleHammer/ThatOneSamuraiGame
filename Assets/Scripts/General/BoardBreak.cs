﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBreak : MonoBehaviour, IDamageable
{
    List<Rigidbody> boards = new List<Rigidbody>();
    public BoxCollider thisCol;

    private void Start()
    {
        Rigidbody[] children = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody child in children) boards.Add(child);
        thisCol = GetComponent<BoxCollider>();
    }

    public void DisableDamage()
    {
       
    }

    public void EnableDamage()
    {
       
    }

    public void OnEntityDamage(float damage, GameObject attacker)
    {
        thisCol.enabled = false;
        foreach (Rigidbody board in boards)
        {
            board.isKinematic = false;
            board.AddForce((board.transform.position - attacker.transform.position) * 2f, ForceMode.Impulse);
        }
    }

    
}