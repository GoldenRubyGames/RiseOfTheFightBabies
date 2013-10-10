using UnityEngine;
using System.Collections;

public class SpawnEffectPower : Power {
	
	public float recoilForce;

	public override void customUse(){
		
		//spawn a bullet effect
		GameObject thisObj = Instantiate( effectObject, owner.transform.position, new Quaternion(0,0,0,0)) as GameObject;
		thisObj.SendMessage("setup", owner);
		
		//recoil!
		owner.push( new Vector3(recoilForce*owner.FacingDir*-1, 0, 0) );
		
	}
}
