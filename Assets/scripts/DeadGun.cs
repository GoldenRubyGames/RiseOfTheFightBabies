using UnityEngine;
using System.Collections;

public class DeadGun : MonoBehaviour {
	
	public float flySpeed;
	private Vector3 vel;
	
	public float angleSpeed;
	private float curAngle;
	
	public float friction;
	
	public float angleRange;

	public void setup(float baseAngle){
		
		//set the vel based on the angle to the killer
		float angleToKiller = baseAngle + Random.Range(-angleRange, angleRange);
		vel = new Vector3(Mathf.Cos(angleToKiller) * flySpeed, Mathf.Sin(angleToKiller) * flySpeed,0);
		
		//cur angle is used to rotate the sprite
		curAngle = 0;
		if (vel.x > 0 ){
			angleSpeed *= -1;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += vel * Time.deltaTime;
		
		curAngle += angleSpeed * Time.deltaTime;
		transform.localEulerAngles = new Vector3(0,0, curAngle);
		
		//friction
		vel *= Mathf.Pow(friction, Time.deltaTime);
		angleSpeed *= Mathf.Pow(friction, Time.deltaTime);
		
	}
}
