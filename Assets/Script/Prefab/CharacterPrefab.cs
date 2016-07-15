using UnityEngine;
using System.Collections;


[System.Serializable]
public class CharacterPrefab : ScriptableObject  {
	
	//General
	public Sprite _image;
	public string _name;
	public string _class;

	//Stats
	public int _strength;
	public int _defense;
	public int _speed;
	public int _skill;
	public int _footspeed;

}
