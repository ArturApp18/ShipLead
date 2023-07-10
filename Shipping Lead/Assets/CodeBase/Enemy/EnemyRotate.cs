using UnityEngine;

namespace CodeBase.Enemy
{
	public class EnemyRotate : MonoBehaviour
	{
		[SerializeField] private bool _isFacingRight;
		public bool IsFacingRight
		{
			get
			{
				return _isFacingRight;
			}
			set
			{
				_isFacingRight = value;
			}
		}

		public void Flip()
		{
			transform.Rotate(0, 180,0);
			IsFacingRight = !IsFacingRight;
		}
	}
}