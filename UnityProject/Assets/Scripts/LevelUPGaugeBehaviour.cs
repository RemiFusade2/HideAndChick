using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelUPGaugeBehaviour : MonoBehaviour 
{
	public RawImage gaugeRawImage;

	public Texture gaugeImage0;
	public Texture gaugeImage1;
	public Texture gaugeImage2;
	public Texture gaugeImage3;
	public Texture gaugeImage4;

	public Text lvlUpText; 
	public Text levelText; 

	private int currentLevel;
	private int currentXP;

	public MenuEngine menuEngine;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	IEnumerator WaitAndUpdateGaugeImage(float timer)
	{
		yield return new WaitForSeconds (timer);
		UpdateGaugeImage (false, false);
	}

	private void UpdateGaugeImage(bool levelUp, bool instant)
	{
		lvlUpText.gameObject.SetActive( (levelUp && !instant) );
		if (levelUp && !instant)
		{
			gaugeRawImage.texture = gaugeImage4;
			StartCoroutine(WaitAndUpdateGaugeImage(2.0f));
		}
		else
		{
			int imageIndex = ((currentXP % 1000) / 250);
			if (imageIndex == 0)
			{
				gaugeRawImage.texture = gaugeImage0;
			}
			else if (imageIndex == 1)
			{
				gaugeRawImage.texture = gaugeImage1;
			}
			else if (imageIndex == 2)
			{
				gaugeRawImage.texture = gaugeImage2;
			}
			else if (imageIndex == 3)
			{
				gaugeRawImage.texture = gaugeImage3;
			}
			string language = (PlayerPrefs.HasKey ("HideAndChickLanguageSetting") && PlayerPrefs.GetString ("HideAndChickLanguageSetting").Equals ("FR")) ? "FR" : "EN";
			levelText.text = (language.Equals("FR") ? "Niveau " : "Level ") + currentLevel;
		}
	}

	private void UpdateLevel(bool instant)
	{
		int lastLevel = currentLevel;
		currentLevel = 1 + (currentXP / 1000);

		UpdateGaugeImage (currentLevel > lastLevel, instant);
	}

	public void SetXP (int xpCount)
	{
		currentXP = xpCount;
		UpdateLevel (true);
	}

	public void AddXP (int xpCount)
	{
		currentXP += xpCount;
		UpdateLevel (false);
	}
}
