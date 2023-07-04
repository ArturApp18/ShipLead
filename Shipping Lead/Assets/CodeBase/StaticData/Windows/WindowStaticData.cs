using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.StaticData.Windows
{
	[CreateAssetMenu(fileName = "StaticData/Window static data", menuName = "WindowStaticData")]
	public class WindowStaticData : ScriptableObject
	{
		public List<WindowConfig> Config;
	}
}