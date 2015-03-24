using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuCameraBehaviour : MonoBehaviour {

	public GameObject InGameCamera;

	public List<GameObject> ObjectToHideAfterStartingAnim;

	public GameObject Player;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void SwitchToInGameCamera()
	{
		InGameCamera.SetActive (true);
		Player.SetActive (true);
		this.gameObject.SetActive (false);
		RenderSettings.fog = true;
		foreach (GameObject obj in ObjectToHideAfterStartingAnim)
		{
			obj.SetActive(false);
		}
		Player.GetComponent<ControlScript> ().ActivateControls ();
	}
}
