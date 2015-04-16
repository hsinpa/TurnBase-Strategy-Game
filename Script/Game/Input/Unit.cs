using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour {

	public int hp = 10;
	public int footSpeed = 3;
	public int power = 4;
	//Status 0 = Not move, 1 = move;
	public int status = 0;
	public Weapon currentWeapon;
	public List<Weapon> weaponSets = new List<Weapon>();

	void Start() {
		addTestWeapon();
	}

	//Test function
	void addTestWeapon() {
		Weapon weapon = new Weapon();
		weapon.attack = 4;
		weapon.cost = 30;
		weapon.name = "Test One";
		weapon.weaponType = Weapon.weaponSet.Melee;
		weaponSets.Add(weapon);
	}

	void moveWeapon(Weapon weapon, int pos) {
		weaponSets.Remove(weapon);
		weaponSets.Insert(pos, weapon);
	}
	
}
