using UnityEngine;
using System.Collections;

public class GrenadePower : Power {

	public override void customUse(){
		
		//spawn a bullet effect
		GameObject thisObj = Instantiate( effectObject, owner.transform.position, new Quaternion(0,0,0,0)) as GameObject;
		thisObj.GetComponent<PowerEffect>().setup(owner);
		
	}
}
