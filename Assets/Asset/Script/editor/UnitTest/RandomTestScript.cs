using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class RandomTestScript  {
	
	[Test]
	public void Test_Object_Name() {
		Map m_map = GetMapOject();
		Assert.AreEqual("map", m_map.name, "Map name wrong la");

	}


	private Map GetMapOject() {
		return GameObject.Find("map").GetComponent<Map>();
	}
}
