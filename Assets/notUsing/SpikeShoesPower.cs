using UnityEngine;
using System.Collections;

public class SpikeShoesPower : Power {
	
	GameObject spikeObject;
	public float gravIncrease;
	
	public override void customReset(){
		owner.fallingGrav *= gravIncrease;//1.2f; //fall just a little faster
		
		//instantiate the spikes
		spikeObject = Instantiate(effectObject, new Vector3(0,0,0), new Quaternion(0,0,0,0)) as GameObject;
		spikeObject.SendMessage("setup", owner);
	}
	
	public override void customCleanUp(){
		Destroy(spikeObject);
	}
	
}
