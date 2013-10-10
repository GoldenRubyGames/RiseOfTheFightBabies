using UnityEngine;
using System.Collections;

public class BatPower : Power {
	
	GameObject curBatObject;
	
	public override void customReset(){
		
	}
	
	public override void customUse(){
		
		//spawn a bat effect
		curBatObject = Instantiate( effectObject, owner.transform.position, new Quaternion(0,0,0,0)) as GameObject;
		curBatObject.SendMessage("setup", owner);
		
	}
	
	public override void customCleanUp (){
		Destroy(curBatObject);
	}
	
}
