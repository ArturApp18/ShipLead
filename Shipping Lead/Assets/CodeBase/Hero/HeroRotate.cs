using System;
using CodeBase.Infrastructure;
using UnityEngine;

namespace CodeBase.Hero
{
	public class HeroRotate : MonoBehaviour
	{
		[SerializeField] private Rigidbody2D _rigidbody2D;
		[SerializeField] private bool _isFacingRight;

		private void Update()
		{
			if (Game.InputService.Axis.x < 0 && _isFacingRight)
			{
				transform.Rotate(0,180,0);
				_isFacingRight = !_isFacingRight;
			}
			else if (Game.InputService.Axis.x > 0 && !_isFacingRight)
			{
				transform.Rotate(0, 180,0 );
				_isFacingRight = !_isFacingRight;
			}
		}
	}
}