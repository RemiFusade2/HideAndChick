using UnityEngine;
using System.Collections;
using TMPro;

public class PoulaillerScript : MonoBehaviour {

	int pouletsRecuperes;
	int pouletsTotal;

	// Use this for initialization
	void Start () {
		pouletsRecuperes = 0;
		pouletsTotal = 20;
		UpdateRemainingChicksText();
	}
	
	// Update is called once per frame
	void Update () {		
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
		}
	}

	void UpdateRemainingChicksText()
	{
		int pouletsRestants = pouletsTotal - pouletsRecuperes;
		GameObject remainingChicksText = GameObject.Find("Maison/RemainingChicksText");
		TextMeshPro textScript = remainingChicksText.GetComponent<TextMeshPro>();
		if (pouletsRestants <= 0)
		{
			textScript.text = "Congratulations! You found all the chicks!";
		} 
		else if (pouletsRestants == 1)
		{
			textScript.text = "One remaining chick!";
		}
		else
		{
			textScript.text = "Remaining chicks: " + pouletsRestants;
		}
	}
}
