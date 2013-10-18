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
			displayTimer -= Time.deltaTime/Time.timeScale;  //do not elt time scale affect this
			
			if (isBlinking){
				textMesh.gameObject.SetActive( displayTimer%blinkTime > blinkTime/2 );
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
		setText("YOU DIED\n"+livesLeft.ToString()+" LIVES LEFT");	
	}
	
	public void showGhostKill(){
		//ignore this if the score text is on screen
		if (showingScoreText){
			return;
		}
		
		textMesh.color = scoreColor;
		setText("CLONE KILL!");
		displayTimer *= 0.5f;
	}
	
	void setText(string curText){
		textMesh.text = curText;
		textMesh.Commit();
		displayTimer = displayTime;
		
	}
	
	public void turnOff(){
		textMesh.gameObject.SetActive(false);
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
