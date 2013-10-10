using UnityEngine;
using System.Collections;

public class PunchPower : Power {
	
	public float dashForce;

	
	public override void customReset(){
		
	}
	
	public override void customUse(){
		
		//spawn a punch effect
		GameObject thisObj = Instantiate( effectObject, owner.transform.position, new Quaternion(0,0,0,0)) as GameObject;
		thisObj.SendMessage("setup", owner);
		
		//push the user forward
		owner.push( new Vector3(dashForce*owner.facingDir, 0, 0) );
	}
}
