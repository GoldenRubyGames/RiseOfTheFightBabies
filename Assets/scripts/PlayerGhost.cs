using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerGhost : Player {
	
	public float alphaVal;
	
	public void ghostSetup(Color oldColor, GhostRecorder record, List<Power> oldPowers){
		
		myColor = oldColor;
		myColor.a = alphaVal;
		
		recorder = new GhostRecorder(record);
		
		powers = new List<Power>();
		
		//set the powers to obey only me!
		for (int i=0; i<oldPowers.Count; i++){
			GameObject thisPower = Instantiate(oldPowers[i].gameObject, new Vector3(0,0,0), new Quaternion(0,0,0,0)) as GameObject;
			thisPower.GetComponent<Power>().assignToPlayer(this);
		}
		
		Debug.Log("new ghost on frame "+Time.frameCount);
		
	}
	
	public override void customStart(){
		isPlayerControlled = false;
	}
	
	
	public override void customReset(){
		recorder.play(false);
		
		
		//set the pos
		transform.position = recorder.CurPos;
		
		//clear velocity
		curVel = recorder.CurVel;
		
	}
	
	public override void customUpdate(){
		recorder.play(true);
		
		transform.position = recorder.CurPos;
		curVel = recorder.CurVel;
		facingDir = recorder.CurFacingDir;
		
		bool attackPressed = recorder.checkAttack();
		if (attackPressed){
			for (int i=0; i<powers.Count; i++){
				powers[i].use();
			}
		}
		
	}
}
