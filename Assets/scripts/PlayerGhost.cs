using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerGhost : Player {
	
	public int scoreValue;
	
	public float alphaVal;
	
	public float startingStunTime;
	public float startingStunTimeBonusRange;
	private float stunTimer;
	
	public float rewindSpeed;
	
	//returning after melting
	public float returnTime, pauseBeforeReturning;
	private float returnTimer;
	private Vector3 deathPos;
	private bool gunSpriteWasOn;
	
	public float returningNoiseDist, returningNoiseSpeed;
	
	
	
	public void ghostSetup(Color oldColor, GhostRecorder record, List<Power> oldPowers, StarHelm _starHelm, GameManager _gm, AudioManager _audioController){
		
		isGhost = true;
		
		recorder = new GhostRecorder(record);
		
		starHelm = _starHelm;
		gm = _gm;
		AudioController = _audioController;
		
		//set the powers to obey only me!
		for (int i=0; i<oldPowers.Count; i++){
			GameObject thisPower = Instantiate(oldPowers[i].gameObject, new Vector3(0,0,0), new Quaternion(0,0,0,0)) as GameObject;
			thisPower.GetComponent<Power>().assignToPlayer(this);
			
		}
		
	}
	
	public override void customStart(){
		isGhost = true;
		stunTimer = 0;
	}
	
	
	public override void customReset(){
		gameObject.SetActive(true);
		
		recorder.reset(false);
		recorder.play(false);
		
		//Debug.Log("reset ghost on frame "+Time.frameCount);
		
		//set the pos
		transform.position = recorder.CurPos;
		
		//clear velocity
		curVel = recorder.CurVel;
		
		stunTimer = startingStunTime + Random.Range(0,startingStunTimeBonusRange);
		
		
	}
	
	public override void customUpdate(){
		stunTimer -= Time.deltaTime;
		
		InvincibilityTimer = stunTimer;
		
		if (!IsGhostMelting){
		
			//if this thing has nothing in the recording, destroy it
			if (recorder.Data.Count == 0){
				Debug.Log("GHOST HAS NO RECORDING");
				Destroy(gameObject);
			}
			
			if (stunTimer <= 0){
				recorder.play(true);
			}
			
			transform.position = recorder.CurPos;
			curVel = recorder.CurVel;
			facingDir = recorder.CurFacingDir;
			
			if (recorder.PlaybackDir == -1){
				facingDir *= -1;
			}
			
			bool attackPressed = recorder.checkAttack();
			//if (attackPressed && recorder.PlaybackDir == 1){
			if (attackPressed){
				for (int i=0; i<powers.Count; i++){
					powers[i].use();
				}
				gunSprite.fire();
			}
			
			//if we hit the end, rewind
			if (recorder.PlaybackDir == 1 && recorder.isAtEnd()){
				/*
				avatarAnimation.Play("melting");
				IsGhostMelting = true;
				returnTimer = -pauseBeforeReturning;
				deathPos = transform.position;
				recorder.reset(false);
				recorder.play(false);
				gunSpriteWasOn = gunSprite.gameObject.active;
				gunSprite.gameObject.SetActive(false);
				*/
				recorder.setPlaybackDir(-1);
				//recorder.setPlaybackSpeed(rewindSpeed);
			}
			
			if (recorder.PlaybackDir == -1 && recorder.isAtStart()){
				recorder.setPlaybackDir(1);
				recorder.setPlaybackSpeed(1);
			}
			
			
		}
		
		if (!avatarAnimation.Playing && IsGhostMelting){
			returnTimer += Time.deltaTime;
			if (returnTimer >= 0){
				InvincibilityTimer = 0.1f;
				float prc = returnTimer/returnTime;
				//set the pos
				transform.position = Vector3.Lerp(deathPos, recorder.CurPos, prc);
				transform.position += new Vector3( Mathf.PerlinNoise( Time.time*returningNoiseSpeed, 0) * returningNoiseDist, Mathf.PerlinNoise( Time.time*returningNoiseSpeed, 100) * returningNoiseDist, 0);
				if (prc >= 1){
					IsGhostMelting = false;
					gunSprite.gameObject.SetActive(gunSpriteWasOn);
				}
			}
		}
		
	}
	
	public override void killPlayerCustom(Player killer, bool cloneKiller){
		//show the text
		GameObject.FindGameObjectWithTag("statusText").SendMessage("showGhostKill", cloneKiller);
		
		//if the killer was a player, give them a point
		if (killer!=null){
			if (killer.isPlayerControlled){
				killer.addScore(scoreValue);
			}
		}
		
		//get rid of any attacks that need to go away
		for (int i=0; i<powers.Count; i++){
			if (powers[i].destroyOnDeath){
				powers[i].customCleanUp();
			}
		}
		
		//store the kill
		gm.addKill();
		//gm.dataHolder.addCloneKill();
		
		//move the ghost away and turn them off until the next round
		transform.position = new Vector3(0, -100, 0); //move it off screen
		gameObject.SetActive(false);
		
		//if this was a clone killer, remove this ghost forever
		if (cloneKiller){
			gm.removeGhost(this);
		}
		
	}
	
	
	
	
	public float StunTimer {
		get {
			return this.stunTimer;
		}
		set {
			stunTimer = value;
		}
	}
}
