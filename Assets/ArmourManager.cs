﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourManager : MonoBehaviour
{
    public ArmourPiece[] armourPieces;
    EnemyAudio enemyAudio;

    // Start is called before the first frame update
    void Start()
    {
        enemyAudio = GetComponent<EnemyAudio>();
        if (armourPieces.Length == 0) armourPieces = GetComponentsInChildren<ArmourPiece>();
    }

    public bool DestroyPiece()
    {
        for(int i = 0; i <= armourPieces.Length-1; i++)
        {
            if (!armourPieces[i].destroyed)
            {
                armourPieces[i].DropPiece();
                return true;
            }
        }
        return false;
    }
}