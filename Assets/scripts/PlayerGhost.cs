using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerGhost : Player {
	
	public int scoreValue;
	
	public float alphaVal;
	
	public float stunTime;
	public float startingStunTime;
	public float startingStunTimeBonusRange;
	private float stunTimer;
	
	public float rewindSpeed;
	
	public void ghostSetup(Color oldColor, GhostRecorder record, List<Power> oldPowers, StarHelm _starHelm, GameManager _gm){
		
		//myColor = oldColor;
		//myColor.a = alphaVal;
		
		
		recorder = new GhostRecorder(record);
		
		powers = new List<Power>();
		
		starHelm = _starHelm;
		gm = _gm;
		
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
		
		bool attackPressed = recorder.checkAttack();
		if (attackPressed && recorder.PlaybackDir == 1){
			for (int i=0; i<powers.Count; i++){
				powers[i].use();
			}
		}
		
		//if we hit the end, rewind
		if (recorder.PlaybackDir == 1 && recorder.isAtEnd()){
			recorder.setPlaybackDir(-1);
			recorder.setPlaybackSpeed(rewindSpeed);
		}
		if (recorder.PlaybackDir == -1 && recorder.isAtStart()){
			recorder.setPlaybackDir(1);
			recorder.setPlaybackSpeed(1);
		}
		
	}
	
	public override void killPlayerCustom(Player killer){
		//show the text
		GameObject.FindGameObjectWithTag("statusText").SendMessage("showGhostKill");
		
		//if the killer was a player, give them a point
		if (killer!=null){
			if (killer.isPlayerControlled){
				killer.addScore(scoreValue);
			}
		}
		
		//reset the ghost
		reset();
		
		//give it pause time
		stunTimer = stunTime;
	}
}
