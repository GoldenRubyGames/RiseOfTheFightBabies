using UnityEngine;
using System.Collections;

public class PauseScreen : MonoBehaviour {
	
	public bool debugSkipTitle;
	
	public tk2dSprite mainSprite;
	
	public GameObject centerAnchor;
	
	private bool showingTitle;
	
	public GameManager gm;
	
	void Awake(){
		showingTitle = false;
	}

	public void activate(bool showHowTo){
		gameObject.SetActive(true);
		
		//set the image base don if we're showing the title card and if there is a controller
		if (showHowTo){
			mainSprite.SetSprite(  (Input.GetJoystickNames().Length > 0 && !gm.forceKeyboardControlImages) ? "howToCardXbox" : "howToCard" );
		}else{
			mainSprite.SetSprite( (Input.GetJoystickNames().Length > 0  && !gm.forceKeyboardControlImages) ? "pauseBoxXbox" : "pauseBox" );
		}
		
		mainSprite.gameObject.transform.position = new Vector3( centerAnchor.transform.position.x, centerAnchor.transform.position.y, -4);
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
