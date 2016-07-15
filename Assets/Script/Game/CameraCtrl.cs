using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Lean;
using DG.Tweening;

public class CameraCtrl : MonoBehaviour {
	private float moveSensitivityX = 1.5f;
	private float moveSensitivityY = 1.5f;
	public bool updateZoomSensitivity = true;
	public float orthoZoomSpeed = 0.05f;
	private float minZoom = 3.0f;
	private float maxZoom = 4.0f;
	private float dragSpeed = 1.5f;
	public bool invertMoveX = false;
	public bool invertMoveY = false;


	private bool followSwitch = false;
	private Unit mFollowUnit;

	private Camera _camera;


	// Use this for initialization
	void Start () {
		_camera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		Touch[] touches = Input.touches;

		if ( followSwitch ) {
			_camera.transform.position = new Vector3( mFollowUnit.transform.position.x, mFollowUnit.transform.position.y, _camera.transform.position.z);

		} else if (Lean.LeanTouch.PinchScale != 1) {
			_camera.orthographicSize /= Lean.LeanTouch.PinchScale ;
			_camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, minZoom, maxZoom);
		} else if (LeanTouch.DragDelta != Vector2.zero){

			Vector3 dragDist = new Vector3(LeanTouch.DragDelta.x, LeanTouch.DragDelta.y, 0) * dragSpeed * Time.deltaTime;
			_camera.transform.position -= dragDist;

//			Vector3 newCamPos = new Vector3(  Mathf.Clamp( _camera.transform.position.x +_camera.rect.size.x, 0, Map.width),
//				 Mathf.Clamp( _camera.transform.position.y + _camera.rect.size.y, 0, Map.height), -10  );

		}
	}

	public void MoveToUnit(Unit p_unit) {
		_camera.transform.DOMove(p_unit.transform.position, 1).SetEase(Ease.Linear);
	}

	public void StartFollowing(Unit p_unit ) {
		mFollowUnit = p_unit;
		followSwitch = true;
	}

	public void StopFollowing() {
		followSwitch = false;

	}
}
