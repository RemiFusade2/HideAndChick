using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	private Vector3 closePosition = new Vector3(0,5,-0.64f);
	private Vector3 farPosition = new Vector3(0,100,-19.03f);

	private float zoom;

	public float initialZoom;
	public float step;

	public GameObject player;


	// Use this for initialization
	void Start () {
		zoom = initialZoom;
		UpdatePosition();
	}
	
	// Update is called once per frame
	void Update () {
		float scroll = Input.GetAxis ("Mouse ScrollWheel");
		if (scroll != 0)
		{
			if (scroll < 0)
			{
				zoom += step;
				if (zoom > 1)
				{
					zoom = 1;
				}
			}
			if (scroll > 0)
			{
				zoom -= step;
				if (zoom < 0)
				{
					zoom = 0;
				}
			}
		}
		UpdatePosition ();
	}

	void UpdatePosition()
	{
		Vector3 playerPosition = player.transform.position;
		this.camera.transform.localPosition = playerPosition + Vector3.Slerp (closePosition, farPosition, zoom);
	}
}
