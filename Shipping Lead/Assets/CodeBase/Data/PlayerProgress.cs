using System;
using System.Collections.Generic;
using CodeBase.Logic;

namespace CodeBase.Data
{
	[Serializable]
	public class PlayerProgress
	{
		public WorldData WorldData;
		public State HeroState;
		public Stats HeroStats;

		public PlayerProgress(string initialLevel)
		{
			WorldData = new WorldData(initialLevel);
			HeroState = new State();
			HeroStats = new Stats();
		}

	}

}