using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace CodeBase.Enemy
{
	[RequireComponent(typeof(EnemyAnimator), typeof(EnemyHealth))]
	public class EnemyDeath : MonoBehaviour
	{
		private const string DeadLayer = "DeadEnemy";
		[SerializeField] private EnemyAnimator _animator;
		[SerializeField] private EnemyHealth _health;
		[SerializeField] private MoveToHero _move;
		[SerializeField] private EnemyAttack _attack;
		[SerializeField] private GameObject _aggro;
		[SerializeField] private GameObject _deathFx;
		[SerializeField] private GameObject _enemyObject;
		[SerializeField] private int _deathLayer;
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
			_enemyObject.layer = _deathLayer;
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
			Instantiate(_deathFx, transform.position, Quaternion.identity);

		private IEnumerator DestroyTimer()
		{
			yield return new WaitForSeconds(_destroyTimer);
			Destroy(gameObject);
		}
	}
}