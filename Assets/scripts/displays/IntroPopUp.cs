using UnityEngine;
using System.Collections;

public class IntroPopUp : MonoBehaviour {
	
	public tk2dTextMesh mainText;
	
	public GameObject crownSprite, cloneSprite;
	
	public int minFramesOnScreen;  //usign frames because time scale is 0 when paused
	private int frameTimer;
	private bool canBeKilled;
	
	private GameManager gm;
	
	private bool isActive;
	
	
	public void setup(GameManager _gm){
		gm = _gm;
		isActive = false;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		frameTimer--;
		if (frameTimer <=0){
			canBeKilled = true;
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
	}
	
	public void dismiss(){
		isActive = false;
		gameObject.SetActive(false);
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
