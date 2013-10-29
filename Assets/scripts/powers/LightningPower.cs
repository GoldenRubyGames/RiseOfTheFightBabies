using UnityEngine;
using System.Collections;

public class LightningPower : Power {

	public Vector3 recoilForce;

	public override void customUse(){
		
		//spawn 2 lightnings 
		for (int i=0; i<2; i++){
			GameObject lightningObj = Instantiate( effectObject, owner.transform.position, new Quaternion(0,0,0,0)) as GameObject;
			lightningObj.GetComponent<LightningEffect>().setup(owner, i==0);
		}
		
		//recoil!
		owner.push( recoilForce );
		
	}
}
