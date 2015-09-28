using UnityEngine;
using System.Collections;

public class StartGameButtonsBehaviour : MonoBehaviour {

	//static bool waitForInput = true;

	public bool newGame;

	public GameObject MenuCamera;

	public GameObject Loader;

	// Use this for initialization
	void Start () 
	{
		if (!newGame && !Loader.GetComponent<LoaderScript>().HasGameData())
		{
			this.gameObject.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	/*
	void OnMouseDown()
	{
		if (waitForInput)
		{
			// start game
			this.GetComponent<TextMeshPro>().color = new Color32(255,255,0,255);

			MenuCamera.GetComponent<Animator>().SetTrigger("StartGame");

			LoaderScript script = Loader.GetComponent<LoaderScript>();
			script.InitGame(newGame);

			waitForInput = false;
		}
	}
	 */
}
