using System;
using UnityEngine;

namespace CodeBase.Hero
{
	public class HeroCollision : MonoBehaviour
	{
		[SerializeField] private LayerMask _groundLayer;
		[SerializeField] private HeroAnimator _animator;
		[SerializeField] private float _groundRaycastLength;
		[SerializeField] private bool _isGrounded;

		public event Action OnGrounded;
		public bool IsGrounded
		{
			get
			{
				return _isGrounded;
			}
			set
			{
				if (_isGrounded)
				{
					OnGrounded?.Invoke();
				}
				_isGrounded = value;
			}
		}

		private void Update()
		{
			CheckCollision();
		}

		private void CheckCollision()
		{
			IsGrounded = Physics2D.Raycast(transform.position, Vector2.down, _groundRaycastLength, _groundLayer);
			_animator.IsGrounded = _isGrounded;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color= Color.green;
			Gizmos.DrawLine(transform.position, transform.position + Vector3.down * _groundRaycastLength);
		}
	}
}