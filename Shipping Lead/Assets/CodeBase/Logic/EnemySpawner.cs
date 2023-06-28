using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Logic
{
	public class EnemySpawner : MonoBehaviour, ISavedProgress
	{
		public MonsterTypeId MonsterTypeId;

		private string _id;
		public bool _slain;
		private IGameFactory _factory;
		private EnemyDeath _enemyDeath;

		private void Awake()
		{
			_id = GetComponent<UniqueId>().Id;
			_factory = AllServices.Container.Single<IGameFactory>();
		}

		public void LoadProgress(PlayerProgress progress)
		{
			if (progress.KillData.ClearSpawners.Contains(_id))
				_slain = true;
			else
				Spawn();
		}

		private void Spawn()
		{
			GameObject monster = _factory.CreateMonster(MonsterTypeId, transform);
			_enemyDeath = monster.GetComponent<EnemyDeath>();
			_enemyDeath.Happend += Slay;
		}

		private void Slay()
		{
			if (_enemyDeath != null)
				_enemyDeath.Happend -= Slay;

			_slain = true;
		}

		public void UpdateProgress(PlayerProgress progress)
		{
			if (_slain)
				progress.KillData.ClearSpawners.Add(_id);
		}
	}

}