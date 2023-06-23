using UnityEngine;

namespace CodeBase.Infrastructure.AssetManagement
{
	public class AssetProvider : IAsset
	{

		public GameObject Instantiate(string path)
		{
			GameObject prefab = Resources.Load<GameObject>(path);
			return Object.Instantiate(prefab);
		}

		public GameObject Instantiate(string path, Vector2 at)
		{
			GameObject prefab = Resources.Load<GameObject>(path);
			return Object.Instantiate(prefab, at, Quaternion.identity);
		}
	}
}