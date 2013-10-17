using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
	
	public PlayerController player; 
	
	public GUIStyle textStyle;
	
	public GameObject anchor;
	public tk2dSlicedSprite spriteBackground;
		

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		spriteBackground.gameObject.transform.position = anchor.transform.position;
	}
	
	void OnGUI(){
		
		float xPos = 5;
		float yPos = 7;
		
		if (player.HudShakeTimer > 0){
			float shakeRange = 5;
			xPos += Random.Range(-shakeRange, shakeRange);
			yPos += Random.Range(-shakeRange, shakeRange);
		}
		
		Rect textPos = new Rect(xPos,yPos,150,300);
		
		//int dispNum = 
		string topText = "";
		
		//lives
		topText += "Lives: "+player.LivesLeft;
		//score
		topText += "\nScore: "+player.Score;
		
		/*
		//then list powers
		topText += "\n";
		for (int k=0; k<player.Powers.Count; k++){
			topText += "\n"+player.Powers[k].powerName;
		}
		*/
		
		
		GUI.Label(textPos, topText, textStyle);
	}
}
