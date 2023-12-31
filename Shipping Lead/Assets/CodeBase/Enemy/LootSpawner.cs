using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.Randomizer;
using UnityEngine;

namespace CodeBase.Enemy
{
	public class LootSpawner : MonoBehaviour
	{
		[SerializeField] private EnemyDeath _enemyDeath;
		private IGameFactory _factory;
		private IRandomService _random;
		private int _lootMin;
		private int _lootMax;

		public void Construct(IGameFactory factory, IRandomService random)
		{
			_factory = factory;
			_random = random;
		}

		private void Start()
		{
			_enemyDeath.Happend += SpawnLoot;
		}

		private async void SpawnLoot()
		{
			LootPiece loot = await _factory.CreateLoot();
			loot.transform.position = transform.position;

			Loot lootItem = GenerateLoot();

			loot.Initialize(lootItem);
		}

		private Loot GenerateLoot()
		{
			return new Loot 
			{
				Value = _random.Next(_lootMin, _lootMax),
			};
		}

		public void SetLoot(int min, int max)
		{
			_lootMin = min;
			_lootMax = max;
		}
	}

}