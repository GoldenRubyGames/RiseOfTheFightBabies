using UnityEngine;
using System.Collections;

public class GameOverScreen : MonoBehaviour {
	
	//public TextMesh scoreText;
	
	public float blinkSpeed;
	
	public GUIStyle style;
	
	private bool isActive;
	
	private string scoreText;
	
	
	public Vector2 boxSize;  //this should be the width and height of the box image
	
	void Start(){
		isActive = false;
	}
	
	public void turnOn(int score){
		gameObject.SetActive(true);
		scoreText = "GAME OVER\nSCORE: "+score.ToString();
		//scoreText.text = "Your score: "+ score.ToString();
		isActive = true;
	}
	
	public void turnOff(){
		gameObject.SetActive(false);
		isActive = false;
	}
	
	
	void Update () {
		
		//blink the score
		//bool isOn = (Time.time%blinkSpeed) < blinkSpeed;
		//scoreText.gameObject.SetActive(isOn);
		
	}
	
	void OnGUI(){
		
		bool isOn = (Time.time%blinkSpeed) < blinkSpeed;
		
		float xPos = Screen.width/2 - boxSize.x/2;
		float yPos = Screen.height/2 - boxSize.y/2;
		
		Rect textPos = new Rect(xPos,yPos, boxSize.x, boxSize.y);
		GUI.Label(textPos, scoreText, style);
	}
}
