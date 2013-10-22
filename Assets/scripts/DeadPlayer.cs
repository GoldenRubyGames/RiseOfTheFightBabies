using UnityEngine;
using System.Collections;

public class DeadPlayer : MonoBehaviour {
	
	public tk2dSprite sprite;
	public tk2dSpriteAnimator anim;
	
	public float deathTime;
	private float deathTimer;
	
	public float flySpeed;
	private Vector3 vel;
	
	public float angleSpeed;
	private float curAngle;
	public float startAngle;
	
	public float friction;
	
	//delaing with the gun if they had one
	public DeadGun gun;
	
	public void setup(tk2dSpriteAnimation animationLibrary, Player killer, bool showGun){
		anim.Library = animationLibrary;
		
		deathTimer = deathTime;
		
		//set the vel based on the angle to the killer
		float angleToKiller = 0;
		if (killer != null){
			angleToKiller = Mathf.Atan2( transform.position.y-killer.transform.position.y, transform.position.x-killer.transform.position.x);
		}
		
		vel = new Vector3(Mathf.Cos(angleToKiller) * flySpeed, Mathf.Sin(angleToKiller) * flySpeed,0);
		
		//cur angle is used to rotate the sprite
		curAngle = startAngle;
		if (vel.x > 0 ){
			angleSpeed *= -1;
			curAngle = -startAngle;
		}
		
		anim.Play("dying");
		
		if (showGun){
			gun.gameObject.SetActive(true);
			gun.setup(angleToKiller);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		deathTimer-= Time.deltaTime;
		
		transform.position += vel * Time.deltaTime;
		
		curAngle += angleSpeed * Time.deltaTime;
		transform.localEulerAngles = new Vector3(0,0, curAngle);
		
		//friction
		vel *= Mathf.Pow(friction, Time.deltaTime);
		angleSpeed *= Mathf.Pow(friction, Time.deltaTime);
		
		if (deathTimer<=0){
			Destroy(gameObject);
		}
	}
}
