using UnityEngine;
using System.Collections;

public class IntroManager : MonoBehaviour {
	
	GameManager gm;
	
	public PickupSpot  pickupSpot;
	
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
		
		phase = -1;
		curPowerNum = 0;
		advancePhase();
		
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
		Debug.Log("dead mother fucker");
		pickupSpot.activate(powerObjects[curPowerNum]);
		pickupSpot.Timer = 9999;
	}
	
	
	void checkPhase(){
		//Debug.Log("round "+gm.RoundNum);
		
		if (phase == 0 && timer > 3){
			advancePhase();
			return;
		}
		
		if (phase == 1 && timer > 10){
			advancePhase();
			return;
		}
		
		if (phase == 2 && timer > 4){
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
		
		if (phase == 5 && targetObject == null){
			advancePhase();
			return;
		}
		
		if (phase == 6 && timer > 3){
			advancePhase();
			return;
		}
		
		if (phase == 7 && timer > 9){
			advancePhase();
			return;
		}
		
		if (phase == 8 && !pickupSpot.IsActive){
			advancePhase();	
			return;
		}
		
		if (phase == 9 && timer > 7){
			advancePhase();
			return;
		}
		
		if (phase == 10 && timer > 5){
			advancePhase();
			return;
		}
		
		if (phase == 11 && timer > 3){
			advancePhase();
			return;
		}
		
		if (phase == 12 && gm.Goons.Count == 0){
			advancePhase();
			return;
		}
		
		if (phase == 13 && !popUp.IsActive){
			advancePhase();
			return;
		}
		
		if (phase == 14 && timer > 4){
			advancePhase();
			return;
		}
		
		if (phase == 15 && timer > 12){
			advancePhase();
			return;
		}
		
		if (phase == 16 && gm.RoundNum >= 3){
			advancePhase();
			return;
		}
		
		if (phase == 17 && timer > 3){
			advancePhase();
			return;
		}
		
		if (phase == 18 && gm.RoundNum >= 4){
			advancePhase();
			return;
		}
		
		if (phase == 19 && !popUp.IsActive && timer > 2){
			advancePhase();
			return;
		}
		
		if (phase == 20 && timer > 8){
			advancePhase();
			return;
		}
		
	}
	
	void advancePhase(){
		phase++;
		timer = 0;
		
		if (phase == 1){
			speakerText.setNewText("Thanks for coming in.\nWe've got a bit of an issue with\nthe clones.");
		}
		
		if (phase == 2){
			speakerText.setNewText("Better get you armed.\nWould you mind grabbing that\npower up?");
		}
		
		if (phase == 3){
			curPowerNum = 0;
			pickupSpot.activate(powerObjects[0]);
			pickupSpot.Timer = 9999;
		}
		
		if (phase == 4){
			speakerText.setNewText("Press Z to fire.");
		}
		
		if (phase == 5){
			speakerText.setNewText("Try taking down that target\nup there.");
			targetObject = Instantiate(targetPrefab, targetSpawnPoint.transform.position, new Quaternion(0,0,0,0)) as GameObject;
			targetObject.GetComponent<PlayerTarget>().customReset();
		}
		
		if (phase == 6){
			speakerText.setNewText("Nice.");
		}
		
		if (phase == 7){
			speakerText.setNewText("You seem like a strong person.\nI bet you can handle another\npower up.");
		}
		
		if (phase == 8){
			speakerText.setNewText("");
			curPowerNum = 1;
			pickupSpot.activate(powerObjects[1]);
			pickupSpot.Timer = 9999;
		}
		
		if (phase == 9){
			speakerText.setNewText("When you press Z they will both\nfire.");
		}
		
		if (phase == 10){
			speakerText.setNewText("Uh oh.\nOne of the robots got loose.");
		}
		
		if (phase == 11){
			gm.spawnGoon();
			gm.Goons[0].reset();
			gm.starHelm.setChosenOne(gm.Goons[0]);
		}
		
		if (phase == 12){
			speakerText.setNewText("Would you mind destroying it?");
		}
		
		if (phase == 13){
			//pop up about ending rounds
			setPopUp("Each round ends when\nyou kill the King\n(wearing the crown).\n\nThe King is worth\n10 points");
			popUp.crownSprite.SetActive(true);
		}
		
		if (phase == 14){
			//this is to give some pause when the new round starts.
			speakerText.setNewText("");
			//we also want to freeze the ghost
			gm.Ghosts[0].StunTimer = 999;
		}
		
		if (phase == 15){
			speakerText.setNewText("Good Work, but Here's the issue:\nThe facility just cloned you.\nCould you destroy that too?");
		}
		
		if (phase == 16){
			curPowerNum = 2;
			pickupSpot.activate(powerObjects[2]);
			pickupSpot.Timer = 9999;
			//actually start the next round
			gm.Ghosts[0].StunTimer = 0;
			gm.Ghosts[0].InvincibilityTimer = 0;
			speakerText.setNewText("");
		}
		
		if (phase == 17){
			//pop up about clones
			setPopUp("When a round ends,\na clone will be made\nthat does exactly\nwhat you did (with\nyour powers).\n\nThis new clone will\nbe the King.");
		}
		
		if (phase == 18){
			curPowerNum = 3;
			pickupSpot.activate(powerObjects[3]);
			pickupSpot.Timer = 9999;
			speakerText.setNewText("This is going to be a problem.");
		}
		
		if (phase == 19){
			speakerText.setNewText("");
			//pop up about clones
			setPopUp("Only killing the King\nwill end the round.\n\nKilling other clones\nis worth 1 point.\n");
			popUp.cloneSprite.SetActive(true);
			
			curPowerNum = 4;
			pickupSpot.activate(powerObjects[4]);
			pickupSpot.Timer = 9999;
		}
		
		if (phase == 20){
			speakerText.setNewText("You're on your own! Good luck!");
		}
		
		if (phase == 21){
			//we're done!
			gm.DoingIntro = false;
			Destroy(speakerText.gameObject);
			Destroy(popUp.gameObject);
			//Destroy(gameObject);
		}
	}
}
