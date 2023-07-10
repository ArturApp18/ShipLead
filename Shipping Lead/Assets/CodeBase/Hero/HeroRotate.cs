using System;
using CodeBase.Infrastructure;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Input;
using UnityEngine;

namespace CodeBase.Hero
{
	public class HeroRotate : MonoBehaviour
	{
		[SerializeField] private Rigidbody2D _rigidbody2D;
		[SerializeField] private bool _isFacingRight;
		
		private IInputService _inputService;

		public void Construct(IInputService inputService)
		{
			_inputService = inputService;
		}
		private void Update()
		{
			if (_inputService.Axis.x < 0 && _isFacingRight)
			{
				transform.Rotate(0,180,0);
				_isFacingRight = !_isFacingRight;
			}
			else if (_inputService.Axis.x > 0 && !_isFacingRight)
			{
				transform.Rotate(0, 180,0 );
				_isFacingRight = !_isFacingRight;
			}
		}
	}
}