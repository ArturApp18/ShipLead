namespace CodeBase.Infrastructure.Services.Randomizer
{
	public interface IRandomService : IService
	{
		int Next(int lootMin, int lootMax);
	}
}