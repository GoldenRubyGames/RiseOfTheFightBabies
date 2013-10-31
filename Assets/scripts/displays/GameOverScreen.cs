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
	
	public float minTimeOnScreen;
	private float timer;
	private bool canKill;
	
	void Start(){
		isActive = false;
		
	}
	
	public void turnOn(){
		isActive = true;
		gameObject.SetActive(true);
		
		timer = minTimeOnScreen;
		canKill = false;
	}
	
	public void turnOn(int score, int rounds, bool _isHighScore, GameManager gm){
		turnOn();
	
		isHighScore = _isHighScore;
		
		scoreText.text = "SCORE: "+score.ToString();
		scoreText.Commit();
		roundsText.text = "ROUNDS: "+rounds.ToString();
		roundsText.Commit();
		
		highScoreText.gameObject.SetActive(true);
		highScoreText.text = "HIGH SCORE: "+gm.dataHolder.HighScores[gm.CurLevelNum].ToString();
		highScoreText.Commit();
		
		setUnlockText(gm);
		
		bottomText.text = "Press R to try again\nPress Q for level select";
		//check for xBox controls
		if (Input.GetJoystickNames().Length > 0){
			bottomText.text = "Press A to try again\nPress BACK for level select";
		}
		bottomText.Commit();
		
		timer = 0;
	}
	
	public void setUnlockText(GameManager gm){
		//unlock text
		Debug.Log("hey fuck face, "+gm.dataHolder.CloneKills.ToString() );
		cloneKillsText.text = "Total Clone Kills: "+gm.dataHolder.CloneKills.ToString();
		cloneKillsText.Commit();
		if (!gm.unlockManager.DoneWithUnlocks){
			nextUnlockText.text = "Next Unlock: "+gm.unlockManager.UnlockVals[ gm.unlockManager.NextUnlock ];
		}else{
			nextUnlockText.text = "Next Unlock: NEVER";
		}
		nextUnlockText.Commit();
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
		
		timer -= Time.deltaTime;
		if (timer <= 0){
			canKill=true;
		}
			
	}
	
	
	
	public bool CanKill {
		get {
			return this.canKill;
		}
	}
	
}
