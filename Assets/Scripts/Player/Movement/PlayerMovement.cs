﻿using System;
using System.Runtime.CompilerServices;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Camera;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Movement
{
    
    public class PlayerMovement : ActionMonoBehaviour, IPlayerMovement
    {
        
        #region - - - - - - Fields - - - - - -

        private Animator m_Animator;
        private ICameraController m_CameraController;
        private IControlledCameraState m_CameraState;
        private FinishingMoveController m_FinishingMoveController; // This needs to be decoupled. Use interfaces
        private IPlayerAttackState m_PlayerAttackState;
        private PlayerState m_PlayerState;

        private float m_CurrentAngleSmoothVelocity;
        private bool m_IsMovementEnabled = true;
        private bool m_IsRotationEnabled = true;
        private bool m_IsSprinting = false;
        private Vector2 m_InputDirection = Vector2.zero;
        private Vector3 m_MoveDirection = Vector2.zero;
        private float m_RotationSpeed = 4f;
        private float m_MovementSmoothingDampingTime = .1f;

        #endregion Fields

        #region - - - - - - Properties - - - - - -

        Vector2 IPlayerMovement.MoveDirection
            => this.m_MoveDirection;

        #endregion Properties
        
        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            this.m_Animator = this.GetComponent<Animator>();
            this.m_CameraController = this.GetComponent<ICameraController>();
            this.m_CameraState = this.GetComponent<IControlledCameraState>();
            this.m_FinishingMoveController = this.GetComponentInChildren<FinishingMoveController>();
            this.m_PlayerAttackState = this.GetComponent<IPlayerAttackState>();
            this.m_PlayerState = this.GetComponent<PlayerState>();
        }

        private void Update()
        {
            if (this.IsPaused || !this.m_IsMovementEnabled || this.m_FinishingMoveController.bIsFinishing) 
                return;

            // Move the player
            this.CalculateMovementDirection();
            this.RotatePlayerToMovementDirection();
            this.PerformMovement();
            
            // Perform specific movement behavior
            //this.LockPlayerRotationToAttackTarget();
            this.PerformSprint();
        }

        #endregion Lifecycle Methods
        
        #region - - - - - - Methods - - - - - -

        void IPlayerMovement.DisableMovement()
            => this.m_IsMovementEnabled = false;

        void IPlayerMovement.DisableRotation()
            => this.m_IsRotationEnabled = false;

        void IPlayerMovement.EnableMovement()
            => this.m_IsMovementEnabled = true;

        void IPlayerMovement.EnableRotation()
            => this.m_IsRotationEnabled = true;

        void IPlayerMovement.PreparePlayerMovement(Vector2 moveDirection)
        {
            Debug.Log($"Is it performing finishing move: {!this.m_FinishingMoveController.bIsFinishing}");
            if (this.m_FinishingMoveController.bIsFinishing)
                return;
            
            if (moveDirection == Vector2.zero)
                this.m_Animator.SetBool("IsSprinting", false);

            this.m_InputDirection = moveDirection;
        }

        void IPlayerMovement.PrepareSprint(bool isSprinting)
        {
            this.m_IsSprinting = isSprinting;

            if (this.m_CameraState.IsCameraViewTargetLocked) 
                return;
            
            this.m_CameraController.ToggleSprintCameraState(this.m_IsSprinting);
        }

        private Vector3 CalculateMovementDirection()
        {
            Vector3 _Direction = new Vector3(this.m_InputDirection.x, 0, this.m_InputDirection.y);
            Vector3 _RelativeDirection = _Direction - this.transform.forward;
            
            // Provide normalized direction otherwise stop moving
            // this.m_MoveDirection = _Direction != Vector3.zero 
            //                         ? _RelativeDirection.normalized 
            //                         : Vector3.zero;

            this.m_MoveDirection = _Direction.normalized;
            
            return this.m_MoveDirection;
        }        

        private void LockPlayerRotationToAttackTarget()
        {
            if (!this.m_CameraState.IsCameraViewTargetLocked)
                return;
            
            Vector3 _NewLookDirection = this.m_PlayerState.AttackTarket.transform.position - this.transform.position;
            this.transform.rotation = Quaternion.Slerp(
                this.transform.rotation,
                Quaternion.LookRotation(_NewLookDirection),
                this.m_RotationSpeed);
            
            this.transform.rotation = Quaternion.Euler(0, this.transform.rotation.eulerAngles.y, 0);
        }

        private void RotatePlayerToMovementDirection()
        {
            // Vector3 _Direction = new Vector3(this.m_MoveDirection.x, 0, this.m_MoveDirection.y).normalized;
            if (this.m_InputDirection == Vector2.zero
                 || this.m_CameraState.IsCameraViewTargetLocked
                 || !this.m_IsRotationEnabled
                 || this.m_PlayerAttackState.IsWeaponSheathed)
                return;

            float _TargetAngle = Mathf.Atan2(this.m_MoveDirection.x, this.m_MoveDirection.z) 
                                    * Mathf.Rad2Deg + this.m_CameraState.CurrentEulerAngles.y;
            float _NextAngleRotation = Mathf.SmoothDampAngle(
                this.transform.eulerAngles.y,
                _TargetAngle,
                ref this.m_CurrentAngleSmoothVelocity,
                this.m_MovementSmoothingDampingTime);
            
            this.transform.rotation = Quaternion.Euler(0f, _NextAngleRotation, 0f);
        }

        private void PerformMovement()
        {
            // Invokes player movement through the physically based animation movements
            this.m_Animator.SetFloat(
                "XInput", 
                this.m_InputDirection.x, 
                this.m_MovementSmoothingDampingTime, 
                Time.deltaTime);
            
            this.m_Animator.SetFloat(
                "YInput",
                this.m_InputDirection.y,
                this.m_MovementSmoothingDampingTime,
                Time.deltaTime);
            
            this.m_Animator.SetFloat(
                "InputSpeed",
                this.m_InputDirection.magnitude,
                this.m_MovementSmoothingDampingTime,
                Time.deltaTime);

        }
        private void PerformSprint()
        {
            // TODO: This behaviour should not be function at runtime, instead make this event based.
            if (this.m_InputDirection == Vector2.zero || !this.m_IsSprinting)
                this.m_Animator.SetBool("IsSprinting", false);
            else
                this.m_Animator.SetBool("IsSprinting", true);
        }

        #endregion Methods
        
    }
    
}
