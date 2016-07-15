using UnityEngine;
using System.Collections;

public class AttackFormula {
	Weapon mWeapon;
	Unit mSelf, mTarget;
	GridHolder mTerrain;

	public AttackFormula (Weapon _weapon, GridHolder _terrain, Unit _self, Unit _target) {
		mWeapon = _weapon;
		mTerrain = _terrain;
		mSelf = _self;
		mTarget = _target;
	}

	//Damage
	public float attack { get { return mSelf.strength + mWeapon.might; }	}
	public float defense  { get { return mTarget.defense + mTerrain.tile.defenseBonus; }	}

	//Hit Rage
	public float evade  { get { return mTarget.speed;  }	}
	public float hitRate  { get { return mWeapon.accuracy + (mSelf.skill * 2.5f);  }	}
	public float accuracy  { get { return (hitRate - evade) / 100; } }

	//Crit
	public float critRate  { get { return mWeapon.crit + (mSelf.skill /2 ); } }

	public int GetDamage() {
		int damage = (int)(attack - defense);
		return Mathf.Clamp(damage, 0, damage);
	}
}
