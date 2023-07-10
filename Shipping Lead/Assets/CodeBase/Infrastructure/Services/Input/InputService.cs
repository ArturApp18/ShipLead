using UnityEngine;

namespace CodeBase.Infrastructure.Services.Input
{
	public abstract class InputService : IInputService
	{
		protected const string Horizontal = "Horizontal";
		protected const string Vertical = "Vertical";
		private const string Fire = "Fire1";
		private const string Jump = "Jump";

		public abstract Vector2 Axis { get; }

		public bool IsAttackButtonUp() =>
			SimpleInput.GetButtonUp(Fire);

		public bool IsJumpButtonDown() =>
			SimpleInput.GetButtonDown(Jump);

		public bool IsJumpButtonUp()
			=>
				SimpleInput.GetButtonUp(Jump);

		public bool IsJumpButton() =>
			SimpleInput.GetButton(Jump);

		protected static Vector2 SimpleInputAxis() =>
			new Vector2(SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));
	}


}