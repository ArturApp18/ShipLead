using System.Linq;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Editor
{
	[CustomEditor(typeof(LevelStaticData))]
	public class LevelStaticDataEditor : UnityEditor.Editor
	{
		private const string PlayerInitialPoint = "PlayerInitialPoint";
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			LevelStaticData levelData = (LevelStaticData) target;

			if (GUILayout.Button("Collect"))
			{
				levelData.EnemySpawners = 
					FindObjectsOfType<EnemySpawnMarker>()
					.Select(x => new EnemySpawnerData(x.GetComponent<UniqueId>().Id, x.MonsterTypeId, x.transform.position))
					.ToList();

				levelData.SaveTriggers =
					FindObjectsOfType<SaveTriggerMarker>()
						.Select(x => new SaveTriggerData(x.GetComponent<UniqueId>().Id, x.transform.position))
						.ToList();
				
				levelData.LevelTransfers =
					FindObjectsOfType<LevelTransferTriggerMarker>()
						.Select(x => new LevelTransferTriggerData(x.GetComponent<UniqueId>().Id, x.transform.position))
						.ToList();
				
				levelData.LevelKey = SceneManager.GetActiveScene().name;

				levelData.InitialHeroPosition = GameObject.FindWithTag(PlayerInitialPoint).transform.position;
			}
			
			EditorUtility.SetDirty(target);
		}
	}

}