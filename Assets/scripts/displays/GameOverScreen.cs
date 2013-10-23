using UnityEngine;
using System.Collections;

public class GameOverScreen : MonoBehaviour {
	
	//public TextMesh scoreText;
	public GameManager gm;
	
	public float blinkSpeed;
	
	public GUIStyle style;
	
	private bool isActive;
	
	private string scoreText;
	
	private bool isHighScore;
	
	
	public Vector2 boxSize;  //this should be the width and height of the box image
	
	void Start(){
		isActive = false;
	}
	
	public void turnOn(int score, bool _isHighScore){
		gameObject.SetActive(true);
		scoreText = "GAME OVER\nSCORE: "+score.ToString();
		//scoreText.text = "Your score: "+ score.ToString();
		isActive = true;
		isHighScore = _isHighScore;
	}
	
	public void turnOff(){
		gameObject.SetActive(false);
		isActive = false;
	}
	
	
	void Update () {
		
		
		//bool isOn = (Time.time%blinkSpeed) < blinkSpeed;
		//scoreText.gameObject.SetActive(isOn);
		
	}
	
	void OnGUI(){
		
		//don't show anything if the unlock screen is active
		if (gm.UnlockScreenPopUp == null){
		
			string textThisFrame = scoreText;
			if (isHighScore && Time.time%blinkSpeed < blinkSpeed/2){
				textThisFrame = "GAME OVER\n";
			}
			
			bool isOn = (Time.time%blinkSpeed) < blinkSpeed;
			
			float xPos = Screen.width/2 - boxSize.x/2;
			float yPos = Screen.height/2 - boxSize.y/2;
			
			Rect textPos = new Rect(xPos,yPos, boxSize.x, boxSize.y);
			GUI.Label(textPos, textThisFrame, style);
		}
	}
}
