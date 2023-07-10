using System.Collections.Generic;
using System.Threading.Tasks;
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

		public void Update() { }

		public void Enter(string sceneName)
		{
			_loadingCurtain.Show();
			_gameFactory.Cleanup();
			_gameFactory.WarmUp();
			_sceneLoader.Load(sceneName, OnLoaded);
		}

		private async void OnLoaded()
		{
			await InitUIRoot();
			await InitGameWorld();
			InformProgressReaders();

			_gameStateMachine.Enter<GameLoopState>();
		}

		public void Exit() =>
			_loadingCurtain.Hide();

		private async Task InitHud(GameObject hero)
		{
			GameObject hud = await _gameFactory.CreateHud();

			hud.GetComponentInChildren<ActorUI>()
				.Construct(hero.GetComponent<HeroHealth>());
		}

		private async Task InitGameWorld()
		{
			LevelStaticData levelData = GetLevelStaticData();
			await InitSpawners(levelData);
			await InitSaveTriggers(levelData);
			await InitLevelTransfers(levelData);
			await InitLoot();
			GameObject hero = await InitHero(levelData);
			await InitHud(hero);
			
			CameraFollow(hero);
		}

		private async Task<GameObject> InitHero(LevelStaticData levelData) =>
			await _gameFactory.CreateHero(levelData.InitialHeroPosition);

		private async Task InitLoot()
		{
			foreach (KeyValuePair<string, LootPieceData> item in _progressService.Progress.WorldData.LootData.LootPiecesOnScene.Dictionary)
			{
				LootPiece lootPiece = await _gameFactory.CreateLoot();
				lootPiece.GetComponent<UniqueId>().Id = item.Key;
				lootPiece.Initialize(item.Value.Loot);
				lootPiece.transform.position = item.Value.Position.AsUnityVector();
			}
		}

		private async Task InitSaveTriggers(LevelStaticData levelData)
		{
			foreach (SaveTriggerData saveTriggerData in levelData.SaveTriggers)
			{
				SaveTrigger saveTrigger = await _gameFactory.CreateSaveTrigger(saveTriggerData.Position, saveTriggerData.Id);
				saveTrigger.Construct(_saveLoadService);
			}
		}

		private async Task InitLevelTransfers(LevelStaticData levelData)
		{
			foreach (LevelTransferTriggerData levelTransfer in levelData.LevelTransfers)
			{
				await _gameFactory.CreateLevelTransferTrigger(levelTransfer.Position, levelTransfer.Id, levelTransfer.TransferTo);
			}
		}

		private async Task InitSpawners(LevelStaticData levelData)
		{
			foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
			{
				await _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
			}
		}

		private async Task InitUIRoot() =>
			await _uiFactory.CreateUIRoot();

		private void InformProgressReaders()
		{
			foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
				progressReader.LoadProgress(_progressService.Progress);
		}

		private void CameraFollow(GameObject hero) =>
			Camera.main
				.GetComponent<CameraFollow>()
				.Follow(hero);

		private LevelStaticData GetLevelStaticData() =>
			_staticData.ForLevel(SceneManager.GetActiveScene().name);

	}
}