using CodeBase.Data;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using UnityEngine;

namespace CodeBase.Logic
{
	public class SaveTrigger : MonoBehaviour, ISavedProgress
	{
		private ISaveLoadService _saveLoadService;
		public BoxCollider2D Collider;

		public string Id { get; set; }
		public bool Saved
		{
			get
			{
				return _saved;
			}
			set
			{
				_saved = value;
			}
		}

		private bool _saved;

		public void Construct(ISaveLoadService saveLoadService)
		{
			_saveLoadService = saveLoadService;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!Saved)
			{
				Saved = true;
				_saveLoadService.SaveProgress();
				Debug.Log("Progress Saved");
				gameObject.SetActive(false);
			}
		}


		private void OnDrawGizmos()
		{
			if (!Collider)
				return;

			Gizmos.color = new Color(0.17f, 0.75f, 0.02f, 0.42f);
			Gizmos.DrawCube(transform.position, Collider.size);
		}

		public void LoadProgress(PlayerProgress progress)
		{
			if (progress.SaveData.SavedTriggers.Contains(Id))
				Saved = true;
		}

		public void UpdateProgress(PlayerProgress progress)
		{
			if (Saved)
				progress.SaveData.SavedTriggers.Add(Id);
		}

	}
}