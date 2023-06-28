using CodeBase.Infrastructure.Factory;
using UnityEngine;

namespace CodeBase.Enemy
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class MoveToHero : Follow
	{
		private const float MinimalDistance = 1.3f;
		
		public Rigidbody2D Rigidbody;

		private Transform _heroTransform;
		private IGameFactory _gameFactory;

		private float _movementSpeed;
		public float MovementSpeed
		{
			get
			{
				return _movementSpeed;
			}
			set
			{
				_movementSpeed = value;
			}
		}


		public void Construct(Transform heroTransform) =>
			_heroTransform = heroTransform;

		

		private void Update()
		{
			if (Initialized() && HeroNotReached())
				Move();
		}

		private void Move()
		{
			if (ChooseSide())
			{
				Rigidbody.velocity = new Vector2(MovementSpeed, Rigidbody.velocity.y);
			}
			else
			{
				Rigidbody.velocity = new Vector2(-MovementSpeed, Rigidbody.velocity.y);
			}
		}

		private bool ChooseSide() =>
			transform.position.x <= _heroTransform.position.x;
		

		private bool Initialized() =>
			_heroTransform != null;
		

		private bool HeroNotReached() =>
			Vector3.Distance(transform.position, _heroTransform.position) >= MinimalDistance;

	}
}