using UnityEngine;
using System.Collections;

public class UnlockPopUp : MonoBehaviour {
	
	public tk2dTextMesh topText, middleText, bottomText;
	
	public float blinkTime;
	
	private GameManager gm;
	
	public float minTimeOnScreen;
	private float timer;
	private bool canBeKilled;

	public void setup(string unlockName, bool isWeapon, int nextUnlockVal, bool noMoreUnlocks, GameManager _gm){
		gm = _gm;
		
		//setup our text
		string unlockType = isWeapon ? "POWER" : "LEVEL";
		topText.text = "NEW "+unlockType+" UNLOCKED!";
		topText.Commit();
		
		middleText.text = unlockName;
		middleText.Commit();
		
		if (!noMoreUnlocks){
			bottomText.text = "NEXT UNLOCK AT "+nextUnlockVal.ToString()+" KILLS";
		}else{
			bottomText.text = "NO MORE UNLOCKS. YOU DID IT!";
		}
		if (!isWeapon){
			bottomText.text = "";
		}
		bottomText.Commit();
		
		gm.setUnlockPopUpShowing(this);
		
		timer = minTimeOnScreen;
		canBeKilled = false;
		
	}
	
	// Update is called once per frame
	void Update () {
	
		middleText.renderer.enabled = Time.time%blinkTime < blinkTime/2;
		
		timer -= Time.deltaTime;
		if (timer <= 0){
			canBeKilled = true;
		}
	}
	
	
	public bool CanBeKilled {
		get {
			return this.canBeKilled;
		}
	}
}
