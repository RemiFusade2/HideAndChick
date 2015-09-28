using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LoaderScript : MonoBehaviour {

	public List<GameObject> chickensList;

	public GameObject TriggerPoulailler;

	public GameObject player;
	public CameraScript playerCamera;
		
	private bool gameInit = false;

	public bool IsGameInit()
	{
		return gameInit;
	}

	private const string userPrefsChicksFound = "HideAndChickChicksFoundCount";
	private const string userPrefsChicksData = "HideAndChickChicksData";
	
	private const string userPrefsPlayerPositionX = "HideAndChickPlayerPositionX";
	private const string userPrefsPlayerPositionY = "HideAndChickPlayerPositionY";
	private const string userPrefsPlayerPositionZ = "HideAndChickPlayerPositionZ";
	
	private const string userPrefsPlayerXP = "HideAndChickPlayerXP";

	// Use this for initialization
	void Start () 
	{
		// PlayerPrefs.DeleteAll ();
	}

	public bool HasGameData()
	{
		return PlayerPrefs.HasKey (userPrefsPlayerPositionX);
	}

	public void InitGame(bool newGame)
	{
		if (!newGame && HasGameData())
		{
			miniMapScript.ResetWarFog();
			LoadGame();
		}
		else
		{
			TriggerPoulailler.GetComponent<PoulaillerScript>().SetPouletsRecuperes(0);
			player.transform.localPosition = new Vector3(0,2.1f,0);
			playerCamera.UpdatePosition();
			miniMapScript.ResetWarFog();
		}
		gameInit = true;
	}

	public void SaveGame()
	{
		// Save Player XP
		PlayerPrefs.SetInt (userPrefsPlayerXP, player.GetComponent<ControlScript>().GetXPGained());

		// Save Player position
		PlayerPrefs.SetFloat (userPrefsPlayerPositionX, player.transform.localPosition.x);
		PlayerPrefs.SetFloat (userPrefsPlayerPositionY, player.transform.localPosition.y);
		PlayerPrefs.SetFloat (userPrefsPlayerPositionZ, player.transform.localPosition.z);

		// Save Chicks founds
		int index = 0;
		foreach (GameObject chicken in chickensList)
		{
			string key = userPrefsChicksData+index;
			ChickenScript script = chicken.GetComponent<ChickenScript>();
			PlayerPrefs.SetString(key, script.HasBeenFound().ToString());
			index++;
		}
		PoulaillerScript poulaillerScript = TriggerPoulailler.GetComponent<PoulaillerScript>();
		PlayerPrefs.SetInt (userPrefsChicksFound, poulaillerScript.GetPouletsRecuperes ());

		// Save Mini map
		SaveTexture(miniMapScript.getFogTexture(), miniMapTextureFileName);

		PlayerPrefs.Save ();
	}
	
	private void LoadGame()
	{
		// Load Player XP
		player.GetComponent<ControlScript> ().SetXP (PlayerPrefs.GetInt (userPrefsPlayerXP));

		// Load Player position
		Vector3 newPlayerPosition = new Vector3 (PlayerPrefs.GetFloat (userPrefsPlayerPositionX), 
		                                         PlayerPrefs.GetFloat (userPrefsPlayerPositionY),
		                                         PlayerPrefs.GetFloat (userPrefsPlayerPositionZ));
		player.transform.localPosition = newPlayerPosition;

		// Load chicks found
		int index = 0;
		float x = 0;
		float y = 3;
		float z = -3;
		float deltaY = 3;
		foreach (GameObject chicken in chickensList)
		{
			string key = userPrefsChicksData+index;
			if (PlayerPrefs.HasKey(key) && PlayerPrefs.GetString(key) == "True")
			{
				ChickenScript script = chicken.GetComponent<ChickenScript>();
				Vector3 pos = new Vector3(x,y,z);
				script.SetFound(player, pos);
				y += deltaY;
			}
			index++;
		}
		if (PlayerPrefs.HasKey(userPrefsChicksFound))
		{
			PoulaillerScript poulaillerScript = TriggerPoulailler.GetComponent<PoulaillerScript>();
			poulaillerScript.SetPouletsRecuperes (PlayerPrefs.GetInt (userPrefsChicksFound));
		}

		// Load Mini map
		Texture2D miniMapTexture = RetrieveTexture (miniMapTextureFileName);
		miniMapScript.setFogTexture (miniMapTexture);
	}

	// Minimap texture
	public MiniMapBehaviour miniMapScript;
	WWW www;
	private const string miniMapTextureFileName = "HideAndChickMiniMapFog";
		
	private void SaveTexture(Texture2D textureToSave, string saveAs)
	{
		Texture2D tex;
		byte[] byteArray;

		tex = textureToSave;
		byteArray = tex.EncodeToPNG();
		
		string temp = Convert.ToBase64String(byteArray);
		
		PlayerPrefs.SetString(saveAs,temp);      /// save it to file if u want.
		PlayerPrefs.SetInt(saveAs+"_w",tex.width);
		PlayerPrefs.SetInt(saveAs+"_h",tex.height);
	}
	
	private Texture2D RetrieveTexture(string savedImageName)
	{
		string temp=PlayerPrefs.GetString(savedImageName);
		
		int width=PlayerPrefs.GetInt(savedImageName+"_w");
		int height=PlayerPrefs.GetInt(savedImageName+"_h");
		
		byte[] byteArray= Convert.FromBase64String(temp);
		
		Texture2D tex = new Texture2D(width,height);
		
		tex.LoadImage(byteArray);
		return tex;		
	}
}
