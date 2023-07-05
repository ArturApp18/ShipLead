using System;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.States;
using UnityEngine;

namespace CodeBase.Logic
{
	public class LevelTransferTrigger : MonoBehaviour
	{
		private const string Player = "Player";
		
		public string TransfeTo;
		private IGameStateMachine _stateMachine;

		private bool _triggered;
		public void Construct(IGameStateMachine stateMachine) =>
			_stateMachine = stateMachine;

		private void OnTriggerEnter2D(Collider2D col)
		{
			if (_triggered)
				return;

			if (col.CompareTag(Player))
			{
				_stateMachine.Enter<LoadLevelState, string>(TransfeTo);
				_triggered = true;
			}
		}
	}
}