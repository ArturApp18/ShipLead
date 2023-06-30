using UnityEngine;

namespace CodeBase.Infrastructure.Services.Randomizer
{
	public class UnityRandomService : IRandomService
	{
		public int Next(int lootMin, int lootMax) =>
			Random.Range(lootMin, lootMax);
	}
}