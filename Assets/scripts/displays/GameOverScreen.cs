using UnityEngine;
using System.Collections;

public class GameOverScreen : MonoBehaviour {
	
	//public TextMesh scoreText;
	public GameManager gm;
	
	public float blinkSpeed;
	
	private bool isActive;
	
	private bool isHighScore;
	
	public tk2dTextMesh gameOverText, scoreText, roundsText, bottomText, highScoreText, cloneKillsText, nextUnlockText;
	
	
	public Vector2 boxSize;  //this should be the width and height of the box image
	
	void Start(){
		isActive = false;
		
	}
	
	public void turnOn(int score, int rounds, bool _isHighScore, GameManager gm){
		gameObject.SetActive(true);
		//scoreText = "GAME OVER\nSCORE: "+score.ToString();
		//scoreText.text = "Your score: "+ score.ToString();
		isActive = true;
		isHighScore = _isHighScore;
		
		scoreText.text = "SCORE: "+score.ToString();
		scoreText.Commit();
		roundsText.text = "ROUNDS: "+rounds.ToString();
		roundsText.Commit();
		
		highScoreText.text = "HIGH SCORE: "+gm.dataHolder.HighScores[gm.CurLevelNum].ToString();
		highScoreText.Commit();
		
		//unlock text
		cloneKillsText.text = "Total Clone Kills: "+gm.dataHolder.CloneKills.ToString();
		cloneKillsText.Commit();
		if (!gm.unlockManager.DoneWithUnlocks){
			nextUnlockText.text = "Next Unlock: "+gm.unlockManager.UnlockVals[ gm.unlockManager.NextUnlock ];
		}else{
			nextUnlockText.text = "Next Unlock: NEVER";
		}
		nextUnlockText.Commit();
		
		//check for xBox controls
		if (Input.GetJoystickNames().Length > 0){
			bottomText.text = "Press A to try again\nPress BACK to quit";
			bottomText.Commit();
		}
	}
	
	public void turnOff(){
		gameObject.SetActive(false);
		isActive = false;
	}
	
	
	void Update () {
		
		if (isHighScore){
			highScoreText.gameObject.SetActive( Time.time%blinkSpeed < blinkSpeed/2 );
		}
		
		gameOverText.gameObject.SetActive( Time.time%blinkSpeed < blinkSpeed/2 );
		
		//bool isOn = (Time.time%blinkSpeed) < blinkSpeed;
		//scoreText.gameObject.SetActive(isOn);
		
	}
	
}
