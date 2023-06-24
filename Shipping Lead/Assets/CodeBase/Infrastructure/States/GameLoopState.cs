using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
	public class GameLoopState : IState
	{
		public GameLoopState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain) { }

		public void Exit() { }

		public void Update()
		{
			
		}

		public void Enter() { }
	}
}