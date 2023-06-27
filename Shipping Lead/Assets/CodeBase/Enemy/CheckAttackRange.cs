using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
	[RequireComponent(typeof(EnemyAttack))]
	public class CheckAttackRange : MonoBehaviour
	{
		public EnemyAttack Attack;
		public TriggerObserver TriggerObserver;

		private void Start()
		{
			TriggerObserver.TriggerEnter += TriggerEnter;
			TriggerObserver.TriggerExit += TriggerExit;

			Attack.DisableAttack();
		}

		private void TriggerExit(Collider2D obj)
		{
			Attack.EnableAttack();
		}

		private void TriggerEnter(Collider2D obj)
		{
			Attack.DisableAttack();
		}
	}
}