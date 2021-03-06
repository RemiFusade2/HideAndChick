﻿using UnityEngine;
using System.Collections;

public class SoundButtonBehaviour : MonoBehaviour {

	public bool soundActive;

	// Use this for initialization
	void Start () 
	{
		float z = this.transform.localPosition.z;
		float y = this.transform.localPosition.y;

		Vector3 leftPosition = this.transform.parent.GetComponent<Camera>().ScreenToWorldPoint (new Vector3 (0, Screen.height-30, z));

		this.transform.localPosition = new Vector3(leftPosition.x+0.1f, y, z);
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown()
	{
		Debug.Log ("on mouse down sound");
		soundActive = !soundActive;
		if (soundActive)
		{
			GameObject.Find("BackgroundMusic").GetComponent<AudioSource>().Play();
			this.GetComponent<Renderer>().material = Resources.Load("speakericon", typeof(Material)) as Material;
		}
		else
		{
			GameObject.Find("BackgroundMusic").GetComponent<AudioSource>().Stop();
			this.GetComponent<Renderer>().material = Resources.Load("speakericon_off", typeof(Material)) as Material;
		}
	}
}
