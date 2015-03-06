using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour {

	bool firstCol;

	// Use this for initialization
	void Start () {
		firstCol = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision col)
	{
		if (firstCol)
		{
			this.transform.Find ("AudioSource").audio.Play ();
		}
		firstCol = true;
	}
}
