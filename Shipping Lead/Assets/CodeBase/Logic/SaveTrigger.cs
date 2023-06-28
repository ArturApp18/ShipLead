using System;
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

		private string _id;

		public bool _saved;

		private void Awake()
		{
			AllServices.Container.Single<IPersistentProgressService>();
			_saveLoadService = AllServices.Container.Single<ISaveLoadService>();

			_id = GetComponent<UniqueId>().Id;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{

			if (!_saved)
			{
				_saved = true;
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
			if (progress.SaveData.SavedTriggers.Contains(_id))
			{
				_saved = true;
			}
		}

		public void UpdateProgress(PlayerProgress progress)
		{
			if (_saved)
				progress.SaveData.SavedTriggers.Add(_id);
		}
	}
}