using System.Collections.Generic;
using System.Linq;
using CodeBase.Logic;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.StaticData
{
	public class StaticDataService : IStaticDataService
	{
		private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;
		private HeroStaticData _hero;

		public void LoadMonsters()
		{
			_monsters = Resources
				.LoadAll<MonsterStaticData>("StaticData/Monsters")
				.ToDictionary(x => x.MonsterTypeId, x => x);
			
			Debug.Log("nice");
		}

		public void LoadHero() =>
			_hero = Resources.Load<HeroStaticData>("StaticData/Hero/Hero");

		public MonsterStaticData ForMonster(MonsterTypeId typeId) =>
			_monsters.TryGetValue(typeId, out MonsterStaticData staticData)
				? staticData
				: null;

		public HeroStaticData ForHero()
		{
			Debug.Log("YOmAN");
			return _hero;
		}

	}
}