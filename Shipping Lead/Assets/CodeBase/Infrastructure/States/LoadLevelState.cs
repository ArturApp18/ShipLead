using System.Collections.Generic;
using CodeBase.CameraLogic;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.Logic;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Factory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
	public class LoadLevelState : IPayloadedState<string>
	{
		private const string PlayerInitialPoint = "PlayerInitialPoint";
		private const string SaveTrigger = "SaveTrigger";
		private const string EnemySpawner = "EnemySpawners";
		private const string Loot = "Loot";


		private readonly GameStateMachine _gameStateMachine;
		private readonly SceneLoader _sceneLoader;
		private readonly LoadingCurtain _loadingCurtain;
		private readonly IGameFactory _gameFactory;
		private readonly IPersistentProgressService _progressService;
		private readonly IStaticDataService _staticData;
		private readonly ISaveLoadService _saveLoadService;
		private readonly IUIFactory _uiFactory;

		public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, IGameFactory gameFactory,
			IPersistentProgressService progressService, IStaticDataService staticData, ISaveLoadService saveLoadService, IUIFactory uiFactory)
		{
			_gameStateMachine = gameStateMachine;
			_sceneLoader = sceneLoader;
			_loadingCurtain = loadingCurtain;
			_gameFactory = gameFactory;
			_progressService = progressService;
			_staticData = staticData;
			_saveLoadService = saveLoadService;
			_uiFactory = uiFactory;
		}

		public void Update()
		{
			
		}

		public void Enter(string sceneName)
		{
			_loadingCurtain.Show();
			_gameFactory.Cleanup();
			_sceneLoader.Load(sceneName, OnLoaded);
		}

		public void Exit() =>
			_loadingCurtain.Hide();

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

			InitLoot();

			GameObject hero = InitHero();

			InitHud(hero);

			CameraFollow(hero);
		}

		private GameObject InitHero() =>
			_gameFactory.CreateHero(at: GameObject.FindWithTag(PlayerInitialPoint));

		private void InitLoot()
		{
			foreach (KeyValuePair<string, LootPieceData> item in _progressService.Progress.WorldData.LootData.LootPiecesOnScene.Dictionary)
			{
				LootPiece lootPiece = _gameFactory.CreateLoot();
				lootPiece.GetComponent<UniqueId>().Id = item.Key;
				lootPiece.Initialize(item.Value.Loot);
				lootPiece.transform.position = item.Value.Position.AsUnityVector();
			}
		}

		private void InitSaveTriggers()
		{
			LevelStaticData levelData = GetLevelStaticData();
			
			foreach (SaveTriggerData saveTriggerData in levelData.SaveTriggers)
			{
				SaveTrigger saveTrigger = _gameFactory.CreateSaveTrigger(saveTriggerData.Position,saveTriggerData.Id);
				saveTrigger.Construct(_saveLoadService);
			}
		}

		private void InitSpawners()
		{
			LevelStaticData levelData = GetLevelStaticData();
			
			foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
			{
				_gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
			}
		}

		private LevelStaticData GetLevelStaticData()
		{
			string sceneKey = SceneManager.GetActiveScene().name;
			LevelStaticData levelData = _staticData.ForLevel(sceneKey);
			return levelData;
		}

		private void OnLoaded()
		{
			InitUIRoot();
			InitGameWorld();
			InformProgressReaders();

			_gameStateMachine.Enter<GameLoopState>();
		}

		private void InitUIRoot() =>
			_uiFactory.CreateUIRoot();

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