﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnCheck : MonoBehaviour
{
    EnemySpawnManager spawnManager;
    public string myName;
    public bool bSpawnMe = true;

    // Start is called before the first frame update
    void Awake()
    {
        spawnManager = GameManager.instance.enemySpawnManager;
        spawnManager.enemies.Add(this);
        myName = gameObject.name;
        if(!GameData.bLoaded) spawnManager.enemySpawnDictionary[myName] = true;
    }

    // Update is called once per frame
    void Start()
    {
        CheckIfSpawned();
    }

    public void CheckIfSpawned()
    {
        bool bToBeSpawned = true;
        spawnManager.enemySpawnDictionary.TryGetValue(myName, out bToBeSpawned);
        Debug.LogError(myName + " " + bToBeSpawned);
        gameObject.SetActive(bToBeSpawned);
    }

    //private void OnDisable()
    //{
    //    Debug.Log("ran in disable" + bSpawnMe);
    //    spawnManager.enemySpawnDictionary[myName] = bSpawnMe;
    //}

    public bool GetValue()
    {
        return bSpawnMe;
    }

    private void OnDestroy()
    {
        
        spawnManager.enemySpawnDictionary[myName] = bSpawnMe;
    }
}
