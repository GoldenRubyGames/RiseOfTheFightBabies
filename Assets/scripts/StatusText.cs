using UnityEngine;
using System.Collections;

public class StatusText : MonoBehaviour {
	
	public float displayTime;
	private float displayTimer;
	
	public float blinkTime;
	
	public TextMesh textMesh;
	
	private bool isBlinking;

	// Use this for initialization
	void Start () {
		renderer.enabled = false;
		displayTimer = 0;
		
		isBlinking = true;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (displayTimer > 0){
			displayTimer -= Time.deltaTime;
			
			if (isBlinking){
				renderer.enabled = displayTimer%blinkTime > blinkTime/2;
			}
		}
		
	}
	
	public void showScoreText(int score){
		setText("HELL YEAH BRO\nSCORE: "+score.ToString() );
	}
	
	public void showDeathText(int livesLeft){
		setText("YOU DIED\n"+livesLeft.ToString()+" LIVES LEFT");
	}
	
	public void showGhostKill(){
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
