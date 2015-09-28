using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MiniMapBehaviour : MonoBehaviour {

	public GameObject player;
	public GameObject playerToken;

	public GameObject poulailler;
	public GameObject poulaillerToken;
	
	public List<GameObject> poussins;
	private List<GameObject> poussinsTokens;
	public GameObject poussinsTokensContainer;
	public GameObject poussinTokenPrefab;

	private Texture2D fogTexture;
	public RawImage warFogImage;
	
	public Texture2D getFogTexture()  
	{
		return fogTexture;
	}
	public void setFogTexture(Texture2D newTexture)  
	{
		fogTexture = newTexture;
		warFogImage.texture = fogTexture;
	}

	public int fogTextureResolution;

	// Use this for initialization
	void Start () 
	{
		InitPoussinsPositions ();
		InitPoulaillerPosition ();
	}

	public void ResetWarFog()
	{
		InitWarFog ();
	}

	private void InitWarFog()
	{
		fogTexture = new Texture2D(fogTextureResolution,fogTextureResolution);
		for (int i = 0 ; i < fogTexture.width ; i++)
		{			
			for (int j = 0 ; j < fogTexture.height ; j++)
			{
				fogTexture.SetPixel (i, j, Color.black);
			}
		}
		fogTexture.Apply ();
		warFogImage.texture = fogTexture;
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
		float minX = -76;
		float maxX = 76;
		float minY = -76;
		float maxY = 76;

		Vector3 playerPos = player.transform.position;
		Vector3 playerTokenPos = (playerPos + Vector3.one * 1000) / 2000 ;
		playerToken.GetComponent<RectTransform> ().localPosition = new Vector3 (minX+playerTokenPos.x*(maxX-minX), minY+playerTokenPos.z*(maxY-minY), 0);
	}

	void UpdateFogOfWar()
	{
		int visibility = fogTextureResolution/10; // en pixels
		Vector3 playerPos = player.transform.position;
		Vector3 playerTokenPos = (playerPos + Vector3.one * 1000) / 2000 ;
		int playerPosInFogTexture_x = Mathf.FloorToInt (playerTokenPos.x * fogTexture.width);
		int playerPosInFogTexture_y = Mathf.FloorToInt (playerTokenPos.z * fogTexture.height);

		int x, y;
		Color fullTransparent = new Color (0, 0, 0, 0);
		Color halfTransparent = new Color (0, 0, 0, 0.3f);
		for (int i = - visibility - 3 ; i < visibility + 3 ; i++)
		{
			x = playerPosInFogTexture_x + i;
			for (int j = - visibility - 3 ; j < visibility + 3 ; j++)
			{
				y = playerPosInFogTexture_y + j;
				float dist = i*i + j*j;
				if (x >= 0 && x < fogTexture.width && y >= 0 && y < fogTexture.height && dist <= visibility*visibility)
				{
					fogTexture.SetPixel (x, y, fullTransparent);
				}
				else if (x >= 0 && x < fogTexture.width && y >= 0 && y < fogTexture.height && dist <= (visibility*visibility+100))
				{
					fogTexture.SetPixel (x, y, halfTransparent);
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

			float minX = -76;
			float maxX = 76;
			float minY = -76;
			float maxY = 76;
			
			Vector3 poussinPos = poussin.transform.position;
			Vector3 poussinTokenPos = (poussinPos + Vector3.one * 1000) / 2000 ;
			poussinToken.GetComponent<RectTransform> ().localPosition = new Vector3 (minX+poussinTokenPos.x*(maxX-minX), minY+poussinTokenPos.z*(maxY-minY), 0);

			index++;
		}
	}

	private void InitPoussinsPositions()
	{
		poussinsTokens = new List<GameObject> ();
		for (int i = 0 ; i < poussins.Count ; i++)
		{
			GameObject poussinToken = (GameObject) Instantiate(poussinTokenPrefab);
			poussinToken.transform.SetParent ( poussinsTokensContainer.transform );
			poussinsTokens.Add (poussinToken);
		}
	}
	
	private void InitPoulaillerPosition()
	{
		float minX = -76;
		float maxX = 76;
		float minY = -76;
		float maxY = 76;		
		Vector3 poulaillerPos = poulailler.transform.position;
		Vector3 poulaillerTokenPos = (poulaillerPos + Vector3.one * 1000) / 2000 ;
		poulaillerToken.GetComponent<RectTransform> ().localPosition = new Vector3 (minX+poulaillerTokenPos.x*(maxX-minX), minY+poulaillerTokenPos.z*(maxY-minY), 0);
	}
}
