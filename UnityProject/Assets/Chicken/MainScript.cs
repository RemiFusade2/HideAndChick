using UnityEngine;
using System.Collections;

public class MainScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		for (int x = -300; x < 300; x+=50)
		{
			for (int z = -300; z < 300; z+=50)
			{
				Instantiate (Resources.Load("Poulet"), new Vector3(x,0,z), Quaternion.identity);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
}
