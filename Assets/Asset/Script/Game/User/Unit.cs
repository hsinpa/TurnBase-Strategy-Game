using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Player;
using Utility;
using DG.Tweening;
using System.Threading;

[System.Serializable]
public class Unit : MonoBehaviour {
	private User parent;

	public bool cameraFollowSwitch = false;
	public string guid;
	//General Unit Config
	public int hp {
		get {
			return mHP;
		}
		set {
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

	public Vector2 unitPos {
					get { 
						return mUnitPos; 
					}
					set {
						transform.position = value;
						mUnitPos = value;	
					} }
	private Vector2 mUnitPos;

	//Status 0 = Not move, 1 = move;
	public enum Status { Idle, Moved};

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

				case Status.Moved:
				spriteRenderer.color= Color.gray;

				break;
			}
		}
	}

	private Status mStatus;

	public Weapon currentWeapon { get { return weaponSets[0]; } }
	public List<Weapon> weaponSets = new List<Weapon>();


	public void Set(User p_user, string p_class) {
		parent = p_user;
		addTestWeapon(p_class);
		guid = System.Guid.NewGuid().ToString();

		transform.name = guid;
	}


	public void Move(Vector3[] _path, System.Action callback=null) {
		float time = _path.Length * 0.1f;
		transform.DOPath(_path, time, PathType.Linear).SetEase(Ease.Flash).OnComplete(delegate() {
			if (callback != null) callback();
			unitPos = _path[ _path.Length - 1 ];
		});
	}

	public void Attack(Unit p_target, GridHolder p_terrain) {
		AttackFormula formula = new AttackFormula(currentWeapon, p_terrain, this, p_target);
		Debug.Log("Damaga " + formula.GetDamage() + " ,hitRate " +formula.accuracy + " Target " +p_target.name);
		if (!UtilityMethod.PercentageGame(formula.accuracy)) {
			p_target.hp = p_target.hp - formula.GetDamage();
		}
	}

	public void Dead() {
		Debug.Log("IsDead");
		parent.allUnits.Remove(this);
		GameObject.Destroy(gameObject, 0.2f);
	}

	//Test function
	void addTestWeapon( string p_class ) {
		Weapon weapon;
		switch (p_class) {
			case "knight":
				weapon = new Melee();
			break;

			case "archer":
				weapon = new Archer();

			break;

			case "warrior" :
				weapon = new Melee();

			break;

			case "troop":
				weapon = new Melee();
			break;

			default:
				weapon = new Melee();
			break;
		}
		weapon.might = 4;
		weapon.name = "Test One";
		weapon.rangeSet = Weapon.RangeSet.Melee;
		weaponSets.Add(weapon);
	}

	void MoveWeapon(Weapon weapon, int pos) {
		weaponSets.Remove(weapon);
		weaponSets.Insert(pos, weapon);
	}

	public void UnitHighLightControl(bool isOn) {
			Animator anim = transform.GetComponent<Animator>();
			anim.SetBool("Idle", isOn);
	}


}
