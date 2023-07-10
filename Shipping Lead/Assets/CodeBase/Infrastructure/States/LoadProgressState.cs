using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.StaticData;

namespace CodeBase.Infrastructure.States
{
	public class LoadProgressState : IState
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly IPersistentProgressService _progressService;
		private readonly ISaveLoadService _saveLoadService;
		private readonly IStaticDataService _staticData;

		public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadService, IStaticDataService staticData)
		{
			_gameStateMachine = gameStateMachine;
			_progressService = progressService;
			_saveLoadService = saveLoadService;
			_staticData = staticData;
		}

		public void Enter()
		{
			LoadProgressOrInitNew();
			_gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.PositionOnLevel.Level);
		}

		public void Update() { }

		public void Exit() { }

		private void LoadProgressOrInitNew() =>
			_progressService.Progress = 
				_saveLoadService.LoadProgress() 
				?? NewProgress();

		private PlayerProgress NewProgress()
		{
			PlayerProgress progress = new PlayerProgress(initialLevel: "Main");

			HeroStaticData heroData = _staticData.ForHero();
			
			progress.HeroState.MaxHP = heroData.Hp;
			progress.HeroStats.Damage = heroData.Damage;
			progress.HeroStats.DamageRadius = heroData.Cleavage;
			progress.HeroStats.MaxMovementSpeed = heroData.MaxMoveSpeed;
			progress.HeroStats.MovementAccelaration = heroData.MovementAccelaration;
			progress.HeroStats.MovementDeacclaration = heroData.MovementDeacclaration;
			progress.HeroStats.JumpForce = heroData.JumpForce;
			progress.HeroStats.AirLinearDrag = heroData.AirLinearDrag;
			progress.HeroStats.FallMultiplier = heroData.FallMultiplier;
			progress.HeroStats.LowMultiplier = heroData.LowJumpMultiplier;
			progress.HeroStats.ExtraJumps = heroData.ExtraJumps;
			progress.HeroStats.VelocityPower = heroData.VelocityPower;
			progress.HeroState.ResetHP();

			return progress;
		}

	}

}