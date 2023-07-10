using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.Randomizer;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using Object = UnityEngine.Object;


namespace CodeBase.Infrastructure.Factory
{
	public class GameFactory : IGameFactory
	{

		private readonly IAssetsProvider _assetsProvider;
		private readonly IStaticDataService _staticData;
		private readonly IRandomService _randomService;
		private readonly IPersistentProgressService _progressService;
		private readonly ISaveLoadService _saveLoadService;
		private readonly IWindowService _windowService;
		private readonly IGameStateMachine _gameStateMachine;
		private readonly IInputService _inputService;

		public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
		public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

		private GameObject HeroGameObject { get; set; }

		public GameFactory(IAssetsProvider assetsProvider, IStaticDataService staticData, IRandomService randomService, IPersistentProgressService progressService,
			IWindowService windowService, IInputService inputService, IGameStateMachine gameStateMachine)
		{
			_assetsProvider = assetsProvider;
			_staticData = staticData;
			_randomService = randomService;
			_progressService = progressService;
			_windowService = windowService;
			_inputService = inputService;
			_gameStateMachine = gameStateMachine;
		}

		public async Task WarmUp()
		{
			await _assetsProvider.Load<GameObject>(AssetAddress.LootAddress);
			await _assetsProvider.Load<GameObject>(AssetAddress.SpawnerAddress);
			await _assetsProvider.Load<GameObject>(AssetAddress.HeroAddress);
			await _assetsProvider.Load<GameObject>(AssetAddress.HUDAddress);
			await _assetsProvider.Load<GameObject>(AssetAddress.LevelTransferAddress);
			await _assetsProvider.Load<GameObject>(AssetAddress.SaveTriggerAddress);
		}

		public async Task<GameObject> CreateHero(Vector3 at)
		{
			GameObject prefab = await _assetsProvider.Load<GameObject>(AssetAddress.HeroAddress);
			
			HeroGameObject = InstantiateRegistered(prefab, at);
			
			HeroGameObject.GetComponent<HeroMove>().Construct(_inputService, _progressService.Progress.HeroStats);
			HeroGameObject.GetComponent<HeroJump>().Construct(_inputService, _progressService.Progress.HeroStats);
			HeroGameObject.GetComponent<HeroRotate>().Construct(_inputService);
			HeroGameObject.GetComponent<HeroAttack>().Construct(_inputService);
			return HeroGameObject;
		}

		public async Task<GameObject> CreateHud()
		{
			GameObject hud = await InstantiateRegisteredAsync(AssetAddress.HUDAddress);


			hud.GetComponentInChildren<LootCounter>()
				.Construct(_progressService.Progress.WorldData);

			foreach (OpenWindowButton openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>())
			{
				openWindowButton.Construct(_windowService);
			}

			return hud;
		}

		public async Task<GameObject> CreateMonster(MonsterTypeId typeId, Transform parent)
		{
			MonsterStaticData monsterData = _staticData.ForMonster(typeId);

			GameObject prefab = await _assetsProvider.Load<GameObject>(monsterData.PrefabReference);

			GameObject monster = Object.Instantiate(prefab, parent.position, Quaternion.identity, parent);

			IHealth health = monster.GetComponent<IHealth>();
			health.Current = monsterData.Hp;
			health.Max = monsterData.Hp;

			monster.GetComponent<MoveToHero>().Construct(HeroGameObject.transform);
			monster.GetComponent<MoveToHero>().MovementSpeed = monsterData.MoveSpeed;

			LootSpawner lootSpawner = monster.GetComponentInChildren<LootSpawner>();
			lootSpawner.SetLoot(monsterData.MinLoot, monsterData.MaxLoot);

			lootSpawner.Construct(this, _randomService);


			EnemyAttack attack = monster.GetComponent<EnemyAttack>();
			attack.Construct(HeroGameObject.transform);
			attack.Damage = monsterData.Damage;
			attack.Cleavage = monsterData.Cleavage;
			attack.EffectiveDistance = monsterData.EffeectiveDistance;

			monster.GetComponent<RotateToHero>()?.Construct(HeroGameObject.transform);


			return monster;
		}

		public async Task<LootPiece> CreateLoot()
		{
			GameObject prefab = await _assetsProvider.Load<GameObject>(AssetAddress.LootAddress);
			LootPiece lootPiece = InstantiateRegistered(prefab).GetComponent<LootPiece>();
			lootPiece.Construct(_progressService.Progress.WorldData);

			return lootPiece;
		}

		public async Task CreateSpawner(Vector2 at, string spawnerId, MonsterTypeId monsterTypeId)
		{
			GameObject prefab = await _assetsProvider.Load<GameObject>(AssetAddress.SpawnerAddress);
			
			EnemySpawnPoint spawner = InstantiateRegistered(prefab, at)
				.GetComponent<EnemySpawnPoint>();

			spawner.Construct(this);
			spawner.Id = spawnerId;
			spawner.MonsterTypeId = monsterTypeId;
		}

		public async Task<LevelTransferTrigger> CreateLevelTransferTrigger(Vector2 at, string levelTransferId, string transferTo)
		{
			GameObject prefab = await _assetsProvider.Load<GameObject>(AssetAddress.LevelTransferAddress);
			
			LevelTransferTrigger transferTrigger = InstantiateRegistered(prefab, at).GetComponent<LevelTransferTrigger>();

			transferTrigger.Construct(_gameStateMachine);
			transferTrigger.TransfeTo = transferTo;
			return transferTrigger;
		}

		public async Task<SaveTrigger> CreateSaveTrigger(Vector2 at, string saveTriggerId)
		{
			GameObject prefab = await _assetsProvider.Load<GameObject>(AssetAddress.SaveTriggerAddress);
			
			SaveTrigger saveTrigger = InstantiateRegistered(prefab, at).GetComponent<SaveTrigger>();

			saveTrigger.Construct(_saveLoadService);
			saveTrigger.Id = saveTriggerId;
			return saveTrigger;
		}

		public void Cleanup()
		{
			ProgressReaders.Clear();
			ProgressWriters.Clear();

			_assetsProvider.CleanUp();
		}

		public void Register(ISavedProgressReader progressReader)
		{
			if (progressReader is ISavedProgress progressWriter)
				ProgressWriters.Add(progressWriter);

			ProgressReaders.Add(progressReader);
		}

		private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath)
		{
			GameObject gameObject = await _assetsProvider.Instantiate(prefabPath);

			RegisterProgressWatchers(gameObject);
			return gameObject;
		}

		private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath, Vector3 at)
		{
			GameObject gameObject = await _assetsProvider.Instantiate(prefabPath, at);

			RegisterProgressWatchers(gameObject);
			return gameObject;
		}

		private GameObject InstantiateRegistered(GameObject prefab)
		{
			GameObject gameObject = Object.Instantiate(prefab);
			RegisterProgressWatchers(gameObject);
			return gameObject;
		}

		private GameObject InstantiateRegistered(GameObject prefab, Vector3 at)
		{
			GameObject gameObject = Object.Instantiate(prefab, at, Quaternion.identity);

			RegisterProgressWatchers(gameObject);
			return gameObject;
		}

		private void RegisterProgressWatchers(GameObject hero)
		{
			foreach (ISavedProgressReader progressReader in hero.GetComponentsInChildren<ISavedProgressReader>())
			{
				Register(progressReader);
			}
		}

	}


}