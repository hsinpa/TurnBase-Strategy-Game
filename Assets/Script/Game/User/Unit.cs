using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Player;
using Utility;

[System.Serializable]
public class Unit : MonoBehaviour {
	private User parent;

	public bool cameraFollowSwitch = false;

	//General Unit Config
	public int hp {
		get {
			return mHP;
		}
		set {
			Debug.Log(value);
			if (value <= 0 ) {
				Dead();
			}
			mHP = value;
		}
	}
	private int mHP = 10;



	public int speed = 2;
	public int defense = 2;
	public int strength = 4;
	public int skill = 2;
	public int footSpeed = 5;

	public Vector2 unitPos { get { return new Vector2(transform.position.x, transform.position.y); } }
	//Status 0 = Not move, 1 = move;
	public enum Status { Idle, Rest};

	public Status status {
		get {
			return mStatus;
		}
		set {
			mStatus = value;
			SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>(); 
			switch (mStatus) {
				case Status.Idle :
				spriteRenderer.color= Color.white;

				break;

				case Status.Rest:
				spriteRenderer.color= Color.gray;

				break;
			}
		}
	}

	private Status mStatus;

	public Weapon currentWeapon { get { return weaponSets[0]; } }
	public List<Weapon> weaponSets = new List<Weapon>();

	//public GridHolder onGridHolder;
	void Start() {
		addTestWeapon();
	}

	public void Set(User p_user) {
		parent = p_user;
	}

	public void Move(Vector3[] _path, System.Action callback=null) {
		float time = _path.Length * 0.1f;
		transform.DOPath(_path, time, PathType.Linear, PathMode.TopDown2D).SetEase(Ease.Linear).OnComplete(delegate() {
			Debug.Log("On Land");
			if (callback != null) callback();
		});
	}

	public void Attack(Unit p_unit, GridHolder p_terrain) {
		AttackFormula formula = new AttackFormula(currentWeapon, p_terrain, this, p_unit);
		Debug.Log("Demaga " + formula.GetDamage() + " ,hitRate " +formula.accuracy);
		if (!UtilityMethod.PercentageGame(formula.accuracy)) {
			p_unit.hp = p_unit.hp - formula.GetDamage();
		}
	}

	public void Dead() {
		Debug.Log("IsDead");
		parent.allUnits.Remove(this);
		GameObject.Destroy(gameObject, 0.2f);
	}

	//Test function
	void addTestWeapon() {
		Archer weapon = new Archer();
		weapon.might = 4;
		weapon.name = "Test One";
		weapon.rangeSet = Weapon.RangeSet.Melee;
		weaponSets.Add(weapon);
	}

	void MoveWeapon(Weapon weapon, int pos) {
		weaponSets.Remove(weapon);
		weaponSets.Insert(pos, weapon);
	}


}
