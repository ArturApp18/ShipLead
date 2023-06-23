using System;
using UnityEngine;

namespace CodeBase.CameraLogic
{
	public class CameraFollow : MonoBehaviour
	{
		[SerializeField] private Transform _following;
		[SerializeField] private float _offset;

		private void LateUpdate()
		{
			if (_following == null)
				return;

			Vector2 followingPosition = FollowingPointPosition();

			transform.position = followingPosition;
		}

		public void Follow(GameObject target) =>
			_following = target.transform;

		private Vector2 FollowingPointPosition()
		{
			Vector3 followingPosition = _following.position;
			followingPosition.z += _offset;
			
			return followingPosition;
		}
	}
}