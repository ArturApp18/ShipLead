using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Shop
{
	public class RewardedItem : MonoBehaviour
	{

		public Button ShowAdButton;
		public GameObject[] AdActiveObject;
		public GameObject[] AdInactiveObject;
		private IAdsService _adsService;
		private IPersistentProgressService _progressService;

		public void Construct(IAdsService adsService, IPersistentProgressService progressService)
		{
			_adsService = adsService;
			_progressService = progressService;
		}

		public void Initialize()
		{
			ShowAdButton.onClick.AddListener(OnShowAdClicked);

			RefreshAvailableAd();
		}

		public void Subscribe() =>
			_adsService.RewardedVideoReady += RefreshAvailableAd;

		public void Cleanup() =>
			_adsService.RewardedVideoReady -= RefreshAvailableAd;

		private void OnShowAdClicked() =>
			_adsService.ShowRewardedVideo(OnVideoFinished);

		private void OnVideoFinished() =>
			_progressService.Progress.WorldData.LootData.Add(_adsService.Reward);

		private void RefreshAvailableAd()
		{
			bool videoReady = _adsService.IsRewardedVideoReady;

			foreach (GameObject adActiveObject in AdActiveObject)
				adActiveObject.SetActive(videoReady);

			foreach (GameObject adInactiveObject in AdInactiveObject)
				adInactiveObject.SetActive(!videoReady);
		}
	}
}