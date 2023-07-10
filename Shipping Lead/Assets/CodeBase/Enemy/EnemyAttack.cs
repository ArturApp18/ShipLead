using System.Linq;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
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
		
		private Transform _heroTransform;
		private float _attackCoolDown;
		private bool _isAttacking;
		private int _layerMask;
		private Collider2D[] _hits = new Collider2D[1];

		private bool _attackIsActive;

		public void Construct(Transform heroTransform) =>
			_heroTransform = heroTransform;

		private void Awake()
		{
			_layerMask = 1 << LayerMask.NameToLayer("Player");
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

		private Vector3 StartPoint() =>
			new Vector3(transform.position.x, transform.position.y + 0.5f) + -transform.right * EffectiveDistance;

		private void StartAttack()
		{
			Animator.PlayAttack();

			_isAttacking = true;
		}

		private bool CanAttack() =>
			_attackIsActive && !_isAttacking && CooldownIsUp();

		private bool CooldownIsUp() =>
			_attackCoolDown <= 0f;

	}
}