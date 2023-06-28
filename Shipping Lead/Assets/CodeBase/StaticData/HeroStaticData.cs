using UnityEngine;

namespace CodeBase.StaticData
{
	[CreateAssetMenu(fileName = "HeroData", menuName = "StaticData/Hero")]
	public class HeroStaticData : ScriptableObject
	{
		public int Hp;
		public float Damage;

		public float Cleavage;

		public GameObject Prefab;
	}
}