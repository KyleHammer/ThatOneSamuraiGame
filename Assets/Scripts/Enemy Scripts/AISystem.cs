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
        
        //TODO: Use a scriptable object for stats instead
        [SerializeField] private float maxEnemyGuard;
        [SerializeField] private float currentGuard;
        
        //TODO: Remove later once more polish is done. These are just placeholders to test enemy states 
        public bool bIsIdle = false;
        public bool bIsLightAttacking = false;
        public bool bIsBlocking = false;
        public bool bIsApproaching = false;
        public Material enemyMaterial;

        //ENEMY MOVEMENT VARIABLES
        public PlayerInput playerInput; // Used to check if the player is moving
        public Transform targetTransform; // Target transform is the player
        public Transform enemyTransform; // Set in inspector
        public Tweener enemyMovementTweener; // Set by enemy states
        
        //Float offset added to the target location so the enemy doesn't clip into the floor 
        //because the player's origin point is on the floor
        public Vector3 floatOffset = Vector3.up * 2.0f;
        
        #endregion

        #region Basic Functions

        private void Start()
        {
            //TODO: Replace with a scriptable object enemy manager that knows where the player is
            playerInput = FindObjectOfType<PlayerInput>(); //Find the player input script
            targetTransform = playerInput.gameObject.transform; //Find the player
            
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
            
            // If a movement tweener is active and the player is moving...
            if (enemyMovementTweener != null && playerInput.bIsMoving)
            {
                // Adjust the target position
                enemyMovementTweener.ChangeEndValue(targetTransform.transform.position + floatOffset);
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
