using UnityEngine;
using System.Collections;

public class SoundButtonBehaviour : MonoBehaviour {

	public bool soundActive;

	// Use this for initialization
	void Start () {
		Vector3 buttonPosition = Camera.main.ScreenToWorldPoint (new Vector3 (40, Screen.height-30, Camera.main.nearClipPlane+1));
		this.transform.position = buttonPosition;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0) && Input.mousePosition.x < 60 && Input.mousePosition.y > Screen.height-60)
		{
			soundActive = !soundActive;
			if (soundActive)
			{
				GameObject.Find("BackgroundMusic").audio.Play();
				this.renderer.material = Resources.Load("speakericon", typeof(Material)) as Material;
			}
			else
			{
				GameObject.Find("BackgroundMusic").audio.Stop();
				this.renderer.material = Resources.Load("speakericon_off", typeof(Material)) as Material;
			}
		}
	}
}
