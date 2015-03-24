using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class LoaderScript : MonoBehaviour {

	public List<GameObject> chickensList;

	public GameObject TriggerPoulailler;

	public GameObject player;

	public GameObject InfoText;

	public string language;
		
	private bool gameInit = false;

	private bool quitting = false;
	private float exitTimer = 0;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (!gameInit)
			{
				Application.Quit();				
			}
			if (gameInit && !quitting)
			{
				string message = (language=="FR")?"Sauvegarde en cours...\nA bientot!":"Saving...\nGood bye!";
				TextMeshPro textScript = InfoText.GetComponent<TextMeshPro>();
				textScript.text = message;
				textScript.renderer.castShadows = false;
				SaveGame();
				quitting = true;
				exitTimer = Time.time;
			}
		}
		if (quitting && Time.time > exitTimer+1)
		{
			Application.Quit();
		}
	}

	public bool HasGameData()
	{
		return PlayerPrefs.HasKey ("PouletsRecuperes") && PlayerPrefs.GetInt ("PouletsRecuperes") > 0;
	}

	public void InitGame(bool newGame)
	{
		if (!newGame)
		{
			LoadGame();
		}
		else
		{
			TriggerPoulailler.GetComponent<PoulaillerScript>().SetPouletsRecuperes(0);
		}
		gameInit = true;
	}

	private void SaveGame()
	{
		int index = 0;
		foreach (GameObject chicken in chickensList)
		{
			string key = "Poulet"+index;
			ChickenScript script = chicken.GetComponent<ChickenScript>();
			PlayerPrefs.SetString(key, script.HasBeenFound().ToString());
			index++;
		}
		PoulaillerScript poulaillerScript = TriggerPoulailler.GetComponent<PoulaillerScript>();
		PlayerPrefs.SetInt ("PouletsRecuperes", poulaillerScript.GetPouletsRecuperes ());
		PlayerPrefs.Save ();
	}
	
	private void LoadGame()
	{
		int index = 0;
		float x = 0;
		float y = 3;
		float z = -3;
		foreach (GameObject chicken in chickensList)
		{
			string key = "Poulet"+index;
			if (PlayerPrefs.HasKey(key) && PlayerPrefs.GetString(key) == "True")
			{
				ChickenScript script = chicken.GetComponent<ChickenScript>();
				Vector3 pos = new Vector3(x,y,z);
				script.SetFound(player, pos);
				y += 1.5f;
			}
			index++;
		}
		if (PlayerPrefs.HasKey("PouletsRecuperes"))
		{
			PoulaillerScript poulaillerScript = TriggerPoulailler.GetComponent<PoulaillerScript>();
			poulaillerScript.SetPouletsRecuperes (PlayerPrefs.GetInt ("PouletsRecuperes"));
		}
	}
}
