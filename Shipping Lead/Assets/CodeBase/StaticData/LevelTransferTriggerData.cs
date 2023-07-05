using System;
using UnityEngine;

namespace CodeBase.StaticData
{
	[Serializable]
	public class LevelTransferTriggerData
	{
		public Vector2 Position;
		public string Id;
		public string TransferTo;


		public LevelTransferTriggerData(string id, Vector2 position)
		{
			Id = id;
			Position = position;
		}
	}
}