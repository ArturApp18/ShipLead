using System;
using UnityEngine;
using UnityEngine.Advertisements;


namespace CodeBase.Infrastructure.Services.Ads
{
	public class AdsService : IAdsService, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
	{
		private const string RewardedVideoPlacementId = "rewardedVideo";
		private const string AndroidGameId = "5335486";
		private const string IOSGameId = "5335487";

		public event Action RewardedVideoReady;


		public bool IsRewardedVideoReady { get; set; }
		public int Reward => 13; 
		
		private string _gameId;
		private Action _onVideoFinished;

		private const bool TestMod = true;


		public void Initialize()
		{
			switch (Application.platform)
			{
				case RuntimePlatform.Android:
					_gameId = AndroidGameId;
					break;
				case RuntimePlatform.IPhonePlayer :
					_gameId = IOSGameId;
					break;
				case RuntimePlatform.WindowsEditor:
					_gameId = IOSGameId;
					break;
				default:
					Debug.Log("Unsupported platform for ads");
					break;
			}
			
			Advertisement.Initialize(_gameId, TestMod, this);
		}

		public void ShowRewardedVideo(Action onVideoFinished)
		{
			Advertisement.Show(RewardedVideoPlacementId, this);
			_onVideoFinished = onVideoFinished;
		}

		public void OnInitializationComplete()
		{
			IsRewardedVideoReady = true;
		}

		public void OnInitializationFailed(UnityAdsInitializationError error, string message)
		{
			IsRewardedVideoReady = false;
		}

		public void OnUnityAdsAdLoaded(string placementId)
		{
			Debug.Log($"OnUnityAdsAdLoaded {placementId}");

			if (placementId == RewardedVideoPlacementId)
				RewardedVideoReady?.Invoke();
		}

		public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) =>
			Debug.Log($"OnUnityAdsFailedToLoad {message}");

		public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) =>
			Debug.Log($"OnUnityAdsShowFailure {message}");

		public void OnUnityAdsShowStart(string placementId) =>
			Debug.Log($"OnUnityAdsShowStart {placementId}");

		public void OnUnityAdsShowClick(string placementId) =>
			Debug.Log($"OnUnityAdsShowClick {placementId}");

		public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
		{
			switch (showCompletionState)
			{
				case UnityAdsShowCompletionState.SKIPPED:
					Debug.LogError($"Skipped {placementId}");
					break;
				case UnityAdsShowCompletionState.COMPLETED:
					_onVideoFinished?.Invoke();
					break;
				case UnityAdsShowCompletionState.UNKNOWN:
					break;
				default:
					Debug.LogError($"Default {placementId}");
					break;
			}

			_onVideoFinished = null;
		}
	}
}