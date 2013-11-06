using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : Player {
	
	//getting input;
	public int controllerNum;
	string moveAxis, jumpButton, attack1Button, atack2Button; 
	
	public float airMoveLerp, airMoveLerpSlow;
	public float groundPush;
	
	public float headBumpJumpLoss;
	
	public float timeBeforeNotGrounded;
	private float notGroundedTimer;
	private bool isGrounded;
	
	public bool neverBeenGrounded;
	
	public int numLives;
	private int livesLeft;
	
	//spawning in different places
	private float lastSpawnX;
	public float minSpawnDistance;
	private List<float> spawnAvoidPosXs = new List<float>();
	public float spawnAvoidPosDist;
	
	//using clone kills
	bool nextAttackIsCloneKill;
	public tk2dSprite cloneKillGlow;
	public HUD hud;
	public GameObject cloneKilLExplosionPrefab;
	
	public AudioClip cloneKillFireSound, cloneKillChargeSound;
	
	//showing the tutorial info for clone kill
	public bool showCloneKillPopUp;
	public GameObject cloneKillPopUpPrefab;
	
	//rewidning at the end of a round
	private bool doingRewind;
	private tk2dSpriteAnimationClip cloneRewindClip;
	
	private float rewindTimer;
	public float rewindTimeBeforeChange,rewindTimeBeforeMoving, rewindFlashTime;
	public float rewindFlashSpeed;
	private bool rewindMadeChange;
	
	public GameObject jumpPopPrefab;
	
	public override void customStart(){
		
		isPlayerControlled = true;
		
		curVel = new Vector3(0,0,0);
		
		clearPowers();
		//give them a punch
		if (startingPowerObject != null){
			GameObject powerObject = Instantiate(startingPowerObject, new Vector3(0,0,0), new Quaternion(0,0,0,0) ) as GameObject;
			Power thisPower = powerObject.GetComponent<Power>();
			thisPower.assignToPlayer(this);
		}
		
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
		
		//livesLeft = numLives;
		
		if (recorder == null){
			recorder = new GhostRecorder();
		}
		
		doingRewind = false;
		cloneRewindClip = avatarAnimation.GetClipByName("clone");
		
		lastSpawnX = -1000;//we don't want this to mess with the first spawn
	}
	
	public override void customReset(){
		//set the pos
		float spawnX = lastSpawnX;
		bool newPosIsBad = true;
		while ( newPosIsBad ){
			spawnX = Random.Range(spawnLeft.transform.position.x, spawnRight.transform.position.x);
			newPosIsBad = false; //assume this will work
			
			//if it is too close to the last one, it is bad
			if (Mathf.Abs( spawnX - lastSpawnX) < minSpawnDistance){
				newPosIsBad = true;
			}
			
			//if it is in a place we're trying to avoid, it is bad
			for (int i=0; i<spawnAvoidPosXs.Count; i++){
				if( Mathf.Abs( spawnX - spawnAvoidPosXs[i]) < spawnAvoidPosDist ){
					newPosIsBad = true;
					Debug.Log("you're a dumb cunt");
				}
			}
			
		}
		lastSpawnX = spawnX;
		transform.position = new Vector3(spawnX, spawnLeft.transform.position.y, 0);
		
		//clear velocity
		pushVel = new Vector3(0,0,0);
		curVel = new Vector3(0,0,0);
		
		clearPowers();
		//give them a punch
		if (startingPowerObject != null){
			GameObject powerObject = Instantiate(startingPowerObject, new Vector3(0,0,0), new Quaternion(0,0,0,0) ) as GameObject;
			Power thisPower = powerObject.GetComponent<Power>();
			thisPower.assignToPlayer(this);
		}
		
		if (recorder == null){
			recorder = new GhostRecorder();
		}
		recorder.reset(true);
		
		doingRewind = false;
				
		isGrounded = false;
		neverBeenGrounded = true;
		notGroundedTimer = timeBeforeNotGrounded;
		
		//make sure they are not doing clone kill
		if (nextAttackIsCloneKill){
			hud.removeCloneKillIcon();
			nextAttackIsCloneKill = false;
			cloneKillGlow.gameObject.SetActive(false);
		}
	}
	
	public override void customUpdate(){
		//if they are rewidnignafter a round, just do that
		if (doingRewind){
			rewindTimer += Time.deltaTime;
			
			if (!rewindMadeChange && rewindTimer >= rewindTimeBeforeChange){
				OverrideAnimation = true;
				avatarAnimation.Play(cloneRewindClip);
				rewindTimer = 0;
				rewindMadeChange = true;
			}
			
			if (rewindMadeChange){
				if(rewindTimer < rewindFlashTime){
					if (rewindTimer%rewindFlashSpeed > rewindFlashSpeed/2){
						avatarAnimation.Play(cloneRewindClip);
					}else{
						avatarAnimation.Play(AnimStandingClip);
					}
				}else{
					avatarAnimation.Play(cloneRewindClip);
				}
			}
			
			if (rewindMadeChange && rewindTimer >= rewindTimeBeforeMoving){
				recorder.play(true);
				
				transform.position = recorder.CurPos;
				curVel = recorder.CurVel;
				facingDir = recorder.CurFacingDir;
				
				//when we hit the start, end it
				if (recorder.isAtStart()){
					endRewind();
				}
			}
			
			return;
		}
		
		//figure out if we count as grounded
		if (!controller.isGrounded){
			notGroundedTimer += Time.deltaTime;
		}else{
			notGroundedTimer = 0;
		}
		isGrounded = (notGroundedTimer < timeBeforeNotGrounded);
		
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
		//on gournd just do it, in the air be floaty
		if (isGrounded){
			curVel.x = curSpeed;
		}else{
			float lerpSpeed = airMoveLerp;
			if ( Mathf.Abs(curSpeed) < Mathf.Abs(curVel.x) ){
				lerpSpeed = airMoveLerpSlow;
			}
			curVel.x = Mathf.Lerp(curVel.x, curSpeed, lerpSpeed);
		}
		
		float facingThreshold = 0.25f;
		if (xAxisVal > facingThreshold)  facingDir = 1;
		if (xAxisVal < -facingThreshold)  facingDir = -1;
		
		//jumping
		if (Input.GetButtonDown(jumpButton) && (isGrounded || numDoubleJumpsUsed < numDoubleJumps)){
			startJump();
			if (!isGrounded){
				numDoubleJumpsUsed++;
			}
			//drop a flash
			Instantiate(jumpPopPrefab, transform.position, new Quaternion(0,0,0,0));
		}
		if (Input.GetButtonUp(jumpButton)){
			endJump();
		}
		
		//bump your head, end the jump
		if ( (controller.collisionFlags & CollisionFlags.Above) != 0 ){
			curVel.y *= Mathf.Pow(headBumpJumpLoss, Time.deltaTime);
		}
		
		//gravity
		if (!controller.isGrounded){//using the character controller isGrounded val here because it gets weird otherwise
			if (curVel.y>0){
				curVel.y -= jumpGrav*Time.deltaTime;
			}else{
				curVel.y -= fallingGrav*Time.deltaTime;	
				isJumping = false;
			}
		}else if (!isJumping){
			curVel.y = -groundPush; //need to keep pushing down for it to register as grounded so we can jump
		}
		
		if (isGrounded){
			numDoubleJumpsUsed = 0;
		}
		
		//friciton if they were pushed
		pushVel *= Mathf.Pow(pushFric, Time.deltaTime);
		
		//actually move this guy
		controller.Move(curVel*Time.deltaTime + pushVel*Time.deltaTime);
		
		//using powers
		bool attackPressed = false;
		if (Input.GetButtonDown(attack1Button) && !gm.Paused){
			attackPressed = true;
			
			//if they have a clone kill, use that
			if (nextAttackIsCloneKill){
				nextAttackIsCloneKill = false;
				cloneKillGlow.gameObject.SetActive(false);
				hud.removeCloneKillIcon();
				//spawn the explosion
				GameObject newExplosion = Instantiate(cloneKilLExplosionPrefab, transform.position, new Quaternion(0,0,0,0)) as GameObject;
				newExplosion.GetComponent<Explosion>().setOwner(this);
				//make a sound
				AudioController.Play(cloneKillFireSound);
			}
			//otherwise do normal attack
			else{
				for (int i=0; i<powers.Count; i++){
					bool used = powers[i].use();
					if (used && powers[i].showGun){
						gunSprite.fire();
					}
				}
			}
		}
		
		//record for ghosts
		recorder.record(curVel, facingDir, transform.position, attackPressed);
		
		if (isGrounded && neverBeenGrounded){
			neverBeenGrounded = false;
			recorder.markGrounded();
		}
		
		
		//if the clone kill; glow is active, make sure it's facing the right way
		if (nextAttackIsCloneKill){
			cloneKillGlow.FlipX = facingDir==-1;
		}
		
		//testing
		/*
		if (Input.GetKeyDown(KeyCode.K) && controllerNum==0){
			killPlayer(null, false);
		}
		if (Input.GetKeyDown(KeyCode.T) && controllerNum==0){
			customReset();
		}
		*/
		
	}
	
	
	public override void killPlayerCustom(Player killer, bool cloneKiller){
		if(!gm.DoingKillEffect){
			gm.startKillEffect(this, killer, false);
			//show the text
			GameObject.FindGameObjectWithTag("statusText").SendMessage("showDeathText", livesLeft-1);
			return;
		}
		
		//give the killer a point if they are real. Do not reward suicide
		if (killer != null && killer != this){
			killer.addScore(1);
		}
		
		//reset the player
		reset();
		
		//empty out the powers
		clearPowers();
		//give them a punch
		if (startingPowerObject != null){
			GameObject powerObject = Instantiate(startingPowerObject, new Vector3(0,0,0), new Quaternion(0,0,0,0) ) as GameObject;
			Power thisPower = powerObject.GetComponent<Power>();
			thisPower.assignToPlayer(this);
		}
		
		//destroy all effect objects
		GameObject[] effects = GameObject.FindGameObjectsWithTag("powerEffect");
		for (int i=0; i<effects.Length; i++){
			Destroy( effects[i] );
		}
		
		if (!gm.DoingIntro){
			livesLeft--;
			if (livesLeft <= 0){
				gm.endGame(Score);
			}
		}
	}
	
	
	public override void activateCloneKill(){
		nextAttackIsCloneKill = true;
		cloneKillGlow.gameObject.SetActive(true);
		
		//if this is the first one, show the pop up
		if (showCloneKillPopUp){
			showCloneKillPopUp = false;
			
			GameObject popUpObj = Instantiate(cloneKillPopUpPrefab, new Vector3(0,0,-2.5f), new Quaternion(0,0,0,0)) as GameObject;
			popUpObj.GetComponent<IntroPopUp>().showCloneKill(gm);
			
			gm.setPause(true, false);
			gm.pauseScreen.gameObject.SetActive(false);
		}
		
		AudioController.Play(cloneKillChargeSound);
	}
	
	
	public void startRewind(){
		doingRewind = true;
		recorder.PlayHead = recorder.Data.Count-1;
		//recorder.GroundedFrame = 0;
		recorder.setPlaybackDir(-1);
		recorder.setPlaybackSpeed(7000);
		
		rewindMadeChange = false;
		rewindTimer = 0;
	}
	
	public void endRewind(){
		recorder.setPlaybackDir(1);
		recorder.setPlaybackSpeed(1);
		
		OverrideAnimation = false;
		
		doingRewind = false;
		
		gm.endRewind();
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
	
	public bool NextAttackIsCloneKill {
		get {
			return this.nextAttackIsCloneKill;
		}
		set {
			nextAttackIsCloneKill = value;
		}
	}
	
	public List<float> SpawnAvoidPosXs {
		get {
			return this.spawnAvoidPosXs;
		}
		set {
			spawnAvoidPosXs = value;
		}
	}
}
