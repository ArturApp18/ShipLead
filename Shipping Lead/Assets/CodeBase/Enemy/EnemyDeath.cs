using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace CodeBase.Enemy
{
	[RequireComponent(typeof(EnemyAnimator), typeof(EnemyHealth))]
	public class EnemyDeath : MonoBehaviour
	{
		[SerializeField] private EnemyAnimator _animator;
		[SerializeField] private EnemyHealth _health;
		[SerializeField] private MoveToHero _move;
		[SerializeField] private EnemyAttack _attack;
		[SerializeField] private GameObject _aggro;
		[SerializeField] private GameObject DeathFx;
		[SerializeField] private float _destroyTimer;

		public event Action Happend;

		private void Start()
		{
			_health.HealthChanged += HealthChanged;
		}

		private void OnDestroy()
		{
			_health.HealthChanged -= HealthChanged;
		}

		private void HealthChanged()
		{
			if (_health.Current <= 0)
				Die();
		}

		private void Die()
		{
			_health.HealthChanged -= HealthChanged;

			StopFollowing();

			_animator.PlayDeath();

			SpawnDeathFx();
			StartCoroutine(DestroyTimer());
			
			Happend?.Invoke();
		}

		private void StopFollowing()
		{
			_aggro.SetActive(false);
			_move.enabled = false;
			_attack.enabled = false;
		}

		private void SpawnDeathFx() =>
			Instantiate(DeathFx, transform.position, Quaternion.identity);

		private IEnumerator DestroyTimer()
		{
			yield return new WaitForSeconds(_destroyTimer);
			Destroy(gameObject);
		}
	}
}