using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.Randomizer;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.StaticData;
using CodeBase.UI;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using Object = UnityEngine.Object;


namespace CodeBase.Infrastructure.Factory
{
	public class GameFactory : IGameFactory
	{

		private readonly IAssets _assets;
		private readonly IStaticDataService _staticData;
		private readonly IRandomService _randomService;
		private readonly IPersistentProgressService _progressService;
		private readonly ISaveLoadService _saveLoadService;
		private readonly IWindowService _windowService;

		public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
		public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

		private GameObject HeroGameObject { get; set; }

		public GameFactory(IAssets assets, IStaticDataService staticData, IRandomService randomService, IPersistentProgressService progressService, IWindowService windowService)
		{
			_assets = assets;
			_staticData = staticData;
			_randomService = randomService;
			_progressService = progressService;
			_windowService = windowService;
		}

		public GameObject CreateHero(GameObject at)
		{
			HeroGameObject = InstantiateRegistered(AssetPath.HeroPath, at.transform.position);
			
			return HeroGameObject;
		}

		public GameObject CreateHud()
		{
			GameObject hud = InstantiateRegistered(AssetPath.HUDPath);

			
			hud.GetComponentInChildren<LootCounter>()
				.Construct(_progressService.Progress.WorldData);
			foreach (OpenWindowButton openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>())
			{
				openWindowButton.Construct(_windowService);
			}
			return hud;
		}

		public GameObject CreateMonster(MonsterTypeId typeId, Transform parent)
		{
			MonsterStaticData monsterData = _staticData.ForMonster(typeId);
			GameObject monster = Object.Instantiate(monsterData.Prefab, parent.position, Quaternion.identity, parent);

			var health = monster.GetComponent<IHealth>();
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

		public LootPiece CreateLoot()
		{
			LootPiece lootPiece = InstantiateRegistered(AssetPath.LootPath).GetComponent<LootPiece>();
			string id = lootPiece.GetComponent<UniqueId>().Id;
			lootPiece.Construct(_progressService.Progress.WorldData, id);

			return lootPiece;
		}

		public void CreateSpawner(Vector2 at, string spawnerId, MonsterTypeId monsterTypeId)
		{
			EnemySpawnPoint spawner = InstantiateRegistered(AssetPath.SpawnerPath, at)
				.GetComponent<EnemySpawnPoint>();

			spawner.Construct(this);
			spawner.Id = spawnerId;
			spawner.MonsterTypeId = monsterTypeId;
		}

		public SaveTrigger CreateSaveTrigger(Vector2 at, string saveTriggerId)
		{
			SaveTrigger saveTrigger = InstantiateRegistered(AssetPath.SaveTriggerPath, at)
				.GetComponent<SaveTrigger>();
			
			saveTrigger.Id = saveTriggerId;
			return saveTrigger;
		}

		public void Cleanup()
		{
			ProgressReaders.Clear();
			ProgressWriters.Clear();
		}

		public void Register(ISavedProgressReader progressReader)
		{
			if (progressReader is ISavedProgress progressWriter)
				ProgressWriters.Add(progressWriter);

			ProgressReaders.Add(progressReader);
		}

		private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
		{
			GameObject gameObject = _assets.Instantiate(prefabPath, at);

			RegisterProgressWatchers(gameObject);
			return gameObject;
		}

		private GameObject InstantiateRegistered(string prefabPath)
		{
			GameObject gameObject = _assets.Instantiate(prefabPath);


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