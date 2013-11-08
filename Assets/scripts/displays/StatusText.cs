using UnityEngine;
using System.Collections;

public class StatusText : MonoBehaviour {
	
	public float displayTime;
	private float displayTimer;
	
	public float blinkTime;
	
	//public GameObject textObject;
	public tk2dTextMesh textMesh;
	
	private bool isBlinking;
	
	private bool showingScoreText;
	
	public Color deathColor;
	public Color scoreColor;
	
	public float alpha;
	
	public tk2dSprite fadeSprite;
	public float bigFadeAlpha, softFadeAlpha;

	// Use this for initialization
	void Start () {
		textMesh.gameObject.SetActive(false);
		displayTimer = 0;
		
		isBlinking = true;
		showingScoreText = false;
		
		deathColor.a = alpha;
		scoreColor.a = alpha;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (displayTimer > 0){
			if (Time.timeScale > 0){
				displayTimer -= Time.deltaTime/Time.timeScale;  //do not let time scale affect this
			}else{
				displayTimer = 0;
			}
			if (isBlinking){
				textMesh.gameObject.SetActive( displayTimer%blinkTime > blinkTime/2 );
			}
			
			if (displayTimer <= 0){
				turnOff();
			}
			
		}else{
			showingScoreText = false;
		}
		
	}
	
	public void showScoreText(int score){
		textMesh.color = scoreColor;
		setText("ROUND OVER\nSCORE: "+score.ToString() );
		showingScoreText = true;
	}
	
	public void showDeathText(int livesLeft){
		textMesh.color = deathColor;
		if (livesLeft != 1){
			setText("YOU DIED\n"+livesLeft.ToString()+" LIVES LEFT");	
		}else{
			setText("YOU DIED\n"+livesLeft.ToString()+" LIFE LEFT");	
		}
	}
	
	public void showGhostKill(bool cloneDeadFoever){
		//ignore this if the score text is on screen
		if (showingScoreText){
			return;
		}
		
		textMesh.color = scoreColor;
		if (cloneDeadFoever){
			setText("GHOST\nEXORCISED!");
		}else{
			setText("GHOST SCORE!");
		}
		displayTimer *= 0.5f;
		
		fadeSprite.color = new Color(1,1,1, softFadeAlpha);
	}
	
	void setText(string curText){
		textMesh.text = curText;
		textMesh.Commit();
		displayTimer = displayTime;
		fadeSprite.color = new Color(1,1,1, bigFadeAlpha);
		fadeSprite.gameObject.SetActive(true);
	}
	
	public void turnOff(){
		textMesh.gameObject.SetActive(false);
		fadeSprite.gameObject.SetActive(false);
		DisplayTimer = 0;
	}
	
	
	//setters getters
	
	public float DisplayTimer {
		get {
			return this.displayTimer;
		}
		set {
			displayTimer = value;
		}
	}
	
}
