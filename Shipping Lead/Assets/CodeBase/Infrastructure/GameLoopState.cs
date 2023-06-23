using System;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Infrastructure
{
	public class GameLoopState : IState
	{
		public GameLoopState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain) { }

		public void Exit() { }

		public void Update()
		{
			Debug.Log(Game.InputService.Axis);
		}

		public void Enter() { }
	}
}