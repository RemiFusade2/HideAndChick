using UnityEngine;
using System.Collections;

public class OutOfLimitBehaviour : MonoBehaviour {

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == "Player")
		{
			Ray upRay = new Ray(col.transform.position + 2000*Vector3.up, -Vector3.up);
			RaycastHit rayHit;
			int groundLayerMask = LayerMask.NameToLayer("Ground");
			if ( Physics.Raycast(upRay, out rayHit, 10000, groundLayerMask))
			{
				col.GetComponent<Rigidbody>().velocity = Vector3.zero;
				col.transform.localPosition = rayHit.point + 5 * Vector3.up;
			}
			else
			{
				col.GetComponent<Rigidbody>().velocity = Vector3.zero;
				col.transform.localPosition = new Vector3( col.transform.localPosition.x, 5, col.transform.localPosition.z);
			}
		}
	}
}
