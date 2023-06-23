using System;
using CodeBase.CameraLogic;
using CodeBase.Infrastructure;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Hero
{
	public class HeroMove : MonoBehaviour
	{
		[SerializeField] private Rigidbody2D _rigidbody;
		[SerializeField] private float _movementSpeed;

		private IInputService _inputService;
		private Camera _camera;

		private void Awake()
		{
			_inputService = Game.InputService;
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

		

	}
}