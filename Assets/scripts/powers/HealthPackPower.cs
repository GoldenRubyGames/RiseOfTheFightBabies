using UnityEngine;
using System.Collections;

public class HealthPackPower : Power {
	
	GameObject spikeObject;
	public float gravIncrease;
	
	public override void customReset(){
		owner.Health ++;
	}
}
