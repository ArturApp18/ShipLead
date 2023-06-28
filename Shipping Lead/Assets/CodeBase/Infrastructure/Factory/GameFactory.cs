using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.Logic;
using CodeBase.StaticData;
using UnityEngine;
using Object = UnityEngine.Object;


namespace CodeBase.Infrastructure.Factory
{
	public class GameFactory : IGameFactory
	{

		private readonly IAssets _assets;
		private readonly IStaticDataService _staticData;

		public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
		public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

		private GameObject HeroGameObject { get; set; }

		public GameFactory(IAssets assets, IStaticDataService staticData)
		{
			_assets = assets;
			_staticData = staticData;
		}

		public GameObject CreateHero(GameObject at)
		{
			//HeroStaticData heroData = _staticData.ForHero();
			HeroGameObject = InstantiateRegistered(AssetPath.HeroPath, at.transform.position);

			//var health = HeroGameObject.GetComponent<IHealth>();
			//health.Current = heroData.Hp;
			//health.Max = heroData.Hp;
			return HeroGameObject;
		}

		public GameObject CreateHud() =>
			InstantiateRegistered(AssetPath.HUDPath);

		public GameObject CreateMonster(MonsterTypeId typeId, Transform parent)
		{
			MonsterStaticData monsterData = _staticData.ForMonster(typeId);
			GameObject monster = Object.Instantiate(monsterData.Prefab, parent.position, Quaternion.identity, parent);

			var health = monster.GetComponent<IHealth>();
			health.Current = monsterData.Hp;
			health.Max = monsterData.Hp;

			monster.GetComponent<MoveToHero>().Construct(HeroGameObject.transform);
			monster.GetComponent<MoveToHero>().MovementSpeed = monsterData.MoveSpeed;

			EnemyAttack attack = monster.GetComponent<EnemyAttack>();
			attack.Construct(HeroGameObject.transform);
			attack.Damage = monsterData.Damage;
			attack.Cleavage = monsterData.Cleavage;
			attack.EffectiveDistance = monsterData.EffeectiveDistance;

			monster.GetComponent<RotateToHero>()?.Construct(HeroGameObject.transform);
			 
			return monster;
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