using System.Collections;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
	public class Aggro : MonoBehaviour
	{
		public TriggerObserver TriggerObserver;
		public Follow Follow;

		public float Cooldown;
		private bool _hasAggroTarget;
		private Coroutine _aggroCoroutine;

		private void Start()
		{
			TriggerObserver.TriggerEnter += TriggerEnter;
			TriggerObserver.TriggerExit += TriggerExit;

			SwitchFollowOff();
		}

		private void TriggerEnter(Collider2D obj)
		{
			if (!_hasAggroTarget)
			{
				_hasAggroTarget = true;

				StopAggroCoroutine();

				SwitchFollowOn();
				Debug.Log("MoveEnemy");
			}
		}

		private void TriggerExit(Collider2D obj)
		{
			if (_hasAggroTarget)
			{
				_hasAggroTarget = false;

				_aggroCoroutine = StartCoroutine(SwitchFollowOffAfterCooldown());
			}
		}

		private IEnumerator SwitchFollowOffAfterCooldown()
		{
			yield return new WaitForSeconds(Cooldown);

			SwitchFollowOff();
		}

		private void StopAggroCoroutine()
		{
			if (_aggroCoroutine != null)
			{
				StopCoroutine(_aggroCoroutine);
				_aggroCoroutine = null;
			}
		}

		private void SwitchFollowOn() =>
			Follow.enabled = true;

		private void SwitchFollowOff() =>
			Follow.enabled = false;
	}
}