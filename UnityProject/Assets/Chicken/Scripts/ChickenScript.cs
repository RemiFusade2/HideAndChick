using UnityEngine;
using System.Collections;

public class ChickenScript : MonoBehaviour {
	
	GameObject target = null;
	bool attached;

	private float lastTwitteringTimer;

	private bool hasBeenCounted;

	private GameObject player;

	// Use this for initialization
	void Start () 
	{
		this.GetComponent<Animator>().SetBool("walking", false);
		this.GetComponent<Animator>().SetBool("jumping", false);
		attached = false;
		lastTwitteringTimer = 0;
		hasBeenCounted = false;
		player = null;
	}

	private void InitPlayerGameObject()
	{
		if (player == null)
		{
			player = GameObject.Find ("Player");
		}
	}

	// Update is called once per frame
	void Update () 
	{
		InitPlayerGameObject ();


		//this.GetComponent<Animator>().SetBool("twiterring", false);
		// Chicken wants to move
		Vector3 wantedMovementDirection = new Vector3(this.GetComponent<Rigidbody>().velocity.x, 0, this.GetComponent<Rigidbody>().velocity.z);
		// Check terrain (wall, slope, etc)
		Ray rayToWantedDirection = new Ray (this.GetComponent<Rigidbody>().position, wantedMovementDirection);
		RaycastHit resultingHit;
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
					// We are near a steep slope: don't authorize move!
					this.GetComponent<Rigidbody>().velocity = Vector3.zero;
				}
			}
		}
		if (target != null)
		{
			this.transform.LookAt (target.transform.position);
		}

		if (Time.time > lastTwitteringTimer+0.5f)
		{
			this.GetComponent<Animator>().SetBool("twiterring", false);
		}

		float magnitude = this.GetComponent<Rigidbody> ().velocity.magnitude;
		float maxMagnitude = 50.0f;
		if (magnitude > maxMagnitude)
		{
			this.GetComponent<Rigidbody> ().velocity = this.GetComponent<Rigidbody> ().velocity * (maxMagnitude / magnitude);
		}
	}
	
	void OnCollisionEnter(Collision col)
	{
		CollideWithObject (col.gameObject);
	}

	void OnTriggerEnter(Collider col)
	{
		CollideWithObject (col.gameObject);
	}

	void CollideWithObject(GameObject col)
	{
		if (col != null && (col.tag == "Poulet" || col.tag == "Player") && target == null && !hasBeenCounted)
		{
			//target = col.gameObject;
			target = player.gameObject;
			this.GetComponent<Animator>().SetBool("walking", true);
			SpringJoint joint = this.gameObject.AddComponent<SpringJoint>();
			joint.connectedBody = target.GetComponent<Rigidbody>();
			joint.connectedAnchor = Vector3.up;
			joint.spring = 100;
			//joint.breakForce = 10f;
			joint.minDistance = 0.5f;
			joint.maxDistance = 0.5f;
			joint.damper = 0.1f;
			joint.enableCollision = true;
			GameObject.Find ("/Player").GetComponent<ControlScript>().AddChicken(this.gameObject);
			attached = true;
			this.transform.Find("ValidationAudioCrow").GetComponent<AudioSource>().Play();
		}
	}

	public int RemoveSpringJoint()
	{
		int count = 0;
		SpringJoint joint = this.GetComponent<SpringJoint> ();
		if (joint != null && attached)
		{
			attached = false;
			count = 1;
			Destroy (joint);
			this.GetComponent<Rigidbody>().velocity = Vector3.zero;
			this.GetComponent<Rigidbody>().mass = float.MaxValue;
		}
		if (!hasBeenCounted)
		{
			hasBeenCounted = true;
			count = 1;
		}
		return count;
	}

	public bool HasBeenFound()
	{
		return (target != null) && !attached && hasBeenCounted;
	}

	public void SetFound(GameObject player, Vector3 pos)
	{
		target = player;
		attached = false;
		this.transform.position = pos;
		hasBeenCounted = true;
		this.GetComponent<Rigidbody>().mass = float.MaxValue;
	}

	void OnMouseDown()
	{
		if (Time.time > lastTwitteringTimer+0.5f)
		{
			this.transform.Find("ValidationAudioCrow").GetComponent<AudioSource>().Stop();
			this.transform.Find("ValidationAudioCrow").GetComponent<AudioSource>().Play();
			this.GetComponent<Animator>().SetBool("twiterring", true);
			lastTwitteringTimer = Time.time;
		}
	}
}
