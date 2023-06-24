using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using UnityEngine;

namespace CodeBase.Logic
{
	public class SaveTrigger : MonoBehaviour
	{
		private IPersistentProgressService _progressService;
		private ISaveLoadService _saveLoadService;
		public BoxCollider2D Collider;

		private void Awake()
		{
			_progressService = AllServices.Container.Single<IPersistentProgressService>();
			_saveLoadService = AllServices.Container.Single<ISaveLoadService>();
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			_saveLoadService.SaveProgress();
			Debug.Log("Progress Saved");
			gameObject.SetActive(false);
		}

		private void OnDrawGizmos()
		{
			if (!Collider)
				return;

			Gizmos.color = new Color(0.17f, 0.75f, 0.02f, 0.42f);
			Gizmos.DrawCube(transform.position, Collider.size);
		}
	}
}