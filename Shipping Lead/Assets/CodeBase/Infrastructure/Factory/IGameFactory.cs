using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
	public interface IGameFactory : IService
	{
		Task CreateSpawner(Vector2 at, string spawnerId, MonsterTypeId monsterTypeId);
		Task<SaveTrigger> CreateSaveTrigger(Vector2 at, string saveTriggerId);
		Task<GameObject> CreateHero(Vector3 at);
		Task<GameObject> CreateHud();
		List<ISavedProgressReader> ProgressReaders { get; }
		List<ISavedProgress> ProgressWriters { get; }
		void Cleanup();
		Task<GameObject> CreateMonster(MonsterTypeId typeId, Transform parent);
		Task<LootPiece> CreateLoot();
		Task<LevelTransferTrigger> CreateLevelTransferTrigger(Vector2 at, string levelTransferId, string transferTo);
		Task WarmUp();
	}
}