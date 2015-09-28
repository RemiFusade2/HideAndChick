using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControlScript : MonoBehaviour {

	public float speed;
	public float jumpHeight;
	
	public float speedLvl1;
	public float speedLvl10;
	public float jumpHeightLvl1;
	public float jumpHeightLvl10;

	private List<GameObject> attachedChickens;

	public Transform corps;

	private float jumpTimer;

	private float lastMovementTimer;

	private bool controlActive = false;

	public List<GameObject> chicksList;

	public float callingRadius;
	
	public float callingRadiusLvl1;
	public float callingRadiusLvl10;
	
	public float callingStrength;
	
	public float callingStrengthLvl1;
	public float callingStrengthLvl10;
	
	public bool callingSkill;

	public bool doubleJump;
	public bool tripleJump;
	public bool infiniteJumps;
	public int numberOfJumpsWithoutTouchingGround;

	private string keyboardLayout;

	public void SetKeyboardLayout(string newKeyboardLayout)
	{
		if ( newKeyboardLayout.Equals("AZERTY") )
		{
			keyboardLayout = newKeyboardLayout;
		} else {
			keyboardLayout = "QWERTY";
		}
	}

	public GameObject xpContainer;
	public GameObject XPGainedTextPrefab;
	public GameObject LevelUPTextPrefab;

	private int level;
	private int xpGained;

	public LevelUPGaugeBehaviour levelUpGauge;
	public MenuEngine menuEngine;
	
	public AudioSource levelUpMusic;
	public AudioSource backgroundMusic;

	public int GetXPGained()
	{
		return xpGained;
	}

	public bool IsControlsActive ()
	{
		return controlActive;
	}
	public void DeactivateControls ()
	{
		controlActive = false;
	}
	public void ActivateControls ()
	{
		controlActive = true;
	}

	public void AddChicken(GameObject chicken)
	{
		attachedChickens.Add (chicken);
	}

	public int RemoveAllChicken()
	{
		int count = 0;
		foreach (GameObject chicken in attachedChickens)
		{
			ChickenScript chickenScript = chicken.GetComponent<ChickenScript>();
			if (chickenScript != null)
			{
				count += chickenScript.RemoveSpringJoint();
			}
		}
		return count;
	}

	// Use this for initialization
	void Start () 
	{
		SetXP(1);
		attachedChickens = new List<GameObject> ();
		jumpTimer = 0;
		lastGetXPTimer = Time.time;
		xpCountBuffer = 0;

		doubleJump = false;
		tripleJump = false;
		infiniteJumps = false;
		numberOfJumpsWithoutTouchingGround = 0;
	}

	/*
	void UpdateText()
	{
		if (Input.GetKey(KeyCode.Keypad0))
		{
			GameObject text = GameObject.Find("InfoText");
			TextMeshPro textScript = text.GetComponent<TextMeshPro>();
			textScript.text = "";
			textScript.GetComponent<Renderer>().castShadows = false;
		}
		if (Input.GetKey(KeyCode.Keypad1))
		{
			GameObject text = GameObject.Find("InfoText");
			TextMeshPro textScript = text.GetComponent<TextMeshPro>();
			textScript.text = "Open the door";
			textScript.GetComponent<Renderer>().castShadows = false;
		}
		if (Input.GetKey(KeyCode.Keypad2))
		{
			GameObject text = GameObject.Find("InfoText");
			TextMeshPro textScript = text.GetComponent<TextMeshPro>();
			textScript.text = "Play as a chicken";
			textScript.GetComponent<Renderer>().castShadows = false;
		}
		if (Input.GetKey(KeyCode.Keypad3))
		{
			GameObject text = GameObject.Find("InfoText");
			TextMeshPro textScript = text.GetComponent<TextMeshPro>();
			textScript.text = "Play as a straight white male chicken";
			textScript.GetComponent<Renderer>().castShadows = false;
		}
		if (Input.GetKey(KeyCode.Keypad4))
		{
			GameObject text = GameObject.Find("InfoText");
			TextMeshPro textScript = text.GetComponent<TextMeshPro>();
			textScript.text = "Find the chicks";
			textScript.GetComponent<Renderer>().castShadows = false;
		}
		if (Input.GetKey(KeyCode.Keypad5))
		{
			GameObject text = GameObject.Find("InfoText");
			TextMeshPro textScript = text.GetComponent<TextMeshPro>();
			textScript.text = "Find ALL the chicks!";
			textScript.GetComponent<Renderer>().castShadows = false;
		}
		if (Input.GetKey(KeyCode.Keypad6))
		{
			GameObject text = GameObject.Find("InfoText");
			TextMeshPro textScript = text.GetComponent<TextMeshPro>();
			textScript.text = "Pick up the chicks!";
			textScript.GetComponent<Renderer>().castShadows = false;
		}
		if (Input.GetKey(KeyCode.Keypad7))
		{
			GameObject text = GameObject.Find("InfoText");
			TextMeshPro textScript = text.GetComponent<TextMeshPro>();
			textScript.text = "Bring them home";
			textScript.GetComponent<Renderer>().castShadows = false;
		}
		if (Input.GetKey(KeyCode.Keypad8))
		{
			GameObject text = GameObject.Find("InfoText");
			TextMeshPro textScript = text.GetComponent<TextMeshPro>();
			textScript.text = "Find secret paths";
			textScript.GetComponent<Renderer>().castShadows = false;
		}
		if (Input.GetKey(KeyCode.Keypad9))
		{
			GameObject text = GameObject.Find("InfoText");
			TextMeshPro textScript = text.GetComponent<TextMeshPro>();
			textScript.text = "Walk on lava!";
			textScript.GetComponent<Renderer>().castShadows = false;
		}
	}
	*/

	// Update is called once per frame
	void Update () 
	{
		// Update XP Display
		UpdateXPCounter ();

		if (controlActive)
		{
			// Texts for trailer
			//UpdateText ();
			
			// Get input for movement
			Vector3 wantedMovementDirection = Vector3.zero;
			if (Input.GetKey(KeyCode.UpArrow) || 
			    (keyboardLayout.Equals("AZERTY") && Input.GetKey(KeyCode.Z) ) || 
			    (keyboardLayout.Equals("QWERTY") && Input.GetKey(KeyCode.W) ) )
			{
				wantedMovementDirection += Vector3.forward;
			}
			if (Input.GetKey(KeyCode.DownArrow) || 
			    (keyboardLayout.Equals("AZERTY") && Input.GetKey(KeyCode.S) ) || 
			 	(keyboardLayout.Equals("QWERTY") && Input.GetKey(KeyCode.S) ) )
			{
				wantedMovementDirection += Vector3.back;
			}
			if (Input.GetKey(KeyCode.RightArrow) || 
			    (keyboardLayout.Equals("AZERTY") && Input.GetKey(KeyCode.D) ) || 
			    (keyboardLayout.Equals("QWERTY") && Input.GetKey(KeyCode.D) ) )
			{
				wantedMovementDirection += Vector3.right;
			}
			if (Input.GetKey(KeyCode.LeftArrow) || 
			    (keyboardLayout.Equals("AZERTY") && Input.GetKey(KeyCode.Q) ) || 
			    (keyboardLayout.Equals("QWERTY") && Input.GetKey(KeyCode.A) ) )
			{
				wantedMovementDirection += Vector3.left;
			}
			Vector3 actualMovementDirection = Vector3.zero;
			
			if (Input.GetKeyDown(KeyCode.E) && callingSkill)
			{
				this.transform.Find("TwitterAudio").GetComponent<AudioSource>().Stop();
				this.transform.Find("TwitterAudio").GetComponent<AudioSource>().Play();
				this.GetComponent<Animator>().SetBool("twiterring", true);
				CallChicks();
			}
			else
			{
				this.GetComponent<Animator>().SetBool("twiterring", false);
			}
			
			RaycastHit resultingHit;
			if (wantedMovementDirection.magnitude > 0)
			{
				// Player wants to move
				wantedMovementDirection.Normalize ();
				actualMovementDirection = wantedMovementDirection;
				// Check terrain (wall, slope, etc)
				Ray rayToWantedDirection = new Ray (corps.transform.position, wantedMovementDirection);
				float rayLength = 2;
				if (Physics.Raycast(rayToWantedDirection, out resultingHit, rayLength))
				{
					// Here we deal with ray result
					if (resultingHit.collider.gameObject.tag == "Ground")
					{
						// The ray hit the ground
						float slopeAngle = Mathf.Deg2Rad * Vector3.Angle(Vector3.up, resultingHit.normal);
						//float slopeRayHeight = 0.5f;
						//float radius = Mathf.Abs(slopeRayHeight / Mathf.Sin (slopeAngle));
						float steepSlopeAngle = 45;
						if (slopeAngle >= steepSlopeAngle * Mathf.Deg2Rad)
						{
							actualMovementDirection = Vector3.zero;
						}
					}
				}
			}

			// Jump ?
			Vector3 jump = Vector3.zero;
			Vector3 slide = Vector3.zero;
			// Check if the player is on the ground
			Ray bottomRay = new Ray (corps.transform.position, Vector3.down);
			float distanceForJump = 0.8f;
			float distanceForSlide = 2;
			float maxDistance = 200.0f;

			if (Physics.Raycast(bottomRay, out resultingHit, maxDistance) && resultingHit.collider.gameObject.tag != "Poulet")
			{
				// Player is on something else than a chicken
				float slopeAngle = Vector3.Angle(Vector3.up, resultingHit.normal);
				if (Time.time > jumpTimer + 0.3f && resultingHit.distance < distanceForJump)
				{
					this.GetComponent<Animator>().SetBool("jumping", false);
					this.transform.Find("JumpAudio").GetComponent<AudioSource>().Stop();
					if ( resultingHit.distance < distanceForJump && slopeAngle < 50 )
					{
						numberOfJumpsWithoutTouchingGround = 0;
					}
				}

				if (Input.GetKeyDown(KeyCode.Space) && Time.time > jumpTimer + 0.3f)
				{
					bool jumping = false;
					if ( resultingHit.distance < distanceForJump && slopeAngle < 50 )
					{
						// jump from ground
						numberOfJumpsWithoutTouchingGround = 0;
						jumping = true;
					}
					else if ( doubleJump && numberOfJumpsWithoutTouchingGround == 0 )
					{
						// double Jump
						numberOfJumpsWithoutTouchingGround = 1;
						jumping = true;
					}
					else if ( tripleJump && numberOfJumpsWithoutTouchingGround == 1 )
					{
						// triple Jump
						numberOfJumpsWithoutTouchingGround = 2;
						jumping = true;
					}
					else if ( infiniteJumps )
					{
						// lots of Jumps
						numberOfJumpsWithoutTouchingGround++;
						jumping = true;
					}
					if ( jumping )
					{
						jump = Vector3.up;
						this.transform.Find("JumpAudio").GetComponent<AudioSource>().Play();
						this.GetComponent<Animator>().SetBool("jumping", true);
						jumpTimer = Time.time;
						lastMovementTimer = Time.time;
					}
				}

				// He will also slide
				if (slopeAngle > 30 && resultingHit.collider.gameObject.tag == "Ground" && resultingHit.distance < distanceForSlide)
				{
					float ratio =  (slopeAngle-30)/45;
					slide = new Vector3(resultingHit.normal.x * ratio, 0, resultingHit.normal.z * ratio);
				}
			}
			
			// Slide, move and jump if needed
			Vector3 newVelocity = Vector3.zero;
			if (jump.y.Equals(1))
			{
				newVelocity = new Vector3 (actualMovementDirection.x * speed + slide.x, jump.y * jumpHeight, actualMovementDirection.z * speed + slide.z);
			}
			else
			{
				newVelocity = new Vector3 (actualMovementDirection.x * speed + slide.x, GetComponent<Rigidbody>().velocity.y, actualMovementDirection.z * speed + slide.z);
			}
			GetComponent<Rigidbody>().velocity = newVelocity;
			if (wantedMovementDirection.magnitude > 0)
			{
				// Rotate to face wanted movement direction
				Vector3 directionLook = new Vector3(wantedMovementDirection.x, 0, wantedMovementDirection.z);
				corps.LookAt(corps.position + directionLook.normalized);
				lastMovementTimer = Time.time;
			}
			this.GetComponent<Animator>().SetBool("walking", (wantedMovementDirection.magnitude > 0));
			
			// Idle animation
			this.GetComponent<Animator>().SetFloat("waiting", Time.time - lastMovementTimer);
			if (Time.time - lastMovementTimer > 5)
			{
				lastMovementTimer = Time.time;
			}
		}
	}

	public void CallChicks()
	{
		float sqrCallingRadius = callingRadius * callingRadius;
		foreach (GameObject chick in chicksList)
		{
			if ( (chick.transform.position - this.transform.position).sqrMagnitude <= sqrCallingRadius )
			{
				chick.transform.LookAt (this.transform.position);
				chick.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
				Vector3 newVelocity = callingStrength*(this.transform.position - chick.transform.position).normalized;
				newVelocity = new Vector3 (newVelocity.x, 0, newVelocity.z);
				chick.GetComponent<Rigidbody>().velocity = newVelocity;
			}
		}
	}

	private float lastGetXPTimer;
	private int xpCountBuffer;

	private void UpdateXPCounter()
	{
		if (xpCountBuffer > 0 && Time.time - lastGetXPTimer > 0.5f)
		{
			GameObject xpgainedText = (GameObject) Instantiate (XPGainedTextPrefab, Vector3.zero, Quaternion.identity);
			xpgainedText.GetComponent<TextMesh> ().text = "+" + xpCountBuffer + " XP";
			xpgainedText.GetComponent<Animator> ().SetTrigger ("XPGained");
			xpgainedText.transform.parent = xpContainer.transform;
			xpgainedText.transform.localRotation = Quaternion.identity;
			StartCoroutine(WaitAndDestroyGameObject(3.0f, xpgainedText));
			xpGained += xpCountBuffer;
			levelUpGauge.AddXP (xpCountBuffer);
			xpCountBuffer = 0;
			UpdateLevel (true);
		}
	}

	public void AddXP(int xpCount)
	{
		xpCountBuffer += xpCount;
		lastGetXPTimer = Time.time;
	}

	private IEnumerator WaitAndDestroyGameObject(float timer, GameObject go)
	{
		yield return new WaitForSeconds(timer);
		Destroy (go);
	}

	public void SetXP(int xpTotal)
	{
		level = 1;
		xpGained = xpTotal;
		UpdateLevel (false);
		levelUpGauge.SetXP (xpTotal);
	}

	private void UpdateLevel(bool display)
	{
		int newLevel = Mathf.CeilToInt (xpGained / 1000.0f);
		if (newLevel > level)
		{
			float timer = 0.5f;
			while (newLevel > level)
			{
				level++;
				if (display)
				{
					StartCoroutine(WaitAndSendLevelUpText(timer));
					timer += 1.0f;
				}
			}

			speed = speedLvl1 + (speedLvl10-speedLvl1) * ((level-1)/10.0f);
			jumpHeight = jumpHeightLvl1 + (jumpHeightLvl10-jumpHeightLvl1) * ((level-1)/10.0f);
			callingRadius = callingRadiusLvl1 + (callingRadiusLvl10-callingRadiusLvl1) * ((level-1)/10.0f);

			if (level >= 2)
			{
				callingSkill = true;
			}
			if (level >= 4)
			{
				doubleJump = true;
			}
			if (level >= 6)
			{
				tripleJump = true;
			}
			if (level >= 9)
			{
				infiniteJumps = true;
			}
			menuEngine.ShowNextLevelMessage (level, display);
		}
	}

	private Coroutine replayBackgroundMusic;

	private IEnumerator WaitAndSendLevelUpText(float timer)
	{
		yield return new WaitForSeconds(timer);
		GameObject levelUpText = (GameObject) Instantiate (LevelUPTextPrefab, Vector3.zero, Quaternion.identity);
		levelUpText.GetComponent<Animator> ().SetTrigger ("XPGained");
		levelUpText.transform.parent = xpContainer.transform;
		levelUpText.transform.localRotation = Quaternion.identity;
		levelUpMusic.Stop ();
		backgroundMusic.Stop ();
		if (replayBackgroundMusic != null)
		{
			StopCoroutine(replayBackgroundMusic);
		}
		replayBackgroundMusic = StartCoroutine(WaitAndPlayBackgroundMusicAgain(4.5f));
		levelUpMusic.Play ();
		StartCoroutine(WaitAndDestroyGameObject(3.0f, levelUpText));
	}

	private IEnumerator WaitAndPlayBackgroundMusicAgain(float timer)
	{
		yield return new WaitForSeconds(timer);
		backgroundMusic.Play ();
	}
}
