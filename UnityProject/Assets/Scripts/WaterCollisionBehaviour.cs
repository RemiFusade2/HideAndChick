using UnityEngine;
using System.Collections;

public class WaterCollisionBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player")
		{
			col.transform.Find ("WaterWalkAudio").GetComponent<AudioSource>().Play();
		}
	}
	
	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.tag == "Player")
		{
			col.transform.Find ("WaterWalkAudio").GetComponent<AudioSource>().Stop();
		}
	}
}
