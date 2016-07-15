using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : IAttakeMode {
	public enum RangeSet {Melee, Archer, Mage };
	public RangeSet rangeSet;

	public float might, weight, accuracy, crit;
	public string name;

	public Sprite image;

	public virtual List<Vector2> GetAttackPoint (Vector2 unit)	{
		throw new System.NotImplementedException ();
	}
}
