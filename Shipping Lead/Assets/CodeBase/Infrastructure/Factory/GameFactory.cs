using CodeBase.Infrastructure.AssetManagement;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
	public class GameFactory : IGameFactory
	{

		private readonly IAsset _assets;

		public GameFactory(IAsset assets)
		{
			_assets = assets;
		}

		public GameObject CreateHero(GameObject at) =>
			_assets.Instantiate(AssetPath.HeroPath, at: at.transform.position);

		public void CreateHud() =>
			_assets.Instantiate(AssetPath.HUDPath);

	}

}