using UnityEngine;
using System.Collections;

public class SpawnEffectPower : Power {
	
	public Vector3 recoilForce;
	
	public bool requirePlayerToBeInAir;

	public override void customUse(){
		
		if (!owner.controller.isGrounded || !requirePlayerToBeInAir){
			//spawn an effect
			GameObject thisObj = Instantiate( effectObject, owner.transform.position, new Quaternion(0,0,0,0)) as GameObject;
			thisObj.GetComponent<PowerEffect>().setup(owner);
			
			Debug.Log("do it dog shit");
			
			//recoil!
			owner.push( new Vector3(recoilForce.x*owner.facingDir*-1, recoilForce.y, 0) );
		}
		
	}
}
