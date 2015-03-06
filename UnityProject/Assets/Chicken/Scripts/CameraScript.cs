using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	private Vector3 closePosition = new Vector3(0,5,-0.64f);
	private Vector3 farPosition = new Vector3(0,100,-19.03f);

	private float zoom;

	public float initialZoom = 0.3f;
	public float step = 0.03f;


	// Use this for initialization
	void Start () {
		zoom = initialZoom;
		UpdatePositionFromZoom();
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
			UpdatePositionFromZoom();
		}
	}

	void UpdatePositionFromZoom()
	{
		this.camera.transform.localPosition = Vector3.Slerp (closePosition, farPosition, zoom);
	}
}
