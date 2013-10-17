using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : Player {
	
	//getting input;
	public int controllerNum;
	string moveAxis, jumpButton, attack1Button, atack2Button; 
	
	
	public int numLives;
	private int livesLeft;
	
	public override void customStart(){
		
		isPlayerControlled = true;
		
		curVel = new Vector3(0,0,0);
		
		clearPowers();
		//give them a punch
		GameObject powerObject = Instantiate(punchPowerObject, new Vector3(0,0,0), new Quaternion(0,0,0,0) ) as GameObject;
		Power thisPower = powerObject.GetComponent<Power>();
		thisPower.assignToPlayer(this);
		
		//set the controller
		moveAxis = "player0Move";
		jumpButton = "player0Jump";    //X
		attack1Button = "player0Fire1"; //square
		atack2Button = "player0Fire2";  //triangle
		
		if (controllerNum == 1){
			moveAxis = "player1Move";
			jumpButton = "player1Jump";    //X
			attack1Button = "player1Fire1"; //square
			atack2Button = "player1Fire2";  //triangle
		}
		
		livesLeft = numLives;
		
		if (recorder == null){
			recorder = new GhostRecorder();
		}
		
	}
	
	public override void customReset(){
		
		//set the pos
		float spawnX = Random.Range(spawnLeft.transform.position.x, spawnRight.transform.position.x);
		transform.position = new Vector3(spawnX, spawnLeft.transform.position.y, 0);
		
		//clear velocity
		pushVel = new Vector3(0,0,0);
		curVel = new Vector3(0,0,0);
		
		clearPowers();
		//give them a punch
		GameObject powerObject = Instantiate(punchPowerObject, new Vector3(0,0,0), new Quaternion(0,0,0,0) ) as GameObject;
		Power thisPower = powerObject.GetComponent<Power>();
		thisPower.assignToPlayer(this);
		
		if (recorder == null){
			recorder = new GhostRecorder();
		}
		recorder.reset(true);
	}
	
	public override void customUpdate(){
		float xAxisJoy = Input.GetAxis(moveAxis);
		//assume we'r eusing this value
		float xAxisVal = xAxisJoy;
		//player 0 can be keyboard control
		if (controllerNum == 0){
			//get the keyboard input
			float xAxisKey = Input.GetAxis("player0KeyboardMove");
			
			//use whichever one is further form 0
			if ( Mathf.Abs(xAxisJoy) > Mathf.Abs(xAxisKey) ){
				xAxisVal = xAxisJoy;
			}else{
				xAxisVal = xAxisKey;
			}
		}
		
		
		//running
		float curSpeed = speed * xAxisVal;
		curVel.x = curSpeed;
		
		
		float facingThreshold = 0.25f;
		if (xAxisVal > facingThreshold)  facingDir = 1;
		if (xAxisVal < -facingThreshold)  facingDir = -1;
		
		//jumping
		if (Input.GetButtonDown(jumpButton) && (controller.isGrounded || numDoubleJumpsUsed < numDoubleJumps)){
			startJump();
			if (!controller.isGrounded){
				numDoubleJumpsUsed++;
			}
		}
		if (Input.GetButtonUp(jumpButton)){
			endJump();
		}
		
		//gravity
		if (!controller.isGrounded){
			if (curVel.y>0){
				curVel.y -= jumpGrav*Time.deltaTime;
			}else{
				curVel.y -= fallingGrav*Time.deltaTime;	
				isJumping = false;
			}
		}else if (!isJumping){
			curVel.y = -1; //need to keep pushing down for it to register as grounded so we can jump
		}
		
		if (controller.isGrounded){
			numDoubleJumpsUsed = 0;
		}
		
		//friciton if they were pushed
		pushVel *= Mathf.Pow(pushFric, Time.deltaTime);
		
		//actually move this guy
		controller.Move(curVel*Time.deltaTime + pushVel*Time.deltaTime);
		
		//using powers
		bool attackPressed = false;
		if (Input.GetButtonDown(attack1Button)){
			attackPressed = true;
			for (int i=0; i<powers.Count; i++){
				powers[i].use();
			}
		}
		
		//record for ghosts
		recorder.record(curVel, facingDir, transform.position, attackPressed);
		
		
		//testing
		
		if (Input.GetKeyDown(KeyCode.K) && controllerNum==0){
			changeHealth(-1, null);
		}
		
	}
	
	
	public override void killPlayerCustom(Player killer){
		if(!gm.DoingKillEffect){
			gm.startKillEffect(this, killer);
			//show the text
			GameObject.FindGameObjectWithTag("statusText").SendMessage("showDeathText", livesLeft-1);
			return;
		}
		
		//give the killer a point if they are real. Do not reward suicide
		if (killer != null && killer != this){
			killer.addScore(1);
		}
		
		//spawn a spooky ghost
		/*
		if (isPlayerControlled){
			makeGhost();
		}
		*/
		
		//reset the player
		reset();
		
		//empty out the powers
		clearPowers();
		//give them a punch
		GameObject powerObject = Instantiate(punchPowerObject, new Vector3(0,0,0), new Quaternion(0,0,0,0) ) as GameObject;
		Power thisPower = powerObject.GetComponent<Power>();
		thisPower.assignToPlayer(this);
		
		//destroy all effect objects
		GameObject[] effects = GameObject.FindGameObjectsWithTag("powerEffect");
		for (int i=0; i<effects.Length; i++){
			Destroy( effects[i] );
		}
		
		livesLeft--;
		if (livesLeft <= 0){
			gm.endGame(Score);
		}
	}
	
	
	
	
	
	//setters getters
	
	public int LivesLeft {
		get {
			return this.livesLeft;
		}
		set {
			livesLeft = value;
		}
	}
	
	
	
}
