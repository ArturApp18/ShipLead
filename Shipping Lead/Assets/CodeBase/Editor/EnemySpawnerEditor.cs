using CodeBase.Logic.EnemySpawners;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
	[CustomEditor(typeof(EnemySpawnMarker))]
	public class EnemySpawnerEditor : UnityEditor.Editor
	{
		[DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
		public static void RenderCustomGizmo(EnemySpawnMarker enemySpawnPoint, GizmoType gizmo)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(enemySpawnPoint.transform.position, 0.5f);
		}
	}
}