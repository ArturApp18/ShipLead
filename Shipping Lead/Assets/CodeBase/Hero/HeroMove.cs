using CodeBase.Data;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services.Input;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Hero
{
	public class HeroMove : MonoBehaviour, ISavedProgress
	{
		[SerializeField] private Rigidbody2D _rigidbody;
		[SerializeField] private CapsuleCollider2D _heroCollider;
		[SerializeField] private float _movementSpeed;

		private IInputService _inputService;
		private Camera _camera;

		private void Awake()
		{
			_inputService = AllServices.Container.Single<IInputService>();
		}

		private void Start()
		{
			_camera = Camera.main;
		}

		private void Update()
		{
			Vector2 movementVector = Vector2.zero;

			if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
			{
				movementVector = _camera.transform.TransformDirection(_inputService.Axis);
				movementVector.Normalize();
			}

			movementVector += Physics2D.gravity;
			
			_rigidbody.velocity  = new Vector2(_movementSpeed * movementVector.x * Time.deltaTime, _rigidbody.velocity.y);
		}


		public void UpdateProgress(PlayerProgress progress) =>
			progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(), transform.position.AsVectorData());

		public void LoadProgress(PlayerProgress progress)
		{
			if (CurrentLevel() == progress.WorldData.PositionOnLevel.Level)
			{
				Vector3Data savedPosition = progress.WorldData.PositionOnLevel.Position;
				if (savedPosition != null)
					Warp(to: savedPosition);
			}
		}

		private void Warp(Vector3Data to) =>
			transform.position = to.AsUnityVector().AddY(_heroCollider.size.y);

		private static string CurrentLevel() =>
			SceneManager.GetActiveScene().name;
	}

}