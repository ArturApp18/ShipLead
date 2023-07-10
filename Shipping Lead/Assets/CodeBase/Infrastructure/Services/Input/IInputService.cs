using UnityEngine;

namespace CodeBase.Infrastructure.Services.Input
{
	public interface IInputService : IService
	{
		Vector2 Axis { get; }

		bool IsAttackButtonUp();
		bool IsJumpButtonDown();
		bool IsJumpButtonUp();
		bool IsJumpButton();
	}

}