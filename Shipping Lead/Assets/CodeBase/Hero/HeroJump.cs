using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.Input;
using UnityEngine;

namespace CodeBase.Hero
{
	public class HeroJump : MonoBehaviour
	{
		[SerializeField] private Rigidbody2D _rigidbody;
		[SerializeField] private HeroCollision _heroCollision;
		[SerializeField] private HeroAnimator _animator;

		private Stats _stats;
		private IInputService _inputService;
		private bool _canJump;
		[SerializeField]
		private int _extraJumps;

		private void Start() =>
			_heroCollision.OnGrounded += OnGrounded;

		private void OnDestroy() =>
			_heroCollision.OnGrounded -= OnGrounded;

		private void OnGrounded() =>
			_extraJumps = _stats.ExtraJumps;

		public void Construct(IInputService inputService, Stats stats)
		{
			_inputService = inputService;
			_stats = stats;
		}


		private void Update()
		{
			if (CanJump())
				Jump();
		}

		private void FixedUpdate()
		{
			

			if (!_heroCollision.IsGrounded)
			{
				ApplyingAirLinearDrag();
				FallMultiplier();
			}
		}

		private void FallMultiplier()
		{
			if (_rigidbody.velocity.y < 0)
			{
				_rigidbody.gravityScale = _stats.FallMultiplier;
			}
			else if (_rigidbody.velocity.y > 0 && !_inputService.IsJumpButton())
			{
				_rigidbody.gravityScale = _stats.LowMultiplier;
			}
			else
			{
				_rigidbody.gravityScale = 1f;
			}
		}
		private void ApplyingAirLinearDrag() =>
			_rigidbody.drag = _stats.AirLinearDrag;

		private bool CanJump() =>
			_inputService.IsJumpButtonDown() && (_heroCollision.IsGrounded || _extraJumps > 0);

		private void Jump()
		{
			if (!_heroCollision.IsGrounded)
				_extraJumps--;
			_animator.PlayJump();

			_rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0f);
			_rigidbody.AddForce(Vector2.up * _stats.JumpForce, ForceMode2D.Impulse);
		}
	}

}