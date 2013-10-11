using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerGhost : Player {
	
	public int scoreValue;
	
	public float alphaVal;
	
	public float stunTime;
	public float startingStunTime;
	private float stunTimer;
	
	public void ghostSetup(Color oldColor, GhostRecorder record, List<Power> oldPowers, StarHelm _starHelm){
		
		//myColor = oldColor;
		//myColor.a = alphaVal;
		
		recorder = new GhostRecorder(record);
		
		powers = new List<Power>();
		
		starHelm = _starHelm;
		
		//set the powers to obey only me!
		for (int i=0; i<oldPowers.Count; i++){
			GameObject thisPower = Instantiate(oldPowers[i].gameObject, new Vector3(0,0,0), new Quaternion(0,0,0,0)) as GameObject;
			thisPower.GetComponent<Power>().assignToPlayer(this);
		}
		
		
	}
	
	public override void customStart(){
		isPlayerControlled = false;
		canPickupPowers = true;
		
		stunTimer = 0;
	}
	
	
	public override void customReset(){
		recorder.reset(false);
		recorder.play(false);
		
		Debug.Log("reset ghost on frame "+Time.frameCount);
		
		//set the pos
		transform.position = recorder.CurPos;
		
		//clear velocity
		curVel = recorder.CurVel;
		
		stunTimer = startingStunTime;
		
	}
	
	public override void customUpdate(){
		stunTimer -= Time.deltaTime;
		
		if (stunTimer <= 0){
			recorder.play(true);
		}
		
		transform.position = recorder.CurPos;
		curVel = recorder.CurVel;
		facingDir = recorder.CurFacingDir;
		
		bool attackPressed = recorder.checkAttack();
		if (attackPressed){
			for (int i=0; i<powers.Count; i++){
				powers[i].use();
			}
		}
		
		//if we hit the end, rewind
		if (recorder.isFinished()){
			recorder.reset(false);
		}
		
	}
	
	public override void killPlayerCustom(Player killer){
		//give it pause time
		stunTimer = stunTime;
		
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
	}
}
