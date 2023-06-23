using System;
using CodeBase.Services.Input;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure
{
	public class BootstrapState : IState
	{
		private const string Initial = "Initial";
		private readonly GameStateMachine _stateMachine;
		private readonly SceneLoader _sceneLoader;

		public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader)
		{
			_stateMachine = stateMachine;
			_sceneLoader = sceneLoader;
		}

		public void Enter()
		{
			RegisterServices();
			_sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);
		}

		private void EnterLoadLevel() =>
			_stateMachine.Enter<LoadLevelState, string>("Main");

		private void RegisterServices()
		{
			Game.InputService = RegisterInputServices();
		}

		public void Exit()
		{
			
		}

		public void Update()
		{
			Debug.Log("lox");
		}

		private IInputService RegisterInputServices()
		{
			return Application.isEditor 
				? new StandaloneInputService() 
				: new MobileInputService();
		}
	}
}