using System.Collections;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using TMPro;
using UnityEngine;

namespace CodeBase.Enemy
{
	public class LootPiece : MonoBehaviour, ISavedProgress
	{
		public float _destroyTimer;
		
		public GameObject Sprite;
		public GameObject PickupFxPrefab;
		public TextMeshPro LootText;
		public GameObject PickUpPopup;

		private Loot _loot;
		private WorldData _worldData;
		
		private bool _picked;

		public string Id;
		public bool Picked
		{
			get
			{
				return _picked;
			}
			set
			{
				_picked = value;
			}
		}

		public void Construct(WorldData worldData)
		{
			_worldData = worldData;
		}

		public void Initialize(Loot loot)
		{
			_loot = loot;
		}

		private void OnTriggerEnter2D(Collider2D col) =>
			PickUp();

		private void PickUp()
		{
			if (Picked)
				return;

			Picked = true;

			UpdateWorldData();
			HideSprite();
			PlayPickUpFx();
			ShowText();
			StartCoroutine(StartDestroyTimer());
		}

		private void UpdateWorldData() =>
			_worldData.LootData.Collect(_loot);

		private void HideSprite() =>
			Sprite.SetActive(false);

		private void PlayPickUpFx() =>
			Instantiate(PickupFxPrefab, transform.position, Quaternion.identity);

		private void ShowText()
		{
			LootText.text = $"{_loot.Value}";
			PickUpPopup.SetActive(true);
		}

		private IEnumerator StartDestroyTimer()
		{
			yield return new WaitForSeconds(_destroyTimer);

			Destroy(gameObject);
		}

		public void LoadProgress(PlayerProgress progress)
		{
			LootPieceDataDictionary lootPiecesOnScene = progress.WorldData.LootData.LootPiecesOnScene;

			if (lootPiecesOnScene.Dictionary.ContainsKey(Id))
			{
				lootPiecesOnScene.Dictionary.Remove(Id);
			}
		}

		public void UpdateProgress(PlayerProgress progress)
		{
			if (Picked)
				return;

			LootPieceDataDictionary lootPiecesOnScene = progress.WorldData.LootData.LootPiecesOnScene;

			if (!lootPiecesOnScene.Dictionary.ContainsKey(Id))
				lootPiecesOnScene.Dictionary
					.Add(Id, new LootPieceData(transform.position.AsVectorData(), _loot));
		}
	}

	
}