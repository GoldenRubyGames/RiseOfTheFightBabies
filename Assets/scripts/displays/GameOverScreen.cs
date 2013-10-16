using UnityEngine;
using System.Collections;

public class GameOverScreen : MonoBehaviour {
	
	public TextMesh scoreText;
	
	public float blinkSpeed;

	public void turnOn(int score){
		gameObject.SetActive(true);
		scoreText.text = "Your score: "+ score.ToString();
	}
	
	public void turnOff(){
		gameObject.SetActive(false);
	}
	
	
	void Update () {
		
		//blink the score
		bool isOn = (Time.time%blinkSpeed) < blinkSpeed;
		scoreText.gameObject.SetActive(isOn);
		
	}
}
