using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PoulaillerScript : MonoBehaviour {

	int pouletsRecuperes = 0;
	public int pouletsTotal;

	public List<GameObject> objectsToHideAfterEnd;
	public List<GameObject> objectsToShowAfterEnd;

	public Material skyboxToShowAfterEnd;

	public MenuEngine menuEngine;

	public bool debug;

	public ControlScript player;

	public int GetPouletsRecuperes()
	{
		return pouletsRecuperes;
	}
	public void SetPouletsRecuperes(int poulets)
	{
		pouletsRecuperes = poulets;
		UpdateRemainingChicksText();
	}

	// Use this for initialization
	void Start () 
	{
		//UpdateRemainingChicksText();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (debug)
		{
			DebugChicksCountChange ();
		}
	}

	void DebugChicksCountChange()
	{		
		if (Input.GetKeyDown(KeyCode.KeypadPlus))
		{
			pouletsRecuperes++;
			UpdateRemainingChicksText();
		}		
		if (Input.GetKeyDown(KeyCode.KeypadMinus))
		{
			pouletsRecuperes--;
			UpdateRemainingChicksText();
		}
	}

	public int xpForOneChick;

	void OnTriggerEnter(Collider col)
	{
		int count = 0;
		if (col.gameObject.tag == "Poulet")
		{
			ChickenScript chickenScript = col.GetComponent<ChickenScript>();
			if (chickenScript != null)
			{
				count += chickenScript.RemoveSpringJoint();
			}
		}
		if (col.gameObject.tag == "Player")
		{
			ControlScript playerScript = col.GetComponent<ControlScript>();
			if (playerScript != null)
			{
				count += playerScript.RemoveAllChicken();
			}
		}
		if (count != 0)
		{
			player.AddXP(count*xpForOneChick);
			pouletsRecuperes += count;
			this.transform.Find("Validation").GetComponent<AudioSource>().Play();
			UpdateRemainingChicksText();
			CheckForEnding();
		}
	}

	public TextMesh remainingChicksText;

	public void UpdateRemainingChicksText()
	{
		int pouletsRestants = pouletsTotal - pouletsRecuperes;
		if (pouletsRestants <= 0)
		{
			if (menuEngine.getLanguage().Equals("FR"))
			{
				remainingChicksText.text = "Felicitations!\nTu as trouvé tous les poussins!";
			}
			else
			{
				remainingChicksText.text = "Congratulations!\nYou found all the chicks!";
			}
		} 
		else if (pouletsRestants == 1)
		{
			if (menuEngine.getLanguage().Equals("FR"))
			{
				remainingChicksText.text = "Plus qu'un poussin à trouver!";
			}
			else
			{
				remainingChicksText.text = "One remaining chick!";
			}
		}
		else
		{
			if (menuEngine.getLanguage().Equals("FR"))
			{
				remainingChicksText.text = "Poussins restants: " + pouletsRestants;
			}
			else
			{
				remainingChicksText.text = "Remaining chicks: " + pouletsRestants;
			}
		}
	}

	void CheckForEnding()
	{
		if (pouletsRecuperes >= pouletsTotal)
		{
			foreach (GameObject obj in this.objectsToHideAfterEnd)
			{
				obj.SetActive(false);
			}
			foreach (GameObject obj in this.objectsToShowAfterEnd)
			{
				obj.SetActive(true);
			}
			RenderSettings.skybox = skyboxToShowAfterEnd;
			RenderSettings.fog = false;
			menuEngine.ShowEndingMenu();
		}
	}
}
