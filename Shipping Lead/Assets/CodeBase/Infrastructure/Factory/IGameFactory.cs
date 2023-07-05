using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
	public interface IGameFactory : IService
	{
		void CreateSpawner(Vector2 at, string spawnerId, MonsterTypeId monsterTypeId);
		SaveTrigger CreateSaveTrigger(Vector2 at, string saveTriggerId);
		GameObject CreateHero(Vector3 at);
		GameObject CreateHud();
		List<ISavedProgressReader> ProgressReaders { get; }
		List<ISavedProgress> ProgressWriters { get; }
		void Cleanup();
		GameObject CreateMonster(MonsterTypeId typeId, Transform parent);
		LootPiece CreateLoot();
		LevelTransferTrigger CreateLevelTransferTrigger(Vector2 at, string levelTransferId, string transferTo);
	}
}