using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialBehaviour : MonoBehaviour {

	public bool visible;

	public MenuEngine menuEngine;

	public List<GameObject> tutorialTextsFR;
	public List<GameObject> tutorialTextsEN;
	public GameObject tutorialTextsAZERTY;
	public GameObject tutorialTextsQWERTY;

	public void UpdateTextsLanguageAndControlsLayout()
	{
		foreach (GameObject text in tutorialTextsEN)
		{
			text.SetActive(menuEngine.getLanguage().Equals("EN"));
		}
		foreach (GameObject text in tutorialTextsFR)
		{
			text.SetActive(menuEngine.getLanguage().Equals("FR"));
		}
		tutorialTextsAZERTY.SetActive(menuEngine.getKeyboardLayout().Equals("AZERTY"));
		tutorialTextsQWERTY.SetActive(menuEngine.getKeyboardLayout().Equals("QWERTY"));
	}

	void OnMouseDown()
	{
		visible = !visible;
		UpdateTextsVisibility ();
	}

	void UpdateTextsVisibility()
	{
		foreach (Transform child in transform)
		{
			Color c = child.GetComponent<TextMesh>().color;
			Color c2 = new Color(c.r, c.g, c.b, visible?0.8f:0.2f);
			child.GetComponent<TextMesh>().color = c2;
		}
	}
}
