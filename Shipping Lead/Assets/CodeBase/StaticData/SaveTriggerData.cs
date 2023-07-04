using System;
using UnityEngine;

namespace CodeBase.StaticData
{
	[Serializable]
	public class SaveTriggerData
	{
		public Vector2 Position;
		public string Id;

		public SaveTriggerData(string id, Vector2 position)
		{
			Id = id;
			Position = position;
		}
	}
}