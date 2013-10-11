using UnityEngine;
using System.Collections;

public class MeleeStandardPower : Power {
	
	public float dashForce;
	
	private GameObject curObject;
	
	public override void customUse(){
		//spawn an effect
		curObject = Instantiate( effectObject, owner.transform.position, new Quaternion(0,0,0,0)) as GameObject;
		curObject.SendMessage("setup", owner);
		
		//push the user forward
		owner.push( new Vector3(dashForce*owner.facingDir, 0, 0) );
	}
	
	public override void customCleanUp (){
		Destroy(curObject);
	}
}
