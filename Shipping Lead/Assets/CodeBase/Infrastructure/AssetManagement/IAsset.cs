using UnityEngine;

namespace CodeBase.Infrastructure.AssetManagement
{
	public interface IAsset
	{
		GameObject Instantiate(string path);
		GameObject Instantiate(string path, Vector2 at);
	}
}