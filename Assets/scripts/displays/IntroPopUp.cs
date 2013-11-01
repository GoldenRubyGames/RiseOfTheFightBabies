using UnityEngine;
using System.Collections;

public class IntroPopUp : MonoBehaviour {
	
	public tk2dTextMesh mainText;
	
	public GameObject crownSprite, cloneSprite, cloneKillSprite;
	
	public int minFramesOnScreen;  //usign frames because time scale is 0 when paused
	private int frameTimer;
	private bool canBeKilled;
	
	private GameManager gm;
	
	private bool isActive;
	
	private bool killOnDismissal;
	
	
	public void setup(GameManager _gm){
		gm = _gm;
		isActive = false;
		killOnDismissal = false;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		frameTimer--;
		if (frameTimer <=0){
			canBeKilled = true;
		}
		
		//do not let the game unpause
		if (!gm.Paused){
			gm.setPause(true, false);
			gm.pauseScreen.gameObject.SetActive(false);
		}
		
		//check for it being dismissed
		if (Input.anyKey && canBeKilled){
			gm.setPause(false,false);
			dismiss();
		}
	}
	
	public void setText(string newText){
		gameObject.SetActive(true);
		
		mainText.text = newText;
		mainText.Commit();
		
		frameTimer = minFramesOnScreen;
		canBeKilled = false;
		
		isActive = true;
		
		//assume everything is off
		crownSprite.SetActive(false);
		cloneSprite.SetActive(false);
		cloneKillSprite.SetActive(false);
	}
	
	public void dismiss(){
		isActive = false;
		gameObject.SetActive(false);
		
		if (killOnDismissal){
			Destroy(gameObject);
		}
	}
	
	
	public void showCloneKill(GameManager _gm){
		gm = _gm;
		setText("When you use the GHOST KILL, the ghosts you hit will be gone forever. But you only get one shot!\n\n");
		cloneKillSprite.SetActive(true);
		killOnDismissal = true;
	}
	
	
	public bool CanBeKilled {
		get {
			return this.canBeKilled;
		}
	}
	
	public GameManager Gm {
		get {
			return this.gm;
		}
		set {
			gm = value;
		}
	}
	
	public bool IsActive {
		get {
			return this.isActive;
		}
		set {
			isActive = value;
		}
	}
}
