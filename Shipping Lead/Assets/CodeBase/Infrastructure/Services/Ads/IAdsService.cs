using System;

namespace CodeBase.Infrastructure.Services.Ads
{
	public interface IAdsService : IService
	{
		event Action RewardedVideoReady;
		void Initialize();
		bool IsRewardedVideoReady { get; }
		int Reward { get; }
		void ShowRewardedVideo(Action onVideoFinished);
	}
}