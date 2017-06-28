using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanismContainer : MonoBehaviour {
	public CharacterManager characterManager { get { return transform.Find("character").GetComponent<CharacterManager>(); } }
	
}
