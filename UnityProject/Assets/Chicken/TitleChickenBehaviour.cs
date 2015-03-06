﻿using UnityEngine;
using System.Collections;
using TMPro;

public class TitleChickenBehaviour : MonoBehaviour {

	public float initSpeedX;
	public float initSpeedY;
	public float initSpeedZ;

	private float timer;
	
	public float ENJMINFadeInTime;
	public float ENJMINFadeOutTime;
	public float TitleFadeInTime;
	public float ChickenStartTime;
	public float ChickenEndTime;

	private int ENJMINAlpha;
	private int TitleAlpha;

	// Use this for initialization
	void Start () {
		timer = Time.time;
		ENJMINAlpha = 0;
		TitleAlpha = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Time.time > timer + 60)
		{
			Destroy(this.gameObject);
		}
		else if (Time.time > timer + 32)
		{
			TextMeshPro meshscript = GameObject.Find("OpenWorldText").GetComponent<TextMeshPro>();
			Color32 c = new Color32(255,255,255,255);
			meshscript.color = c;
		}
		else if (Time.time > timer + 31)
		{
			TextMeshPro meshscript = GameObject.Find("SecretsText").GetComponent<TextMeshPro>();
			Color32 c = new Color32(255,255,255,0);
			meshscript.color = c;
			meshscript = GameObject.Find("ParticleFXText").GetComponent<TextMeshPro>();
			c = new Color32(255,255,255,255);
			meshscript.color = c;
			meshscript = GameObject.Find("ExploreText").GetComponent<TextMeshPro>();
			c = new Color32(255,255,255,0);
			meshscript.color = c;
		}
		else if (Time.time > timer + 28)
		{
			TextMeshPro meshscript = GameObject.Find("SecretsText").GetComponent<TextMeshPro>();
			Color32 c = new Color32(255,255,255,255);
			meshscript.color = c;
		}
		else if (Time.time > timer + 26)
		{
			TextMeshPro meshscript = GameObject.Find("LDText").GetComponent<TextMeshPro>();
			Color32 c = new Color32(255,255,255,0);
			meshscript.color = c;
			meshscript = GameObject.Find("ExploreText").GetComponent<TextMeshPro>();
			c = new Color32(255,255,255,255);
			meshscript.color = c;
		}
		else if (Time.time > timer + 20)
		{
			TextMeshPro meshscript = GameObject.Find("FindingChicksText").GetComponent<TextMeshPro>();
			Color32 c = new Color32(255,255,255,0);
			meshscript.color = c;
			meshscript = GameObject.Find("LDText").GetComponent<TextMeshPro>();
			c = new Color32(255,255,255,255);
			meshscript.color = c;
			meshscript = GameObject.Find("TitleMesh").GetComponent<TextMeshPro>();
			c = new Color32(255,255,0,0);
			meshscript.color = c;
			meshscript = GameObject.Find("CreatedTExt").GetComponent<TextMeshPro>();
			c = new Color32(255,255,255,0);
			meshscript.color = c;
		}
		else if (Time.time > timer + 15)
		{
			TextMeshPro meshscript = GameObject.Find("FindingChicksText").GetComponent<TextMeshPro>();
			Color32 c = new Color32(255,255,255,255);
			meshscript.color = c;
		}
		else if (Time.time > timer + ChickenEndTime)
		{
			this.rigidbody.useGravity = false;
			this.enabled = false;
		}
		else if (Time.time > timer + ChickenStartTime)
		{
			this.rigidbody.useGravity = true;
			this.rigidbody.velocity = new Vector3 (initSpeedX, initSpeedY, initSpeedZ);
			this.rigidbody.angularVelocity = new Vector3 (100, 0, 0);
		}
		else if (Time.time > timer + TitleFadeInTime)
		{
			TextMeshPro titlemeshscript = GameObject.Find("TitleMesh").GetComponent<TextMeshPro>();
			Color32 c = new Color32(255,255,0,(byte)TitleAlpha);
			titlemeshscript.color = c;
			TitleAlpha += 2;
			if (TitleAlpha > 255)
			{
				TitleAlpha = 255;
			}
		}
		else if (Time.time > timer + ENJMINFadeOutTime)
		{
			TextMeshPro textmeshscript = GameObject.Find("ENJMINPresents").GetComponent<TextMeshPro>();
			Color32 c = new Color32(205,30,55,(byte)ENJMINAlpha);
			textmeshscript.color = c;
			textmeshscript.ForceMeshUpdate();
			ENJMINAlpha -= 2;
			if (ENJMINAlpha < 0)
			{
				ENJMINAlpha = 0;
			}
		}
		else if (Time.time > timer + ENJMINFadeInTime)
		{
			TextMeshPro textmeshscript = GameObject.Find("ENJMINPresents").GetComponent<TextMeshPro>();
			Color32 c = new Color32(205,30,55,(byte)ENJMINAlpha);
			textmeshscript.color = c;
			textmeshscript.ForceMeshUpdate();
			ENJMINAlpha += 2;
			if (ENJMINAlpha > 255)
			{
				ENJMINAlpha = 255;
			}
		}




	}
}