using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiniMapBehaviour : MonoBehaviour {

	public GameObject player;
	public GameObject playerToken;

	public Renderer fogOfWar;
	private Texture2D fogTexture;

	public int fogTextureResolution;

	public GameObject poulailler;


	public List<GameObject> poussins;
	public List<GameObject> poussinsTokens;

	public bool big;

	// Use this for initialization
	void Start () 
	{
		float z = this.transform.localPosition.z;
		float y = this.transform.localPosition.y;
		
		Vector3 leftPosition = this.transform.parent.camera.ScreenToWorldPoint (new Vector3 (0, Screen.height-30, z));
		
		this.transform.localPosition = new Vector3(leftPosition.x+0.2f, y, z);

		fogTexture = new Texture2D(fogTextureResolution,fogTextureResolution);
		for (int i = 0 ; i < fogTexture.width ; i++)
		{			
			for (int j = 0 ; j < fogTexture.height ; j++)
			{
				fogTexture.SetPixel (i, j, Color.black);
			}
		}
		fogTexture.Apply ();
		fogOfWar.material.mainTexture = fogTexture;
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdatePlayerPosition ();
		UpdatePoussinsPositions ();
		UpdateFogOfWar ();
	}

	void UpdatePlayerPosition()
	{
		Vector3 playerPos = player.transform.position;
		Vector3 playerTokenPos = playerPos * 5 / 1000;
		playerToken.transform.localPosition = new Vector3 (playerTokenPos.x, playerToken.transform.localPosition.y, playerTokenPos.z);
	}

	void UpdateFogOfWar()
	{
		int visibility = fogTextureResolution/10; // en pixels
		Vector3 playerPos = player.transform.position;
		Vector3 playerTokenPos = playerPos * 5 / 1000;
		int playerPosInFogTexture_x = Mathf.FloorToInt (((5 - playerTokenPos.x) / 10.0f) * fogTexture.width);
		int playerPosInFogTexture_y = Mathf.FloorToInt (((5 - playerTokenPos.z) / 10.0f) * fogTexture.height);

		int x, y;
		for (int i = - visibility ; i < visibility ; i++)
		{
			x = playerPosInFogTexture_x + i;
			for (int j = - visibility ; j < visibility ; j++)
			{
				y = playerPosInFogTexture_y + j;
				float dist = i*i + j*j;
				if (x >= 0 && x < fogTexture.width && y >= 0 && y < fogTexture.height && dist < visibility*visibility)
				{
					fogTexture.SetPixel (x, y, new Color (0, 0, 0, 0));
				}
			}
		}
		fogTexture.Apply ();
	}

	void UpdatePoussinsPositions()
	{
		int index = 0;
		foreach (GameObject poussin in poussins)
		{
			GameObject poussinToken = poussinsTokens[index];

			Vector3 poussinPos = poussin.transform.position;
			Vector3 poussinTokenPos = poussinPos * 5 / 1000;
			poussinToken.transform.localPosition = new Vector3 (poussinTokenPos.x, poussinToken.transform.localPosition.y, poussinTokenPos.z);

			index++;
		}
	}

	void OnMouseDown()
	{
		big = !big;
		this.GetComponent<Animator> ().SetBool ("big", big);
	}
}
