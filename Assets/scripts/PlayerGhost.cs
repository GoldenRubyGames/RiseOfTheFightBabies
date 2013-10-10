using UnityEngine;
using System.Collections;

public class PlayerGhost : Player {
	
	public float alphaVal;
	
	public void ghostSetup(Color oldColor, GhostRecorder record){
		
		myColor = oldColor;
		myColor.a = alphaVal;
		
		recorder = new GhostRecorder(record);
		
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
		
	}
}
