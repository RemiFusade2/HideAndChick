using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuCameraBehaviour : MonoBehaviour {
	
	public Transform TransitionCameraPosition;
	public Transform IntroMenuCameraPosition;
	public float IntroMenuCameraFov;

	public GameObject InGameCamera;

	public GameObject Player;

	private bool moveToInGameCamera;
	private bool moveToIntroMenuCamera;
	private float moveStep;
	private Vector3 startPosition;
	private Quaternion startOrientation;
	private float startFov;
	
	private Vector3 targetPosition;
	private Quaternion targetOrientation;
	private float targetFov;

	public float timerForCameraMove;

	// Use this for initialization
	void Start () 
	{
		moveStep = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( moveToInGameCamera || moveToIntroMenuCamera )
		{
			float lerpRatio = moveStep / timerForCameraMove;
			Vector3 position = Vector3.Lerp(startPosition, targetPosition, lerpRatio);
			Quaternion orientation = Quaternion.Lerp(startOrientation, targetOrientation, lerpRatio);
			this.transform.position = position;
			this.transform.rotation = orientation;
			this.GetComponent<Camera>().fieldOfView = startFov + (targetFov - startFov)*lerpRatio;
			if ( moveStep >= timerForCameraMove && targetPosition.Equals(TransitionCameraPosition.transform.position) )
			{
				startPosition = this.transform.position;
				startOrientation = this.transform.rotation;
				startFov = this.GetComponent<Camera> ().fieldOfView;
				if (moveToInGameCamera)
				{
					InGameCamera.GetComponent<CameraScript>().UpdatePosition();
					targetPosition = InGameCamera.transform.position;
					targetOrientation = InGameCamera.transform.rotation;
					targetFov = InGameCamera.GetComponent<Camera>().fieldOfView;
				}
				else if (moveToIntroMenuCamera)
				{
					targetPosition = IntroMenuCameraPosition.position;
					targetOrientation = IntroMenuCameraPosition.rotation;
					targetFov = IntroMenuCameraFov;
				}
				moveStep = 0;
			}
			else if ( moveStep >= timerForCameraMove && !targetPosition.Equals(TransitionCameraPosition.transform.position) )
			{
				if (moveToInGameCamera)
				{
					Player.GetComponent<ControlScript> ().ActivateControls ();
					moveToInGameCamera = false;
					this.gameObject.SetActive (false);
					this.GetComponent<AudioListener> ().enabled = false;
					//RenderSettings.fog = true;
					InGameCamera.SetActive (true);
					InGameCamera.GetComponent<AudioListener> ().enabled = true;
				}
				
				if (moveToIntroMenuCamera)
				{
					Player.GetComponent<ControlScript> ().DeactivateControls ();
					moveToIntroMenuCamera = false;
					//this.gameObject.SetActive (false);
					RenderSettings.fog = false;
				}
			}
			moveStep += Time.deltaTime;
		}
	}

	public void SwitchToInGameCamera()
	{
		Player.SetActive (true);
		startPosition = this.transform.position;
		startOrientation = this.transform.rotation;
		startFov = this.GetComponent<Camera> ().fieldOfView;
		moveStep = 0;
		moveToInGameCamera = true;	
		moveToIntroMenuCamera = false;	

		targetPosition = Vector3.Lerp(TransitionCameraPosition.position, InGameCamera.transform.position, 0);
		targetOrientation = Quaternion.Lerp(TransitionCameraPosition.rotation, InGameCamera.transform.rotation, 0);
		targetFov = 60.0f;
	}
	
	public void SwitchToIntroMenuCamera()
	{
		this.transform.localPosition = InGameCamera.transform.localPosition;
		this.transform.localRotation = InGameCamera.transform.localRotation;

		this.gameObject.SetActive (true);
		this.GetComponent<AudioListener> ().enabled = true;
		InGameCamera.SetActive (false);
		InGameCamera.GetComponent<AudioListener> ().enabled = false;

		startPosition = this.transform.position;
		startOrientation = this.transform.rotation;
		startFov = this.GetComponent<Camera> ().fieldOfView;
		moveStep = 0;
		moveToIntroMenuCamera = true;
		moveToInGameCamera = false;	
		//RenderSettings.fog = false;
		
		targetPosition = Vector3.Lerp(TransitionCameraPosition.position, InGameCamera.transform.position, 0);
		targetOrientation = Quaternion.Lerp(TransitionCameraPosition.rotation, InGameCamera.transform.rotation, 0);
		targetFov = 60.0f;
	}
}
