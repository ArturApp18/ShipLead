using System;
using System.Linq;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Logic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace CodeBase.Enemy
{
	[RequireComponent(typeof(EnemyAnimator))]
	public class EnemyAttack : MonoBehaviour
	{
		public EnemyAnimator Animator;

		public float AttackCoolDown = 3f;
		public float Cleavage = 0.5f;
		public float EffectiveDistance = 0.5f;
		public float Damage = 10;

		private IGameFactory _gameFactory;
		private Transform _heroTransform;
		private float _attackCoolDown;
		private bool _isAttacking;
		private int _layerMask;
		private Collider2D[] _hits = new Collider2D[1];

		private bool _attackIsActive;

		private void Awake()
		{
			_gameFactory = AllServices.Container.Single<IGameFactory>();
			_layerMask = 1 << LayerMask.NameToLayer("Player");
			_gameFactory.HeroCreated += OnHeroCreated;
		}

		private void Update()
		{
			UpdateCooldown();

			if (CanAttack())
				StartAttack();
		}

		public void EnableAttack() =>
			_attackIsActive = true;

		public void DisableAttack() =>
			_attackIsActive = false;

		private void OnAttack()
		{
			if (Hit(out Collider2D hit))
			{
				PhysicDebug.DrawDebug(StartPoint(), Cleavage, 1);
				hit.transform.GetComponent<IHealth>().TakeDamage(Damage);
			}
		}

		private void OnAttackEnded()
		{
			_attackCoolDown = AttackCoolDown;
			_isAttacking = false;
		}

		private bool Hit(out Collider2D hit)
		{
			int hitCount = Physics2D.OverlapCircleNonAlloc(StartPoint(), Cleavage, _hits, _layerMask);

			hit = _hits.FirstOrDefault();

			return hitCount > 0;
		}

		private void UpdateCooldown()
		{
			if (!CooldownIsUp())
				_attackCoolDown -= Time.deltaTime;
		}

		private Vector3 StartPoint()
		{
			return new Vector3(transform.position.x, transform.position.y + 0.5f) + -transform.right * EffectiveDistance;
		}

		private void StartAttack()
		{
			//transform.LookAt(_heroTransform);
			Animator.PlayAttack();

			_isAttacking = true;
		}

		private bool CanAttack() =>
			_attackIsActive && !_isAttacking && CooldownIsUp();

		private bool CooldownIsUp() =>
			_attackCoolDown <= 0f;

		private void OnHeroCreated() =>
			_heroTransform = _gameFactory.HeroGameObject.transform;

	}

}