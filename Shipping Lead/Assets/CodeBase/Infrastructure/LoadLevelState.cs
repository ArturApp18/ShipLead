using CodeBase.CameraLogic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Infrastructure
{
	public class LoadLevelState : IPayloadedState<string>
	{
		private const string PlayerInitialPoint = "PlayerInitialPoint";
		
	
		
		private readonly GameStateMachine _gameStateMachine;
		private readonly SceneLoader _sceneLoader;
		private readonly LoadingCurtain _loadingCurtain;
		private readonly IGameFactory _gameFactory;

		public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain)
		{
			_gameStateMachine = gameStateMachine;
			_sceneLoader = sceneLoader;
			_loadingCurtain = loadingCurtain;
		}

		public void Enter(string sceneName)
		{
			_loadingCurtain.Show();
			_sceneLoader.Load(sceneName, OnLoaded);
		}

		public void Exit()
		{
			_loadingCurtain.Hide();
		}

		public void Update() { }

		private void OnLoaded()
		{
			GameObject hero = _gameFactory.CreateHero(at: GameObject.FindWithTag(PlayerInitialPoint));

			_gameFactory.CreateHud();

			CameraFollow(hero);
			
			_gameStateMachine.Enter<GameLoopState>();
		}

		private void CameraFollow(GameObject hero)
		{
			Camera.main
				.GetComponent<CameraFollow>()
				.Follow(hero);
		}

	}
}