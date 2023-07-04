using CodeBase.Logic;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
	[CustomEditor(typeof(SaveTriggerMarker))]
	public class SaveTriggerEditor : UnityEditor.Editor
	{
		[DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
		public static void RenderCustomGizmo(SaveTriggerMarker saveTriggerPoint, GizmoType gizmo)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(saveTriggerPoint.transform.position, 0.5f);
		}
	}
}