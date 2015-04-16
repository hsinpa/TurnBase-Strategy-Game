using UnityEngine;
using System.Collections;

public class cameraCtrl : MonoBehaviour {
	private float moveSensitivityX = 1.5f;
	private float moveSensitivityY = 1.5f;
	public bool updateZoomSensitivity = true;
	public float orthoZoomSpeed = 0.05f;
	private float minZoom = 3.0f;
	private float maxZoom = 5.0f;
	public bool invertMoveX = false;
	public bool invertMoveY = false;

	private Camera _camera;


	// Use this for initialization
	void Start () {
		_camera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		if (updateZoomSensitivity) {
			moveSensitivityX = _camera.orthographicSize/5f;
			moveSensitivityY = _camera.orthographicSize / 5f;
		}
		Touch[] touches = Input.touches;

		//Single touch
		if (touches.Length > 0 ) {
			if (touches[0].phase == TouchPhase.Moved) {
				Vector2 delta = touches[0].deltaPosition;
				float positionX = delta.x * moveSensitivityX * Time.deltaTime;
				positionX = invertMoveX ? positionX : positionX * -1;

				float positionY = delta.y * moveSensitivityY * Time.deltaTime;
				positionY = invertMoveY ? positionY : positionY * -1;

				_camera.transform.position += new Vector3(positionX, positionY, 0);
			}
		}
		//Zoom
		if (touches.Length == 2  ) {
			Touch touchOne = touches[0];
			Touch touchTwo = touches[1];

			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
			Vector2 touchTwoPrevPos = touchTwo.position - touchTwo.deltaPosition;
			float prevtouchDeltaMag = (touchOnePrevPos - touchTwoPrevPos).magnitude;
			float touchDeltaMag = (touchOne.position - touchTwo.position).magnitude;

			float deltaMagDiff = prevtouchDeltaMag - touchDeltaMag;
			_camera.orthographicSize += deltaMagDiff * orthoZoomSpeed;
			_camera.orthographicSize = Mathf.Clamp( _camera.orthographicSize, minZoom, maxZoom);
		}
	}
}
