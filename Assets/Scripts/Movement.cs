using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BrackeysGameJam
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerInput))]
    public sealed class Movement : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rigidBody;

        [SerializeField] private float speed;
        [SerializeField] private float crouchSpeed;
        
        [SerializeField] private float jumpForce;
        
        [SerializeField] private float hopAngle;
        [SerializeField] private float hopForce;
        [SerializeField] private float hopImpulseTime;
        
        [SerializeField] private Animator animator;
        [SerializeField] private string hopTrigger;
        
        private Vector2 input;

        private bool hop;
        private bool jump;
        private bool crouch;

        private bool grounded;
        private bool groundedFlag;

        private int hopTriggerHash;

        void Start()
        {
            if (rigidBody == null)
                rigidBody = GetComponent<Rigidbody2D>();
            if (animator == null)
                animator = GetComponent<Animator>();

            hopTriggerHash = Animator.StringToHash(hopTrigger);
        }

        private void FixedUpdate()
        {
            if (groundedFlag)
            {
                rigidBody.AddForce(new Vector2(-rigidBody.linearVelocityX / hopImpulseTime, -rigidBody.linearVelocityY / hopImpulseTime));
                groundedFlag = false;
                return;
            }
            
            if(!grounded)
                return;

            if (jump && grounded)
            {
                rigidBody.AddForce(Vector2.up * jumpForce);
                jump = false;
                return;
            }

            if (hop && !Mathf.Approximately(input.x, 0))
            {
                var direction = new Vector2(input.x, 0).Rotate(input.x > 0 ? hopAngle : -hopAngle);
                rigidBody.AddForce(direction * hopForce);
                
                animator.SetTrigger(hopTriggerHash);
            }
            else if (crouch)
                rigidBody.linearVelocityX = crouchSpeed * input.x;
            else
                rigidBody.linearVelocityX = speed * input.x;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            input = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started)
                jump = true;
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (context.started ^ context.canceled)
                crouch = !crouch;
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.started ^ context.canceled)
                hop = !hop;
        }

        public void OnGroundEnter()
        {
            grounded = true;
            groundedFlag = true;
        }

        public void OnGroundExit()
        {
            grounded = false;
            groundedFlag = false;
        }
    }
}