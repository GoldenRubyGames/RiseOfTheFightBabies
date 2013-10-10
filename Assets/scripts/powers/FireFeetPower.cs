using UnityEngine;
using System.Collections;

public class FireFeetPower : Power {
	
	public float timeBetweenDrops;
	float timer;

	public override void customReset(){
		timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
		timer -= Time.deltaTime;
		
		if (timer <= 0){
			GameObject newFlame = Instantiate(effectObject, owner.transform.position, new Quaternion(0,0,0,0) ) as GameObject;
			newFlame.SendMessage("setup", owner);
			timer = timeBetweenDrops;
		}
	}
}
