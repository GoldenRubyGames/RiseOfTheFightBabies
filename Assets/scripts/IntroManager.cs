using UnityEngine;
using System.Collections;

public class IntroManager : MonoBehaviour {
	
	GameManager gm;
	
	public PickupSpot  pickupSpot, pickupSpot2;
	
	private int curPowerNum;
	public GameObject[] powerObjects;
	
	public GameObject targetSpawnPoint;
	public GameObject targetPrefab;
	private GameObject targetObject;
	
	public IntroSpeakerText speakerText;
	
	public GameObject popUpPrefab;
	private IntroPopUp popUp;
	
	private int phase;
	
	private float timer;
	
	// Use this for initialization
	void Start () {
		//find the game manager
		gm = GameObject.Find("gameManager").GetComponent<GameManager>();
		
		//instantiate the pickup
		GameObject popUpObj = Instantiate(popUpPrefab, new Vector3(0,0,-3), new Quaternion(0,0,0,0)) as GameObject;
		popUp = popUpObj.GetComponent<IntroPopUp>();
		popUp.setup(gm);
		popUpObj.SetActive(false);
		
		phase = 6;//-1;
		curPowerNum = 0;
		advancePhase();
		
		GameObject.Find("HUD").GetComponent<HUD>().hudBox.SetActive(false);
		
		
		//speakerText.setNewText("Hello. How are you today?\nI'm good!");
	
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		
		checkPhase();
	}
	
	void setPopUp(string text){
		popUp.setText(text);
		gm.setPause(true, false);
		gm.pauseScreen.gameObject.SetActive(false);
	}
	
	public void playerDied(){
		pickupSpot.activate(powerObjects[curPowerNum]);
		pickupSpot.Timer = 9999;
	}
	
	
	void checkPhase(){
		//Debug.Log("round "+gm.RoundNum);
		
		if (phase == 0 && timer > 3){
			advancePhase();
			return;
		}
		
		if (phase == 1 && timer > 8){
			advancePhase();
			return;
		}
		
		if (phase == 2 && timer > 3){
			advancePhase();
			return;
		}
	
		if (phase == 3 && !pickupSpot.IsActive){
			advancePhase();	
			return;
		}
		
		if (phase == 4 && Input.GetButtonDown("player0Fire1")){
			advancePhase();
			return;
		}
		
		if (phase == 5 && timer > 2){
			advancePhase();
			return;
		}
		
		if (phase == 6 && targetObject == null){
			advancePhase();
			return;
		}
		
		if (phase == 7 && timer > 3){
			advancePhase();
			return;
		}
		
		if (phase == 8 && timer > 4){
			advancePhase();
			return;
		}
		
		if (phase == 9 && !pickupSpot2.IsActive){
			advancePhase();	
			return;
		}
		
		if (phase == 10 && timer > 7){
			advancePhase();
			return;
		}
		
		if (phase == 11 && timer > 3){
			advancePhase();
			return;
		}
		
		if (phase == 12 && timer > 0){
			advancePhase();
			return;
		}
		
		if (phase == 13 && gm.Goons.Count == 0){
			advancePhase();
			return;
		}
		
		if (phase == 14 && !popUp.IsActive){
			advancePhase();
			return;
		}
		
		if (phase == 15 && timer > 1){
			advancePhase();
			return;
		}
		
		if (phase == 16 && timer > 8){
			advancePhase();
			return;
		}
		
		if (phase == 17 && gm.RoundNum >= 3){
			advancePhase();
			return;
		}
		
		if (phase == 18 && timer > 3){
			advancePhase();
			return;
		}
		
		if (phase == 19 && gm.RoundNum >= 4){
			advancePhase();
			return;
		}
		
		if (phase == 20 && !popUp.IsActive && timer > 2){
			advancePhase();
			return;
		}
		
		if (phase == 21 && timer > 6){
			advancePhase();
			return;
		}
		
	}
	
	void advancePhase(){
		phase++;
		timer = 0;
		
		if (phase == 1){
			speakerText.setNewText("Thanks for coming in.\nWe've got an issue with the clones.");
		}
		
		if (phase == 2){
			speakerText.setNewText("Would you mind grabbing that power up?");
		}
		
		if (phase == 3){
			curPowerNum = 0;
			pickupSpot.activate(powerObjects[0]);
			pickupSpot.Timer = 9999;
		}
		
		if (phase == 4){
			if (Input.GetJoystickNames().Length == 0){
				speakerText.setNewText("Press Z or M to fire.");
			}else{
				speakerText.setNewText("Press X to fire.");
			}
		}
		
		if (phase == 5){
			//just to pause before spawning the target
			speakerText.setNewText("");
		}
		
		if (phase == 6){
			speakerText.setNewText("Try taking down this target.");
			targetObject = Instantiate(targetPrefab, targetSpawnPoint.transform.position, new Quaternion(0,0,0,0)) as GameObject;
			targetObject.GetComponent<PlayerTarget>().customReset();
			targetObject.GetComponent<PlayerTarget>().AudioController = gm.audioController;
		}
		
		if (phase == 7){
			speakerText.setNewText("Nice.");
		}
		
		if (phase == 8){
			speakerText.setNewText("You seem strong.\nI bet you can handle another power up.");
		}
		
		if (phase == 9){
			//speakerText.setNewText("");
			curPowerNum = 1;
			pickupSpot2.activate(powerObjects[1]);
			pickupSpot2.Timer = 9999;
		}
		
		if (phase == 10){
			if (Input.GetJoystickNames().Length == 0){
				speakerText.setNewText("When you press Z or M they will both\nfire.");
			}else{
				speakerText.setNewText("When you press X they will both\nfire.");
			}
				
		}
		
		if (phase == 11){
			speakerText.setNewText("Uh oh.\nOne of the robots got loose.");
		}
		
		if (phase == 12){
			gm.spawnGoon();
			gm.Goons[0].reset();
			gm.starHelm.setChosenOne(gm.Goons[0]);
		}
		
		if (phase == 13){
			//THIS PAHSE WAS DUMB!
			//speakerText.setNewText("Would you mind destroying it?");
		}
		
		if (phase == 14){
			//pop up about ending rounds
			setPopUp("Each round ends when you kill the King\n(wearing the crown).\n\nThe King is worth 10 points");
			popUp.crownSprite.SetActive(true);
		}
		
		if (phase == 15){
			//this is to give some pause when the new round starts.
			speakerText.setNewText("");
			//we also want to freeze the ghost
			gm.Ghosts[0].StunTimer = 999;
		}
		
		if (phase == 16){
			speakerText.setNewText("Good Work, but Here's the issue:\nThe facility just cloned you.\nPlease destroy that too.");
		}
		
		if (phase == 17){
			curPowerNum = 2;
			pickupSpot.activate(powerObjects[2]);
			pickupSpot.Timer = 9999;
			//actually start the next round
			gm.Ghosts[0].StunTimer = 0;
			gm.Ghosts[0].InvincibilityTimer = 0;
			//speakerText.setNewText("");
		}
		
		if (phase == 18){
			speakerText.setNewText("");
			//pop up about clones
			setPopUp("When a round ends, a clone will be made that does exactly what you did (with your powers).\n\nThis new clone will be the King.");
		}
		
		if (phase == 19){
			curPowerNum = 3;
			pickupSpot.activate(powerObjects[3]);
			pickupSpot.Timer = 9999;
			speakerText.setNewText("This is going to be a problem.");
		}
		
		if (phase == 20){
			speakerText.setNewText("");
			//pop up about clones
			setPopUp("Only killing the King will end the round.\n\nKilling other clones is worth 1 point.\n");
			popUp.cloneSprite.SetActive(true);
			
			curPowerNum = 4;
			pickupSpot.activate(powerObjects[4]);
			pickupSpot.Timer = 9999;
		}
		
		if (phase == 21){
			speakerText.setNewText("You're on your own! Good luck!");
		}
		
		if (phase == 22){
			//we're done!
			gm.DoingIntro = false;
			GameObject.Find("HUD").GetComponent<HUD>().hudBox.SetActive(true);
			Destroy(speakerText.gameObject);
			Destroy(popUp.gameObject);
			//Destroy(gameObject);
		}
	}
}
