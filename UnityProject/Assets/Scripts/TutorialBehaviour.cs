using UnityEngine;
using System.Collections;
using TMPro;

public class TutorialBehaviour : MonoBehaviour {

	public bool visible;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown()
	{
		visible = !visible;
		UpdateTexts ();
	}

	void UpdateTexts()
	{
		foreach (Transform child in transform)
		{
			Color c = child.GetComponent<TextMeshPro>().color;
			Color c2 = new Color(c.r, c.g, c.b, visible?0.8f:0.2f);
			child.GetComponent<TextMeshPro>().color = c2;
		}
	}
}
