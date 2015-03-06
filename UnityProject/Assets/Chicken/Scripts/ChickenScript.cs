using UnityEngine;
using System.Collections;

public class ChickenScript : MonoBehaviour {
	
	GameObject target;
	bool attached;

	// Use this for initialization
	void Start () 
	{
		target = null;
		this.GetComponent<Animator>().SetBool("walking", false);
		this.GetComponent<Animator>().SetBool("jumping", false);
		attached = false;
	}

	// Update is called once per frame
	void Update () 
	{
		// Chicken wants to move
		Vector3 wantedMovementDirection = new Vector3(this.rigidbody.velocity.x, 0, this.rigidbody.velocity.z);
		// Check terrain (wall, slope, etc)
		Ray rayToWantedDirection = new Ray (this.rigidbody.position, wantedMovementDirection);
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
					this.rigidbody.velocity = Vector3.zero;
				}
			}
		}
		if (target != null)
		{
			this.transform.LookAt (target.transform.position);
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
		if (col != null && (col.tag == "Poulet" || col.tag == "Player") && target == null)
		{
			target = col.gameObject;
			this.GetComponent<Animator>().SetBool("walking", true);
			SpringJoint joint = this.gameObject.AddComponent<SpringJoint>();
			joint.connectedBody = target.rigidbody;
			joint.spring = 400f;
			//joint.breakForce = 10f;
			joint.minDistance = 0;
			joint.maxDistance = 0.2f;
			GameObject.Find ("/Player").GetComponent<ControlScript>().AddChicken(this.gameObject);
			attached = true;
			this.transform.Find("ValidationAudioCrow").audio.Play();
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
		}
		return count;
	}
}
