using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Hero
{
	public class HeroMove : MonoBehaviour, ISavedProgress
	{
		[SerializeField] private Rigidbody2D _rigidbody;
		[SerializeField] private CapsuleCollider2D _heroCollider;
		[SerializeField] private HeroCollision _heroCollision;

		private Stats _stats;
		private IInputService _inputService;

		private float _horizontalMove;
		
		[SerializeField] private bool _canMove;

		public void Construct(IInputService inputService, Stats stats)
		{
			_inputService = inputService;
			_stats = stats;
		}
		

		private void Update()
		{
			if (_canMove)
			{
				_horizontalMove = _inputService.Axis.x;
				/*if (_inputService.Axis.x > 0.1 || _inputService.Axis.x < -0.1)
				{
					_animator.StartRun();
				}
				else
				{
					_animator.StopRun();
				}*/
			}
		}

		private void FixedUpdate()
		{
			if (_canMove)
			{
				float targetSpeed = _horizontalMove * _stats.MaxMovementSpeed;
				float speedDif = targetSpeed - _rigidbody.velocity.x;
				float accelRate = ( Mathf.Abs(_stats.MaxMovementSpeed) > 0.01f ) ? _stats.MovementAccelaration : _stats.MovementDeacclaration;
				float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, _stats.VelocityPower) * Mathf.Sign(speedDif);
				_rigidbody.AddForce(movement * Vector2.right, ForceMode2D.Force);

			}
		}


		public void UpdateProgress(PlayerProgress progress) =>
			progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(), transform.position.AsVectorData());

		public void LoadProgress(PlayerProgress progress)
		{
			_stats = progress.HeroStats;

			if (CurrentLevel() == progress.WorldData.PositionOnLevel.Level)
			{
				Vector3Data savedPosition = progress.WorldData.PositionOnLevel.Position;
				if (savedPosition != null)
					Warp(to: savedPosition);
			}
		}

		private void Warp(Vector3Data to) =>
			transform.position = to.AsUnityVector().AddY(_heroCollider.size.y);

		private string CurrentLevel() =>
			SceneManager.GetActiveScene().name;
	}

}