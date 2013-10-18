using UnityEngine;
using System.Collections;

public class PauseScreen : MonoBehaviour {
	
	public bool debugSkipTitle;
	
	public tk2dSprite mainSprite;
	
	public GameObject centerAnchor;
	
	private bool showingTitle;
	
	void Awake(){
		showingTitle = false;
	}

	public void activate(bool showHowTo){
		gameObject.SetActive(true);
		mainSprite.SetSprite( showHowTo ? "howToCard" : "pauseBox" );
		
		mainSprite.gameObject.transform.position = new Vector3( centerAnchor.transform.position.x, centerAnchor.transform.position.y, -2);
	}
	
	public void deactivate(){
		gameObject.SetActive(false);
	}
	
	public void showTitle(){
		
		gameObject.SetActive(true);
		mainSprite.SetSprite("titleCard");
		showingTitle = true;
		mainSprite.gameObject.transform.position = new Vector3( centerAnchor.transform.position.x, centerAnchor.transform.position.y, -2);
	}
	
	void Update(){
	
		//when we're on the title, clicking should advance to instuctions
		if (showingTitle){
			if (Input.GetMouseButtonDown(0)){
				showingTitle = false;
				activate(true);
			}
		}
		
	}
	
	
	
	//setters getters
	
	
	public bool ShowingTitle {
		get {
			return this.showingTitle;
		}
		set {
			showingTitle = value;
		}
	}
	
}
