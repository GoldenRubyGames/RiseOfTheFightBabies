using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
	
	public PlayerController[] players; 
	
	public GUIStyle textStyle0;
	public GUIStyle textStyle1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI(){
		
		for (int i=0; i<players.Length; i++){
			float xPos = i == 0 ? 5 : Screen.width-160;
			float yPos = 5;
			
			if (players[i].HudShakeTimer > 0){
				float shakeRange = 5;
				xPos += Random.Range(-shakeRange, shakeRange);
				yPos += Random.Range(-shakeRange, shakeRange);
			}
			
			Rect textPos = new Rect(xPos,yPos,150,300);
			
			//int dispNum = 
			string playerName = "";
			if(i==0){
				playerName = "Player "+(i+1).ToString();
			}else{	
				for (int k=0; k<players[i].Health; k++) playerName+="[-]";
				playerName += ": Player "+(i+1).ToString();
			}
			
			//lives
			playerName += "\nLives: "+players[i].LivesLeft;
			
			//score
			playerName += "\nScore: "+players[i].Score;
			
			//then list powers
			for (int k=0; k<players[i].Powers.Count; k++){
				playerName += "\n"+players[i].Powers[k].powerName;
			}
			
			//textStyle.alignment = i==0 ? TextAnchor.UpperLeft : TextAnchor.UpperRight;
			//textStyle.normal.textColor = players[i].myColor;
			
			GUI.Label(textPos, playerName, i==0 ? textStyle0 : textStyle1);
		}
	}
}
