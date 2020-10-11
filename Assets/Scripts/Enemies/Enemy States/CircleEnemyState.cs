﻿using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Enemies.Enemy_States
{
    public class CircleEnemyState : EnemyState
    {
        private Vector3 _target;
        private float _longRange;
        private float _shortRange;
        
        private bool _bIsThreatened = false;

        //Class constructor
        public CircleEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            // For the enemy tracker, restart the impatience countdown
            // See enemy tracker for more details
            AISystem.enemyTracker.StartImpatienceCountdown();
            
            // Stop the navMeshAgent from tracking
            AISystem.navMeshAgent.isStopped = true;
            
            // Cache the range value so we're not always getting it in the tick function
            _longRange = AISystem.enemySettings.longRange;
            _shortRange = AISystem.enemySettings.shortRange;

            // Pick a strafe direction and trigger movement animator
            PickStrafeDirection();

            yield break;
        }
        
        public override void Tick()
        {
            // Get the true target point (float offset is added to get a more accurate player-enemy target point)
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            
            // Set the rotation of the enemy
            PositionTowardsTarget(AISystem.transform, _target);
            
            // If player approaches circling enemy, trigger threatened bool and end state
            if(InRange(AISystem.transform.position, _target, _shortRange))
            {
                _bIsThreatened = true;
                EndState();
            }
            // If player runs from circling enemy, trigger end state
            else if (!InRange(AISystem.transform.position, _target, _longRange))
            {
                EndState();
            }
        }

        public override void EndState()
        {
            // Stop the impatience cooldown when state end is called
            AISystem.enemyTracker.StopImpatienceCountdown();
            
            // Reset animation variables
            Animator.SetFloat("MovementX", 0.0f);

            // If threatened, do a threatened response (i.e. if the player is close)
            // Else approach the player again (i.e. if the player is far)
            if(_bIsThreatened)
                PickThreatenedResponse();
            else
                AISystem.OnApproachPlayer();
        }

        // Set the strafe direction
        private void PickStrafeDirection()
        {
            // Random.Range is non-inclusive for it's max value for ints
            if (Random.Range(0, 2) == 0)
            {
                Animator.SetFloat("MovementX", -1.0f);
            }
            else
            {
                Animator.SetFloat("MovementX", 1.0f);
            }
            
            Animator.SetTrigger("TriggerMovement");
        }
        
        // Pick a random action to perform when the player approaches the enemy
        private void PickThreatenedResponse()
        {
            // Reset threatened value
            _bIsThreatened = false;
            
            int actionNumber = Random.Range(0, 10);
            if(AISystem.enemyType == EnemyType.TUTORIALENEMY)
            {
                actionNumber = 3;
            }
            
            if (AISystem.enemyType != EnemyType.GLAIVEWIELDER)
            {
                if (actionNumber >= 4)
                {
                    AISystem.OnSwordAttack();
                }
                else
                {
                    AISystem.OnSwordAttack();
                    //AISystem.OnBlock();
                }
            }
            else
            {
                AISystem.OnGlaiveAttack();
            }
        }
        
    }
}
