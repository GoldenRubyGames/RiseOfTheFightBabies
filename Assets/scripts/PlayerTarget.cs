using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerTarget : Player {
	
	
	public override void customStart(){
		powers = new List<Power>();
	}
	
	
	public override void customReset(){
		Health = 1;
		//clear velocity
		pushVel = new Vector3(0,0,0);
		curVel = new Vector3(0,0,0);
		
		//reset movement info
		speed = baseSpeed;
		fallingGrav = fallingGravBase;
		numDoubleJumpsUsed = 0;
		
	}
	
	
	public override void customUpdate(){
		//Debug.Log("heal "+Health);
		curVel.y -= fallingGravBase*Time.deltaTime;
		//actually move this guy
		controller.Move(curVel*Time.deltaTime + pushVel*Time.deltaTime);
		
	}
	
	public override void killPlayerCustom(Player killer, bool cloneKiller){
		clearPowers();
		Destroy(gameObject);
	}
}
