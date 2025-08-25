using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BrackeysGameJam {
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(PlayerInput))]
	public sealed class Movement : MonoBehaviour {
		[SerializeField] private Rigidbody2D rigidBody;

		[SerializeField] private float speed;
		[SerializeField] private float airAcceleration;
		[SerializeField] private float crouchSpeedModifier;
		[SerializeField] private float airDeceleration;

		[SerializeField] private float jumpVel;

		[SerializeField] private Animator animator;
		[SerializeField] private string hopTrigger;

		private Vector2 input;

		private bool isJumping;
		private bool isCrouching;

		private bool isGrounded;
		private bool justLanded;

		private int hopTriggerHash;

		void Start() {
			if (rigidBody == null)
				rigidBody = GetComponent<Rigidbody2D>();

			if (animator == null)
				animator = GetComponent<Animator>();

			hopTriggerHash = Animator.StringToHash(hopTrigger);
		}

		private void FixedUpdate() {
			if (isJumping && isGrounded) {
				rigidBody.linearVelocityY = jumpVel;
				animator.SetTrigger(hopTriggerHash);
			}
			else if (justLanded) {
				rigidBody.linearVelocityY = 0;
				justLanded = false;
				return;
			}

			{
				if (isGrounded) {
					if (isCrouching) {
						rigidBody.linearVelocityX = crouchSpeedModifier * speed * input.x;
					}
					else {
						rigidBody.linearVelocityX = speed * input.x;
					}
				}
				else if (!Mathf.Approximately(input.x, 0)) {
					rigidBody.linearVelocityX =
						Math.Clamp(rigidBody.linearVelocityX + airAcceleration * input.x, -speed, speed);
				}
				else {
					rigidBody.linearVelocityX *= airDeceleration;
				}
			}
		}

		public void OnMove(InputAction.CallbackContext context) {
			input = context.ReadValue<Vector2>();
		}

		public void OnJump(InputAction.CallbackContext context) {
			if (context.started ^ context.canceled)
				isJumping = !isJumping;
		}

		public void OnCrouch(InputAction.CallbackContext context) {
			if (context.started ^ context.canceled)
				isCrouching = !isCrouching;
		}

		public void OnGroundEnter() {
			isGrounded = true;
			justLanded = true;
		}

		public void OnGroundExit() {
			isGrounded = false;
			justLanded = false;
		}
	}
}