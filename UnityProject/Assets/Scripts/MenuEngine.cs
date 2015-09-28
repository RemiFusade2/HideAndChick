using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

[Serializable]
public class LanguageStringInformation
{
	public Text TextObject;
	public Text TextObjectShadow;
	public string FR;
	public string EN;
}

public class MenuEngine : MonoBehaviour 
{
	private string currentKeyboardLayout;
	private string currentLanguage;

	public string getKeyboardLayout()
	{
		return (currentKeyboardLayout.Equals ("AZERTY") ? "AZERTY" : "QWERTY");
	}
	public string getLanguage()
	{
		return (currentLanguage.Equals ("FR") ? "FR" : "EN");
	}

	private const string userPrefsLanguageKey = "HideAndChickLanguageSetting";
	private const string userPrefsKeyboardLayoutKey = "HideAndChickKeyboardLayoutSetting";

	public List<LanguageStringInformation> translatableTexts;
	
	public GameObject IntroMenuContainer;
	public GameObject InGameUIContainer;
	public GameObject InGamePauseContainer;
	public GameObject SavingGameContainer;
	public GameObject EndingMenuContainer;

	public GameObject MainMenuPanel;
	public GameObject NewGameButton;
	public GameObject ContinueButton;
	public GameObject SettingsMenuPanel;
	
	public GameObject Level2Message;
	private bool level2MessageShown;
	public GameObject Level4Message;
	private bool level4MessageShown;
	public GameObject Level6Message;
	private bool level6MessageShown;
	public GameObject Level9Message;
	private bool level9MessageShown;
	
	public GameObject SavingInProgress;
	public GameObject SavingDone;

	public Text keyboardText;
	public Text keyboardTextShadow;

	public ControlScript controlScript;

	public LoaderScript loaderScript;

	public Camera MenuCamera;

	public TutorialBehaviour tutorialBehaviourScript;

	public AudioSource backgroundMusicSource;
	public AudioSource levelUpMusicSource;
	public List<AudioSource> SFXSources;
	public Slider backgroundMusicVolumeSlider;
	public Slider sfxVolumeSlider;

	public ControlScript playerControls;

	// Use this for initialization
	void Start () 
	{
		currentLanguage = (PlayerPrefs.HasKey (userPrefsLanguageKey) && PlayerPrefs.GetString (userPrefsLanguageKey).Equals ("FR")) ? "FR" : "EN";
		currentKeyboardLayout = (PlayerPrefs.HasKey (userPrefsKeyboardLayoutKey) && PlayerPrefs.GetString (userPrefsKeyboardLayoutKey).Equals ("AZERTY")) ? "AZERTY" : "QWERTY";
		SetLanguage (currentLanguage );
		SetKeyboardLayout ( currentKeyboardLayout );
		ContinueButton.SetActive (loaderScript.HasGameData ());

		level2MessageShown = false;
		level4MessageShown = false;
		level6MessageShown = false;
		level9MessageShown = false;
		currentLevel = 1;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (IntroMenuContainer.activeSelf)
			{
				if (SettingsMenuPanel.activeSelf)
				{
					SettingsMenuPanel.SetActive(false);
					MainMenuPanel.SetActive(true);
				}
				else if (MainMenuPanel.activeSelf)
				{
					Application.Quit();
				}
			}
			else if (InGameUIContainer.activeSelf && playerControls.IsControlsActive())
			{
				InGameUIContainer.SetActive(false);
				InGamePauseContainer.SetActive(true);
				playerControls.DeactivateControls();
			}
			else if (InGamePauseContainer.activeSelf)
			{
				InGamePauseContainer.SetActive(false);
				InGameUIContainer.SetActive(true);
				playerControls.ActivateControls();
			}
			else if (EndingMenuContainer.activeSelf)
			{
				Application.Quit();
			}
		}
	}

	public void StartNewGame()
	{
		MainMenuPanel.SetActive (false);
		IntroMenuContainer.SetActive (false);
		InGameUIContainer.SetActive(true);
		loaderScript.InitGame (true);
		MenuCamera.GetComponent<MenuCameraBehaviour> ().SwitchToInGameCamera ();
	}
	
	public void ContinueLastGame()
	{
		MainMenuPanel.SetActive (false);
		IntroMenuContainer.SetActive (false);
		InGameUIContainer.SetActive(true);
		loaderScript.InitGame (false);
		MenuCamera.GetComponent<MenuCameraBehaviour> ().SwitchToInGameCamera ();
	}

	public void UnPauseGame()
	{
		InGamePauseContainer.SetActive(false);
		InGameUIContainer.SetActive(true);
		playerControls.ActivateControls();
	}
	
	public void SaveAndGoBackToMainMenu()
	{
		InGamePauseContainer.SetActive(false);

		SavingGameContainer.SetActive (true);
		SavingInProgress.SetActive (true);
		SavingDone.SetActive (false);

		loaderScript.SaveGame ();
		MenuCamera.GetComponent<MenuCameraBehaviour> ().SwitchToIntroMenuCamera ();
		ContinueButton.SetActive (true);
		NewGameButton.SetActive (false);

		StartCoroutine (WaitAndTellSavingIsDone (0.5f));
		StartCoroutine (WaitAndHideSavingPanel (1.5f));
		StartCoroutine (WaitAndDisplayMainMenu (2.0f));
	}
	
	public IEnumerator WaitAndTellSavingIsDone(float timer)
	{
		yield return new WaitForSeconds (timer);
		SavingGameContainer.SetActive (true);
		SavingInProgress.SetActive (false);
		SavingDone.SetActive (true);
	}

	public IEnumerator WaitAndHideSavingPanel(float timer)
	{
		yield return new WaitForSeconds (timer);
		SavingGameContainer.SetActive (false);
	}

	public IEnumerator WaitAndDisplayMainMenu(float timer)
	{
		yield return new WaitForSeconds (timer);
		MainMenuPanel.SetActive(true);
		IntroMenuContainer.SetActive(true);
	}

	public void OpenSettingsMenu()
	{
		MainMenuPanel.SetActive(false);
		SettingsMenuPanel.SetActive(true);
	}

	public void SetLanguage(string newLanguage)
	{
		currentLanguage = (newLanguage.Equals("FR")) ? "FR":"EN";
		PlayerPrefs.SetString (userPrefsLanguageKey, newLanguage);
		UpdateSettings ();
		tutorialBehaviourScript.UpdateTextsLanguageAndControlsLayout ();
	}
	public void SetKeyboardLayout(string newKeyboardLayout)
	{
		currentKeyboardLayout = (newKeyboardLayout.Equals("AZERTY")) ? "AZERTY":"QWERTY";
		PlayerPrefs.SetString (userPrefsKeyboardLayoutKey, newKeyboardLayout);
		UpdateSettings ();
		tutorialBehaviourScript.UpdateTextsLanguageAndControlsLayout ();
	}
	public void ChangeLanguage()
	{
		SetLanguage (PlayerPrefs.GetString (userPrefsLanguageKey).Equals ("EN") ? "FR" : "EN");
	}
	public void ChangeKeyboardLayout()
	{
		SetKeyboardLayout (PlayerPrefs.GetString (userPrefsKeyboardLayoutKey).Equals ("QWERTY") ? "AZERTY" : "QWERTY");
	}

	public void UpdateSettings()
	{
		foreach (LanguageStringInformation info in translatableTexts)
		{
			if (PlayerPrefs.GetString (userPrefsLanguageKey).Equals ("FR"))
			{
				if (info.TextObject != null)
					info.TextObject.text = info.FR;
				if (info.TextObjectShadow != null)
					info.TextObjectShadow.text = info.FR;
			} else {
				if (info.TextObject != null)
					info.TextObject.text = info.EN;
				if (info.TextObjectShadow != null)
					info.TextObjectShadow.text = info.EN;
			}
		}
		controlScript.SetKeyboardLayout (PlayerPrefs.GetString (userPrefsKeyboardLayoutKey));
		keyboardText.text = keyboardText.text + PlayerPrefs.GetString (userPrefsKeyboardLayoutKey);
		keyboardTextShadow.text = keyboardTextShadow.text + PlayerPrefs.GetString (userPrefsKeyboardLayoutKey);
	}

	public void BackgroundMusicVolumeChanged()
	{
		float sliderValue = backgroundMusicVolumeSlider.value;
		backgroundMusicSource.volume = sliderValue;
		levelUpMusicSource.volume = sliderValue;
	}
	public void SFXVolumeChanged()
	{
		foreach (AudioSource sfxSource in SFXSources)
		{
			float sliderValue = sfxVolumeSlider.value;
			sfxSource.volume = sliderValue;
		}
	}

	public void ShowEndingMenu()
	{
		InGameUIContainer.SetActive (false);
		InGamePauseContainer.SetActive (false);
		IntroMenuContainer.SetActive (false);
		SavingGameContainer.SetActive (false);
		EndingMenuContainer.SetActive (true);
	}

	// Level up
	private int currentLevel;

	public void HideLevelMessage()
	{
		Level2Message.SetActive(false);
		Level4Message.SetActive(false);
		Level6Message.SetActive(false);
		Level9Message.SetActive(false);
		ShowNextLevelMessage (currentLevel, true);
	}

	public void ShowNextLevelMessage(int level, bool display)
	{
		float timer = 1.0f;
		if (!level2MessageShown && level >= 2)
		{
			if (display) StartCoroutine(WaitAndShowNextLevelMessage(timer, Level2Message));
			level2MessageShown = true;
		}
		else if (!level4MessageShown && level >= 4)
		{
			if (display) StartCoroutine(WaitAndShowNextLevelMessage(timer, Level4Message));
			level4MessageShown = true;
		}
		else if (!level6MessageShown && level >= 6)
		{
			if (display) StartCoroutine(WaitAndShowNextLevelMessage(timer, Level6Message));
			level6MessageShown = true;
		}
		else if (!level9MessageShown && level >= 9)
		{
			if (display) StartCoroutine(WaitAndShowNextLevelMessage(timer, Level9Message));
			level9MessageShown = true;
		}
		currentLevel = level;
	}

	IEnumerator WaitAndShowNextLevelMessage(float timer, GameObject messageToShow)
	{
		yield return new WaitForSeconds (timer);
		messageToShow.SetActive (true);
	}
}
