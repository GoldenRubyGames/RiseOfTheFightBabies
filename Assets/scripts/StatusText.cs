using UnityEngine;
using System.Collections;

public class StatusText : MonoBehaviour {
	
	public float displayTime;
	private float displayTimer;
	
	public float blinkTime;
	
	public TextMesh textMesh;
	
	private bool isBlinking;
	
	private bool showingScoreText;

	// Use this for initialization
	void Start () {
		renderer.enabled = false;
		displayTimer = 0;
		
		isBlinking = true;
		showingScoreText = false;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (displayTimer > 0){
			displayTimer -= Time.deltaTime/Time.timeScale;  //do not elt time scale affect this
			
			if (isBlinking){
				renderer.enabled = displayTimer%blinkTime > blinkTime/2;
			}
		}else{
			showingScoreText = false;
		}
		
	}
	
	public void showScoreText(int score){
		setText("HELL YEAH BRO\nSCORE: "+score.ToString() );
		showingScoreText = true;
	}
	
	public void showDeathText(int livesLeft){
		setText("YOU DIED\n"+livesLeft.ToString()+" LIVES LEFT");
	}
	
	public void showGhostKill(){
		//ignore this if the score text is on screen
		if (showingScoreText){
			return;
		}
		
		setText("GHOST KILL!");
		displayTimer *= 0.5f;
	}
	
	public void showEndGame(int score){
		setText("GAME OVER!\nSCORE: "+score);
		//isBlinking = false;
		renderer.enabled = true;
		displayTimer = 100;
	}
	
	void setText(string curText){
		textMesh.text = curText;
		displayTimer = displayTime;
		
	}
	
	
}
