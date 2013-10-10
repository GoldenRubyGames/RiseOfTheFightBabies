using UnityEngine;
using System.Collections;

public class BatPower : Power {
	
	
	
	public override void customReset(){
		
	}
	
	public override void customUse(){
		
		//spawn a bat effect
		GameObject thisObj = Instantiate( effectObject, owner.transform.position, new Quaternion(0,0,0,0)) as GameObject;
		thisObj.SendMessage("setup", owner);
		
	}
	
}
