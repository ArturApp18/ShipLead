using System;
using UnityEngine;

namespace CodeBase.Enemy
{
	[RequireComponent(typeof(EnemyAnimator))]
	[RequireComponent(typeof(Rigidbody2D))]
	public class AnimateAlongEnemy : MonoBehaviour
	{
		private const float MinimalVelocity = 0.1f;
		
		public EnemyAnimator Animator;
		public Rigidbody2D _rigidbody;

		private void Update()
		{
			if (ShouldMove())
				Animator.Move();
			else
				Animator.StopMoving();
		}

		private bool ShouldMove() =>
			_rigidbody.velocity.magnitude > MinimalVelocity;

	}
}