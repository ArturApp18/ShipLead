using UnityEngine;

namespace CodeBase
{
	public static class PhysicDebug
	{
		public static void DrawDebug(Vector2 worldPos, float radius, float seconds)
		{
			Debug.DrawRay(worldPos, radius * Vector2.up,Color.red, seconds);
			Debug.DrawRay(worldPos, radius * Vector2.down,Color.red, seconds);
			Debug.DrawRay(worldPos, radius * Vector2.left,Color.red, seconds);
			Debug.DrawRay(worldPos, radius * Vector2.right,Color.red, seconds);
		}
	}
}