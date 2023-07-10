using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows.Shop;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
	public class UIFactory : IUIFactory
	{
		private const string UIRootAddress = "UIRoot";

		private readonly IAssetsProvider _assetsProvider;
		private readonly IStaticDataService _staticData;
		private readonly IPersistentProgressService _progressService;
		private readonly IAdsService _adsService;

		private Transform _uiRoot;

		public UIFactory(IAssetsProvider assetsProvider, IStaticDataService staticData, IPersistentProgressService progressService, IAdsService adsService)
		{
			_assetsProvider = assetsProvider;
			_staticData = staticData;
			_progressService = progressService;
			_adsService = adsService;
		}

		public void CreateShop()
		{
			WindowConfig config = _staticData.ForWindow(WindowId.Shop);
			ShopWindow window = Object.Instantiate(config.Prefab, _uiRoot) as ShopWindow;
			window.Construct(_adsService, _progressService);
		}

		public async Task CreateUIRoot()
		{
			GameObject root = await _assetsProvider.Instantiate(UIRootAddress);
			_uiRoot = root.transform;
		}

		public void CreateHeroStats()
		{
			throw new System.NotImplementedException();
		}
	}
}