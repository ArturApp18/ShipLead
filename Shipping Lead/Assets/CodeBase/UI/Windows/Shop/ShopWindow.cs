using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.PersistentProgress;
using TMPro;

namespace CodeBase.UI.Windows.Shop
{
	public class ShopWindow : BaseWindow
	{
		public TextMeshProUGUI CoinText;
		public RewardedItem AdItem;

		public void Construct(IAdsService adsService, IPersistentProgressService progressService)
		{
			base.Construct(progressService);
			AdItem.Construct(adsService, progressService);
		}

		protected override void Initialize()
		{
			AdItem.Initialize();
			RefreshCoinText();
		}

		protected override void SubscribeUpdates()
		{
			AdItem.Subscribe();
			Progress.WorldData.LootData.Changed += RefreshCoinText;
		}

		protected override void Cleanup()
		{
			base.Cleanup();
			AdItem.Cleanup();
			Progress.WorldData.LootData.Changed -= RefreshCoinText;
		}

		private void RefreshCoinText() =>
			CoinText.text = Progress.WorldData.LootData.Collected.ToString();
	}
}