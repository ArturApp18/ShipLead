using System.Collections.Generic;
using System.Linq;
using CodeBase.Logic;
using CodeBase.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.StaticData
{
	public class StaticDataService : IStaticDataService
	{
		private const string StaticDataWindows = "StaticData/UI/WindowBase";
		private const string StaticDataHero = "StaticData/Hero/Hero";
		private const string StaticDataMonsters = "StaticData/Monsters";
		private const string StaticDataLevels = "StaticData/Levels";
		
		private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;
		private Dictionary<string, LevelStaticData> _levels;
		private Dictionary<WindowId, WindowConfig> _windowConfigs;

		private HeroStaticData _hero;

		public void LoadStaticData()
		{
			LoadHero();
			LoadMonsters();
			LoadLevel();
			LoadWindow();
		}

		private void LoadWindow()
		{
			_windowConfigs = Resources
				.Load<WindowStaticData>(StaticDataWindows)
				.Config
				.ToDictionary(x => x.WindowId, x => x);
		}

		public void LoadMonsters()
		{
			_monsters = Resources
				.LoadAll<MonsterStaticData>(StaticDataMonsters)
				.ToDictionary(x => x.MonsterTypeId, x => x);
			
		}

		public void LoadLevel()
		{
			_levels = Resources
				.LoadAll<LevelStaticData>(StaticDataLevels)
				.ToDictionary(x => x.LevelKey, x => x);
		}

		public void LoadHero() =>
			_hero = Resources.Load<HeroStaticData>(StaticDataHero);

		public WindowConfig ForWindow(WindowId windowId) =>
			_windowConfigs.TryGetValue(windowId, out WindowConfig windowConfig)
				? windowConfig
				: null;

		public LevelStaticData ForLevel(string sceneKey) =>
			_levels.TryGetValue(sceneKey, out LevelStaticData staticData)
				? staticData
				: null;

		public MonsterStaticData ForMonster(MonsterTypeId typeId) =>
			_monsters.TryGetValue(typeId, out MonsterStaticData staticData)
				? staticData
				: null;

		public HeroStaticData ForHero() =>
			_hero;

	}
}