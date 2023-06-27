using System;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace CodeBase.Enemy
{
	public class RotateToHero : Follow
	{
		public float Speed;

		private Transform _heroTransform;
		private IGameFactory _gameFactory;
		private Vector2 _positionToLook;

		private void Start()
		{
			_gameFactory = AllServices.Container.Single<IGameFactory>();

			if (HeroExists())
				InitializeTransform();
			else
				_gameFactory.HeroCreated += InitializeTransform;
		}

		private void Update()
		{
			if (Initialized())
				RotateTowardsHero(_positionToLook);
		}

		private void RotateTowardsHero(Vector2 pos)
		{
			Quaternion updatePositionToLookAt = UpdatePositionToLookAt();

			transform.rotation = SmoothedRotation(transform.rotation, updatePositionToLookAt);
		}

		private Quaternion UpdatePositionToLookAt()
		{
			Vector2 directionTarget = _heroTransform.position - transform.position;

			float angle = Mathf.Atan2(directionTarget.x, directionTarget.y) * Mathf.Rad2Deg;

			return TargetRotation(angle);
		}

		private Quaternion TargetRotation(float angle)
		{
			Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));

			return targetRotation;
		}

		private Quaternion SmoothedRotation(Quaternion rotation, Quaternion positionToLook) =>
			Quaternion.Lerp(rotation, positionToLook, SpeedFactor());

		private float SpeedFactor() =>
			Speed * Time.deltaTime;

		private bool Initialized() =>
			_heroTransform != null;

		private void InitializeTransform() =>
			_heroTransform = _gameFactory.HeroGameObject.transform;

		private bool HeroExists() =>
			_gameFactory.HeroGameObject != null;


	}

}