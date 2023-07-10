using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Logic.EnemySpawners
{
	public class EnemySpawnPoint : MonoBehaviour, ISavedProgress
	{
		public MonsterTypeId MonsterTypeId;
		
		public bool _slain;
		private IGameFactory _factory;
		private EnemyDeath _enemyDeath;
		public string Id { get; set; }

		public void Construct(IGameFactory factory)
		{
			_factory = factory;
		}

		public void LoadProgress(PlayerProgress progress)
		{
			if (progress.KillData.ClearSpawners.Contains(Id))
				_slain = true;
			else
				Spawn();
		}

		public void UpdateProgress(PlayerProgress progress)
		{
			if (_slain)
				progress.KillData.ClearSpawners.Add(Id);
		}

		private async void Spawn()
		{
			GameObject monster = await _factory.CreateMonster(MonsterTypeId, transform);
			_enemyDeath = monster.GetComponent<EnemyDeath>();
			_enemyDeath.Happend += Slay;
		}

		private void Slay()
		{
			if (_enemyDeath != null)
				_enemyDeath.Happend -= Slay;

			_slain = true;
		}

	}

}