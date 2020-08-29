﻿using System;
using DG.Tweening;
using Enemy_Scripts.Enemy_States;
using UnityEngine;

namespace Enemy_Scripts
{
    // AI SYSTEM INFO
    // AISystem is responsible for receiving calls to tell the enemy what to perform. It should also
    // Be responsible for storing enemy data (i.e. Guard meter, remaining guard etc.) BUT
    // any enemy behaviours should be handled through the state machine
    public class AISystem : EnemyStateMachine
    {
        #region Fields and Properties

        //TODO: Remove later once more polish is done. These are just placeholders to test enemy states 
        public bool bIsIdle = false;
        public bool bIsLightAttacking = false;
        public bool bIsBlocking = false;
        public bool bIsApproaching = false;
        public Material enemyMaterial;

        //ENEMY SETTINGS [See EntityStatData for list of stats]
        public EnemySettings enemySettings; // Taken from EnemySettings Scriptable object in start

        //Float offset added to the target location so the enemy doesn't clip into the floor 
        //because the player's origin point is on the floor
        public Vector3 floatOffset = Vector3.up * 2.0f;
        
        #endregion

        #region Basic Functions

        private void Start()
        {
            // Grab the enemy settings from the Game Manager > Game Settings > Enemy Settings
            enemySettings = GameManager.instance.gameSettings.enemySettings;
            
            //Start the enemy in an idle state
            SetState(new IdleEnemyState(this));
        }
        
        private void Update()
        {
            //TODO: Remove these ifs later once more polish is done. These are just placeholders to test enemy states
            if (bIsIdle)
            {
                bIsIdle = false;
                OnIdle();
            }
            if (bIsLightAttacking)
            {
                bIsLightAttacking = false;
                OnLightAttack();
            }
            if (bIsBlocking)
            {
                bIsBlocking = false;
                OnBlock();
            }
            if (bIsApproaching)
            {
                bIsApproaching = false;
                OnApproachPlayer();
            }
        }

        #endregion

        // ENEMY STATE SWITCHING INFO
        // Any time an enemy gets a combat maneuver called, their state will switch
        // Upon switching states, they override the EnemyState Start() method to perform their action
        
        #region Enemy Combat Manuervers
        
        public void OnLightAttack()
        {
            SetState(new LightAttackEnemyState(this));
        }

        public void OnHeavyAttack()
        {
        
        }

        public void OnSpecialAttack()
        {
        
        }

        public void OnBlock()
        {
            SetState(new BlockEnemyState(this));
        }

        public void OnParry()
        {
        
        }

        public void OnDodge()
        {
        
        }
    
        #endregion

        #region Enemy Movement

        public void OnIdle()
        {
            SetState(new IdleEnemyState(this));
        }

        public void OnPatrol()
        {
        
        }

        public void OnApproachPlayer()
        {
            SetState(new ApproachPlayerEnemyState(this));
        }

        public void OnCirclePlayer()
        {
        
        }

        public void OnEnemyStun()
        {
            
        }

        public void OnEnemyRecovery()
        {
            
        }

        public void OnEnemyDeath()
        {
            
        }

        #endregion
    }
}
