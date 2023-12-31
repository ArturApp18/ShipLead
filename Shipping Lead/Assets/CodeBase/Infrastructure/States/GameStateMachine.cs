using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.Logic;
using CodeBase.UI.Services.Factory;

namespace CodeBase.Infrastructure.States
{
	public interface IGameStateMachine : IService
	{
		void Enter<TState>() where TState : class, IState;
		void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>;
		void Update();
	}

	public class GameStateMachine : IGameStateMachine
	{
		private readonly Dictionary<Type, IExitableState> _states;
		private IExitableState _activeState;

		public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, AllServices allServices)
		{
			_states = new Dictionary<Type, IExitableState>() {
				[typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, allServices),
				[typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, loadingCurtain, allServices.Single<IGameFactory>(),
					allServices.Single<IPersistentProgressService>(), allServices.Single<IStaticDataService>(), allServices.Single<ISaveLoadService>(), allServices.Single<IUIFactory>()),
				[typeof(LoadProgressState)] = new LoadProgressState(this, allServices.Single<IPersistentProgressService>(), allServices.Single<ISaveLoadService>(),
					allServices.Single<IStaticDataService>()),
				[typeof(GameLoopState)] = new GameLoopState(this, sceneLoader, loadingCurtain),
			};
		}

		public void Enter<TState>() where TState : class, IState
		{
			IState state = ChangeState<TState>();
			state.Enter();
		}

		public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
		{
			TState state = ChangeState<TState>();

			state.Enter(payload);
		}

		public void Update()
		{
			_activeState?.Update();
		}

		private TState ChangeState<TState>() where TState : class, IExitableState
		{
			_activeState?.Exit();

			TState state = GetState<TState>();
			_activeState = state;

			return state;
		}

		private TState GetState<TState>() where TState : class, IExitableState =>
			_states[typeof(TState)] as TState;

	}

}