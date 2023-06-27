using System;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Enemy
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class MoveToHero : Follow
	{
		private const float MinimalDistance = 1.3f;

		//[SerializeField] private RotateToHero _rotateToHero;
		
		public Rigidbody2D Rigidbody;

		private Transform _heroTransform;
		private IGameFactory _gameFactory;

		[SerializeField] private float _movementSpeed;
		

		private void Start()
		{
			_gameFactory = AllServices.Container.Single<IGameFactory>();

			if (_gameFactory.HeroGameObject != null)
				InitializedHeroTransform();
			else
				_gameFactory.HeroCreated += HeroCreated;
		}

		private void Update()
		{
			if (Initialized() && HeroNotReached())
				Move();
		}

		private void Move()
		{
			if (ChooseSide())
			{
				Rigidbody.velocity = new Vector2(_movementSpeed, Rigidbody.velocity.y);
			}
			else
			{
				Rigidbody.velocity = new Vector2(-_movementSpeed, Rigidbody.velocity.y);
			}
		}

		private bool ChooseSide() =>
			transform.position.x <= _heroTransform.position.x;

		private void HeroCreated() =>
			InitializedHeroTransform();

		private bool Initialized() =>
			_heroTransform != null;

		private bool InitializedHeroTransform() =>
			_heroTransform = _gameFactory.HeroGameObject.transform;


		private bool HeroNotReached() =>
			Vector3.Distance(transform.position, _heroTransform.position) >= MinimalDistance;

	}
}