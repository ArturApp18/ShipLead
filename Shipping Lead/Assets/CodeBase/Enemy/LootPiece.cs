using System.Collections;
using CodeBase.Data;
using TMPro;
using UnityEngine;

namespace CodeBase.Enemy
{
	public class LootPiece : MonoBehaviour
	{
		public GameObject Sprite;
		public GameObject PickupFxPrefab;
		public TextMeshPro LootText;
		public GameObject PickUpPopup;
		
		private Loot _loot;
		private WorldData _worldData;

		private bool _picked;

		[SerializeField] private float _destroyTimer;

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
			if (_picked)
				return;

			_picked = true;

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
	}
}