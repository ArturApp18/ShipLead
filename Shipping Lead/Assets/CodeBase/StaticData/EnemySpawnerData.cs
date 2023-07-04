using System;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.StaticData
{
	[Serializable]
	public class EnemySpawnerData
	{
		public string Id;
		public MonsterTypeId MonsterTypeId;
		public Vector2 Position;

		public EnemySpawnerData(string id, MonsterTypeId monsterTypeId, Vector2 position)
		{
			Id = id;
			MonsterTypeId = monsterTypeId;
			Position = position;
		}
	}
}