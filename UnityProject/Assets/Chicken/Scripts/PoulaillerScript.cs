using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class PoulaillerScript : MonoBehaviour {

	int pouletsRecuperes = 0;
	public int pouletsTotal;

	public List<GameObject> objectsToHideAfterEnd;
	public List<GameObject> objectsToShowAfterEnd;

	public List<GameObject> objectsFRToShowAfterEnd;
	public List<GameObject> objectsENToShowAfterEnd;

	public Material skyboxToShowAfterEnd;

	public string language;

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
		DebugChicksCountChange ();
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
			pouletsRecuperes += count;
			this.transform.Find("Validation").audio.Play();
			UpdateRemainingChicksText();
			CheckForEnding();
		}
	}

	public void UpdateRemainingChicksText()
	{
		int pouletsRestants = pouletsTotal - pouletsRecuperes;
		GameObject remainingChicksText = GameObject.Find("Maison/RemainingChicksText");
		TextMeshPro textScript = remainingChicksText.GetComponent<TextMeshPro>();
		if (pouletsRestants <= 0)
		{
			if (language == "FR")
			{
				textScript.text = "Felicitations! Tu as trouve tous les poussins!";
			}
			else
			{
				textScript.text = "Congratulations! You found all the chicks!";
			}
		} 
		else if (pouletsRestants == 1)
		{
			if (language == "FR")
			{
				textScript.text = "Plus qu'un poussin a trouver!";
			}
			else
			{
				textScript.text = "One remaining chick!";
			}
		}
		else
		{
			if (language == "FR")
			{
				textScript.text = "Poussins restants: " + pouletsRestants;
			}
			else
			{
				textScript.text = "Remaining chicks: " + pouletsRestants;
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
			if (language == "FR")
			{
				foreach (GameObject obj in this.objectsFRToShowAfterEnd)
				{
					obj.SetActive(true);
				}
			}
			else
			{
				foreach (GameObject obj in this.objectsENToShowAfterEnd)
				{
					obj.SetActive(true);
				}
			}
		}
		RenderSettings.skybox = skyboxToShowAfterEnd;
		RenderSettings.fog = false;
	}
}
