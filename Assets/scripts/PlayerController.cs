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
		
		powers = new List<Power>();
		
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
		
		recorder = new GhostRecorder();
		
		
	}
	
	public override void customReset(){
		
		
		//set the pos
		float spawnX = Random.Range(spawnLeft.transform.position.x, spawnRight.transform.position.x);
		transform.position = new Vector3(spawnX, spawnLeft.transform.position.y, 0);
		
		//clear velocity
		pushVel = new Vector3(0,0,0);
		curVel = new Vector3(0,0,0);
		
		//reset movement info
		speed = baseSpeed;
		fallingGrav = fallingGravBase;
		numDoubleJumpsUsed = 0;
		
		recorder.reset(true);
	}
	
	public override void customUpdate(){
		//running
		float curSpeed = speed * Input.GetAxis(moveAxis);
		curVel.x = curSpeed;
		
		if (curSpeed > 0)  facingDir = 1;
		if (curSpeed < 0)  facingDir = -1;
		
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
		/*
		if (Input.GetKeyDown(KeyCode.G) && controllerNum==0){
			makeGhost();
		}
		*/
	}
	
	
	public override void killPlayerCustom(Player killer){
		//give the killer a point if they are real. Do not reward suicide
		if (killer != null && killer != this){
			killer.addScore(1);
		}
		
		//spawn a spooky ghost
		if (isPlayerControlled){
			makeGhost();
		}
		
		//reset the player
		reset();
		
		//empty out the powers
		clearPowers();
		//give them a punch
		GameObject powerObject = Instantiate(punchPowerObject, new Vector3(0,0,0), new Quaternion(0,0,0,0) ) as GameObject;
		Power thisPower = powerObject.GetComponent<Power>();
		thisPower.assignToPlayer(this);
		
		livesLeft--;
		if (livesLeft <= 0){
			gm.endGame(Score);
		}
		else{
			//show the text
			GameObject.FindGameObjectWithTag("statusText").SendMessage("showDeathText", livesLeft);
		}
	}
	
	
	
}
