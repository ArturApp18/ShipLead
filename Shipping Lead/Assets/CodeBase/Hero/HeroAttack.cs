using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Hero
{
	[RequireComponent(typeof(HeroAnimator))]
	public class HeroAttack : MonoBehaviour, ISavedProgressReader
	{
		[SerializeField] private HeroAnimator _heroAnimator;
		[SerializeField] private Transform _attackPoint;
		
		private IInputService _input;

		private static int _layerMask;
		private Collider2D[] _hits = new Collider2D[3];
		private Stats _stats;


		public void Construct(IInputService inputService)
		{
			_input = inputService;
		}
		private void Awake()
		{
			_layerMask = 1 << LayerMask.NameToLayer("Hittable");
		}

		private void Update()
		{
			if (_input.IsAttackButtonUp() && !_heroAnimator.IsAttacking)
				_heroAnimator.PlayAttack();
		}

		public void OnAttack()
		{
			for (int i = 0; i < Hit(); i++)
			{
				_hits[i].transform.parent.GetComponent<IHealth>().TakeDamage(_stats.Damage);
			}
		}

		public void LoadProgress(PlayerProgress progress) =>
			_stats = progress.HeroStats;

		private int Hit() =>
			Physics2D.OverlapCircleNonAlloc(StartPoint(), _stats.DamageRadius, _hits, _layerMask);

		private Vector2 StartPoint() =>
			new Vector2(_attackPoint.position.x, _attackPoint.position.y);

	}
}