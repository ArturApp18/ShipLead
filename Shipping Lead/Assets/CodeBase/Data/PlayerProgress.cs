using System;
using CodeBase.Logic;

namespace CodeBase.Data
{
	[Serializable]
	public class PlayerProgress
	{
		public WorldData WorldData;
		public State HeroState;
		public Stats HeroStats;
		public SaveData SaveData;
		public KillData KillData;

		public PlayerProgress(string initialLevel)
		{
			WorldData = new WorldData(initialLevel);
			HeroState = new State();
			HeroStats = new Stats();
			SaveData = new SaveData();
			KillData = new KillData();
		}

	}

}