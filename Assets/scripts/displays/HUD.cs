using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
	
	public PlayerController player; 
	
	public Vector2 boxSize;
	
	public GUIStyle textStyle;
		

	// Use this for initialization
	void Start () {
	
	}
	
	void OnGUI(){
		
		//draw the background
		//GUI.DrawTexture( new Rect(0,0, 350, 85), boxImage, textStyle);
		
		//imgStyle.Draw(new Rect(0,0,35,85), null, 0);
		
		float xPos = 2;
		float yPos = 2;
		
		if (player.HudShakeTimer > 0){
			float shakeRange = 5;
			xPos += Random.Range(-shakeRange, shakeRange);
			yPos += Random.Range(-shakeRange, shakeRange);
		}
		
		Rect textPos = new Rect(xPos,yPos,boxSize.x,boxSize.y);
		
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
