using UnityEngine;
using System.Collections;

public class EffectSpawnOnPickupPower : Power {
	
	GameObject spawnedObject;

	public override void customReset(){
		
		spawnedObject = Instantiate( effectObject, owner.transform.position, new Quaternion(0,0,0,0)) as GameObject;
		spawnedObject.SendMessage("setup", owner);
		
	}
	
	public override void customCleanUp(){
		Destroy(spawnedObject);
	}
	
}
