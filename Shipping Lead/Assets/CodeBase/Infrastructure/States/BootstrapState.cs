using System;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.Randomizer;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.Services.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
	public class BootstrapState : IState, IDisposable
	{
		private const string Initial = "Initial";
		private readonly GameStateMachine _stateMachine;
		private readonly SceneLoader _sceneLoader;
		private readonly AllServices _services;

		public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
		{
			_stateMachine = stateMachine;
			_sceneLoader = sceneLoader;
			_services = services;

			RegisterServices();
		}

		public void Enter() =>
			_sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);

		public void Exit() { }

		public void Update()
		{
			Debug.Log("lox");
		}

		private void EnterLoadLevel() =>
			_stateMachine.Enter<LoadProgressState>();

		private void RegisterServices()
		{
			RegisterStaticData();
			RegisterRandomService();
			_services.RegisterSingle(InputServices());
			_services.RegisterSingle<IAssets>(new AssetsProvider());
			_services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
			_services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssets>(), _services.Single<IStaticDataService>(), _services.Single<IRandomService>(),
				_services.Single<IPersistentProgressService>()));

			_services.RegisterSingle<ISaveLoadService>(new SaveLoadService(_services.Single<IPersistentProgressService>(), _services.Single<IGameFactory>()));
		}

		private void RegisterRandomService()
		{
			IRandomService randomService = new UnityRandomService();
			_services.RegisterSingle(randomService);
		}

		private void RegisterStaticData()
		{
			IStaticDataService staticData = new StaticDataService();
			staticData.LoadMonsters();
			staticData.LoadHero();
			_services.RegisterSingle(staticData);
		}

		private IInputService InputServices()
		{
			return Application.isEditor
				? new StandaloneInputService()
				: new MobileInputService();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}

}