﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public interface IPlayerController {
    string GetStringID();
    StatHandler GetPlayerStats();
}

//Replace to this
public interface IEntity
{
    string GetStringID();
    StatHandler GetPlayerStats();
}

public interface ISecretValidator
{
    int GetKillCount();
    int GetKeyCount();
}

public class PlayerController : MonoBehaviour, IEntity, ISecretValidator
{
    public string playerID = "defaultID1234";
    [HideInInspector] public StatHandler playerStats;
    [HideInInspector] public PlayerSettings playerSettings;
    [HideInInspector] public PlayerStateMachine stateMachine;
    [HideInInspector] public CameraControl cameraController;

    [HideInInspector] public int totalCollectedKeys = 0;
    [HideInInspector] public int totalKillCount = 0;

    //NOTE: Once object is spawned through code init through awake instead.
    void Awake() {
        //NOTE: This is only temporary to fix the camera referencing issues
        GameManager.instance.playerController = this;
    }

    //Summary: Sets initial state and initialise variables
    //
    public void Init(GameObject targetHolder)
    {
        GameManager gameManager = GameManager.instance;
        playerSettings = gameManager.gameSettings.playerSettings;
        EntityStatData playerData = playerSettings.playerStats;

        playerStats = new StatHandler();
        playerStats.Init(playerData);

        stateMachine = this.gameObject.AddComponent<PlayerStateMachine>();

        //This assigns the thirdperson camera targets to this player
        CinemachineFreeLook freeLockCamera = gameManager.thirdPersonViewCamera.GetComponent<CinemachineFreeLook>();
        //freeLockCamera.Follow = this.transform;
        //freeLockCamera.LookAt = this.transform;

        PCombatController combatController = this.GetComponent<PCombatController>();
        combatController.Init(playerStats);
        combatController.UnblockCombatInputs();

        //Sets up the player's camera controller
        cameraController = this.GetComponent<CameraControl>();
        cameraController.Init(this.transform);

        LockOnTargetManager lockOnManager = this.gameObject.GetComponentInChildren<LockOnTargetManager>();
        lockOnManager.targetHolder = targetHolder; //Sets the holder from the gamemanager into the LockOn script

        SetState<PNormalState>();
    }

    //Summary: Clears and Sets the new specified state for player.
    //
    public void SetState<T>() where T : PlayerState 
    {
        stateMachine.AddState<T>();
    }

    public string GetStringID() {
        return playerID;
    }

    public StatHandler GetPlayerStats()
    {
        return playerStats;
    }

    public int GetKillCount()
    {
        return totalKillCount;
    }

    public int GetKeyCount()
    {
        return totalCollectedKeys;
    }
}
