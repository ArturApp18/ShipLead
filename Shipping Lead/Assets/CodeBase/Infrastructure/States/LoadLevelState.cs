using CodeBase.CameraLogic;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
	public class LoadLevelState : IPayloadedState<string>
	{
		private const string PlayerInitialPoint = "PlayerInitialPoint";
		private const string SaveTrigger = "SaveTrigger";
		private const string EnemySpawner = "EnemySpawner";


		private readonly GameStateMachine _gameStateMachine;
		private readonly SceneLoader _sceneLoader;
		private readonly LoadingCurtain _loadingCurtain;
		private readonly IGameFactory _gameFactory;
		private readonly IPersistentProgressService _progressService;

		public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, IGameFactory gameFactory, IPersistentProgressService progressService)
		{
			_gameStateMachine = gameStateMachine;
			_sceneLoader = sceneLoader;
			_loadingCurtain = loadingCurtain;
			_gameFactory = gameFactory;
			_progressService = progressService;
		}

		public void Update() { }

		public void Enter(string sceneName)
		{
			_loadingCurtain.Show();
			_gameFactory.Cleanup();
			_sceneLoader.Load(sceneName, OnLoaded);
		}

		public void Exit()
		{
			_loadingCurtain.Hide();
		}

		private void InitHud(GameObject hero)
		{
			GameObject hud = _gameFactory.CreateHud();

			hud.GetComponentInChildren<ActorUI>()
				.Construct(hero.GetComponent<HeroHealth>());
		}

		private void InitGameWorld()
		{
			InitSpawners();
			InitSaveTriggers();
			
			GameObject hero = _gameFactory.CreateHero(at: GameObject.FindWithTag(PlayerInitialPoint));

			InitHud(hero);

			CameraFollow(hero);
		}

		private void InitSaveTriggers()
		{
			foreach (GameObject saveTrigger in GameObject.FindGameObjectsWithTag(SaveTrigger))
			{
				SaveTrigger trigger = saveTrigger.GetComponent<SaveTrigger>();
				_gameFactory.Register(trigger);
			}
		}

		private void InitSpawners()
		{
			foreach (GameObject enemySpawner in GameObject.FindGameObjectsWithTag(EnemySpawner))
			{
				EnemySpawner spawner = enemySpawner.GetComponent<EnemySpawner>();
				_gameFactory.Register(spawner);
			}
		}

		private void OnLoaded()
		{
			InitGameWorld();
			InformProgressReaders();
			
			_gameStateMachine.Enter<GameLoopState>();
		}

		private void InformProgressReaders()
		{
			foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
				progressReader.LoadProgress(_progressService.Progress);
		}

		private void CameraFollow(GameObject hero)
		{
			Camera.main
				.GetComponent<CameraFollow>()
				.Follow(hero);
		}

	}
}