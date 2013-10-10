using UnityEngine;
using System.Collections;

public class PlayerController : Player {
	
	//getting input;
	public int controllerNum;
	string moveAxis, jumpButton, attack1Button, atack2Button; 
	
	//general moving
	Vector3 pushVel;
	public float pushFric;
	
	//moving left and right
	public float baseSpeed;
	
	//jumping
	public float jumpGrav, fallingGravBase;
	public float jumpPower;
	public float jumpCut;
	bool isJumping;
	
	int numDoubleJumpsUsed;
	
	
	public override void customStart(){
		isPlayerControlled = true;
		
		curVel = new Vector3(0,0,0);
		
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
		
		recorder = new GhostRecorder();
		
	}
	
	public override void customReset(){
		//set the pos
		transform.position = startPos;
		
		//clear velocity
		pushVel = new Vector3(0,0,0);
		curVel = new Vector3(0,0,0);
		
		//reset movement info
		speed = baseSpeed;
		fallingGrav = fallingGravBase;
		numDoubleJumpsUsed = 0;
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
		if (Input.GetButtonDown(attack1Button)){
			for (int i=0; i<powers.Count; i++){
				powers[i].use();
			}
		}
		
		//record for ghosts
		recorder.record(curVel, facingDir, transform.position);
		
		
		
		//testing
		if (Input.GetKeyDown(KeyCode.G) && controllerNum==0){
			makeGhost();
		}
	}
	
	
	void startJump(){
		isJumping = true;
		curVel.y = jumpPower;
	}
	
	void endJump(){
		if (curVel.y > jumpCut){
			curVel.y = jumpCut;
	    }
	    isJumping = false;
	}
	
	public override void push(Vector3 power){
		pushVel += power;
	}
	
	
	
	///setters and getters
	
	
	
	
	
	
}
