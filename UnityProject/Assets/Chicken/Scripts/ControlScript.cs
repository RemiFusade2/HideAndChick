using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ControlScript : MonoBehaviour {

	public float speed = 5;
	public float jumpHeight = 5;

	private List<GameObject> attachedChickens;

	private Transform corps;

	private float jumpTimer;

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
		attachedChickens = new List<GameObject> ();
		FindCorps();
		jumpTimer = 0;
	}

	void FindCorps()
	{
		if (corps == null)
		{
			corps = transform.Find ("Corps");
		}
	}

	void UpdateText()
	{
		if (Input.GetKey(KeyCode.Keypad0))
		{
			GameObject text = GameObject.Find("InfoText");
			TextMeshPro textScript = text.GetComponent<TextMeshPro>();
			textScript.text = "";
		}
		if (Input.GetKey(KeyCode.Keypad1))
		{
			GameObject text = GameObject.Find("InfoText");
			TextMeshPro textScript = text.GetComponent<TextMeshPro>();
			textScript.text = "Open the door";
		}
		if (Input.GetKey(KeyCode.Keypad2))
		{
			GameObject text = GameObject.Find("InfoText");
			TextMeshPro textScript = text.GetComponent<TextMeshPro>();
			textScript.text = "Find the chicks";
		}
		if (Input.GetKey(KeyCode.Keypad3))
		{
			GameObject text = GameObject.Find("InfoText");
			TextMeshPro textScript = text.GetComponent<TextMeshPro>();
			textScript.text = "Bring them home";
		}
		if (Input.GetKey(KeyCode.Keypad4))
		{
			GameObject text = GameObject.Find("InfoText");
			TextMeshPro textScript = text.GetComponent<TextMeshPro>();
			textScript.text = "Play as a straight white male chicken";
		}
		if (Input.GetKey(KeyCode.Keypad5))
		{
			GameObject text = GameObject.Find("InfoText");
			TextMeshPro textScript = text.GetComponent<TextMeshPro>();
			textScript.text = "Use physics";
		}
		if (Input.GetKey(KeyCode.Keypad6))
		{
			GameObject text = GameObject.Find("InfoText");
			TextMeshPro textScript = text.GetComponent<TextMeshPro>();
			textScript.text = "Find ALL the chicks!";
		}
		if (Input.GetKey(KeyCode.Keypad7))
		{
			GameObject text = GameObject.Find("InfoText");
			TextMeshPro textScript = text.GetComponent<TextMeshPro>();
			textScript.text = "Walk on lava!";
		}
		if (Input.GetKey(KeyCode.Keypad8))
		{
			GameObject text = GameObject.Find("InfoText");
			TextMeshPro textScript = text.GetComponent<TextMeshPro>();
			textScript.text = "Find secret paths";
		}
		if (Input.GetKey(KeyCode.Keypad9))
		{
			GameObject text = GameObject.Find("InfoText");
			TextMeshPro textScript = text.GetComponent<TextMeshPro>();
			textScript.text = "Play as a chicken";
		}
	}

	// Update is called once per frame
	void Update () 
	{
		// check objects and init
		FindCorps ();

		// Debug
		UpdateText ();

		// Get input for movement
		Vector3 wantedMovementDirection = Vector3.zero;
		if (Input.GetKey(KeyCode.UpArrow))
		{
			wantedMovementDirection += Vector3.forward;
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			wantedMovementDirection += Vector3.back;
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			wantedMovementDirection += Vector3.right;
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			wantedMovementDirection += Vector3.left;
		}
		Vector3 actualMovementDirection = Vector3.zero;
		
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
					float slopeRayHeight = 0.5f;
					float radius = Mathf.Abs(slopeRayHeight / Mathf.Sin (slopeAngle));
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
		if (Physics.Raycast(bottomRay, out resultingHit, distanceForSlide) && resultingHit.collider.gameObject.tag != "Poulet")
		{
			// Player is on something else than a chicken, he can jump
			float slopeAngle = Vector3.Angle(Vector3.up, resultingHit.normal);
			if (resultingHit.distance < distanceForJump && Time.time > jumpTimer + 0.3f)
			{
				this.GetComponent<Animator>().SetBool("jumping", false);
				this.transform.Find("JumpAudio").audio.Stop();
				if (slopeAngle < 45 && Input.GetKeyDown(KeyCode.Space))
				{
					jump = Vector3.up;
					this.transform.Find("JumpAudio").audio.Play();
					this.GetComponent<Animator>().SetBool("jumping", true);
					jumpTimer = Time.time;
				}

			}
			// He will also slide
			if (slopeAngle > 30 && resultingHit.collider.gameObject.tag == "Ground")
			{
				float ratio =  (slopeAngle-30)/45;
				slide = new Vector3(resultingHit.normal.x * ratio, 0, resultingHit.normal.z * ratio);
			}
		}

		// Slide, move and jump if needed
		Vector3 newVelocity = new Vector3 (actualMovementDirection.x * speed + slide.x, rigidbody.velocity.y + jump.y * jumpHeight, actualMovementDirection.z * speed + slide.z);
		rigidbody.velocity = newVelocity;
		if (wantedMovementDirection.magnitude > 0)
		{
			// Rotate to face wanted movement direction
			Vector3 directionLook = new Vector3(wantedMovementDirection.x, 0, wantedMovementDirection.z);
			corps.LookAt(corps.position + directionLook);
		}
		this.GetComponent<Animator>().SetBool("walking", (wantedMovementDirection.magnitude > 0));
	}
}
