using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon  {
	public enum weaponSet {Melee, Archer, Mage };
	public weaponSet weaponType;
	public int cost;
	public int attack;
	public string name;
	
	public WeaponType getAttackPattern() {
		switch(weaponType) {
		case weaponSet.Melee:
			return new Melee();
			break;
		case weaponSet.Archer:
			return new Archer();
			break;
		case weaponSet.Mage:
			return new Mage();
			break;
		default:
			return null;
		}

	}
}
