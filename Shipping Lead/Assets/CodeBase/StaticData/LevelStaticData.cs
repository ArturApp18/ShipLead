using System.Collections.Generic;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.StaticData
{
	[CreateAssetMenu(fileName = "LevelData", menuName = "StaticData/Level")]
	public class LevelStaticData : ScriptableObject
	{
		public string LevelKey;

		public List<EnemySpawnerData> EnemySpawners;
		
		public List<SaveTriggerData> SaveTriggers;
		public List<LevelTransferTriggerData> LevelTransfers;

		public Vector3 InitialHeroPosition;
	}

}