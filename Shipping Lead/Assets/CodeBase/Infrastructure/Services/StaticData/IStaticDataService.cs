using CodeBase.Logic;
using CodeBase.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;

namespace CodeBase.Infrastructure.Services.StaticData
{
	public interface IStaticDataService : IService
	{
		MonsterStaticData ForMonster(MonsterTypeId typeId);
		HeroStaticData ForHero();
		void LoadMonsters();
		void LoadHero();
		LevelStaticData ForLevel(string sceneKey);
		void LoadStaticData();
		WindowConfig ForWindow(WindowId windowId);
	}

}