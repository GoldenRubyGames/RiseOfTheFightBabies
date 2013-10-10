using UnityEngine;
using System.Collections;

public class EffectSpawnOnPickupPower : Power {
	
	GameObject spawnedObject;

	public override void customReset(){
		
		spawnedObject = Instantiate( effectObject, owner.transform.position, new Quaternion(0,0,0,0)) as GameObject;
		spawnedObject.SendMessage("setup", owner);
		
		Debug.Log("spawn my ugo dream");
		
	}
	
	public override void customCleanUp(){
		Debug.Log("I took a vicous shit");
		Destroy(spawnedObject);
	}
	
}
