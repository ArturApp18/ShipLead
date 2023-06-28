using CodeBase.Logic;
using CodeBase.StaticData;

namespace CodeBase.Infrastructure.Services.StaticData
{
	public interface IStaticDataService : IService
	{
		MonsterStaticData ForMonster(MonsterTypeId typeId);
		HeroStaticData ForHero();
		void LoadMonsters();
		void LoadHero();
	}
}