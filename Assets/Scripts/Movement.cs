using System;
using UnityEngine;
using UnityEngine.InputSystem;


namespace BrackeysGameJam
{
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
        
        private Vector2 input;

        private bool jump;
        private bool crouch;
        private bool hop;

        private bool grounded;
        private bool groundedFlag;

        void Start()
        {
            if (rigidBody == null)
                rigidBody = GetComponent<Rigidbody2D>();
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
                return;
            }

            if (hop)
            {
                var direction = new Vector2(input.x, 0).Rotate(hopAngle);
                rigidBody.AddForce(direction * hopForce);
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
            jump = context.started && !context.canceled;
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
            groundedFlag = false;
        }

        public void OnGroundLeave()
        {
            grounded = false;
            groundedFlag = true;
        }
    }
}