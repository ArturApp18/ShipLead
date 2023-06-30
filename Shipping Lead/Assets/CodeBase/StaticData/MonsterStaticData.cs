using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.StaticData
{
	[CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster")]
	public class MonsterStaticData : ScriptableObject
	{
		public MonsterTypeId MonsterTypeId;
		
		public int MaxLoot;
		
		public int MinLoot;
		
		[Range(1, 100)]
		public int Hp;
		
		[Range(1, 30)]
		public float Damage;
		
		[Range(1, 10)]
		public float MoveSpeed;

		[Range(0.5f, 1)]
		public float EffeectiveDistance;
		
		[Range(0.5f, 1)]
		public float Cleavage;

		public GameObject Prefab;
	}

}