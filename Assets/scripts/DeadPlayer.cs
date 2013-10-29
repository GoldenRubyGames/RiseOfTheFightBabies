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
	
	//when clones are banished, it needs ot look different
	private bool cloneKiller;
	public float cloneKillGrow;
	
	//delaing with the gun if they had one
	public DeadGun gun;
	
	public void setup(tk2dSpriteAnimation animationLibrary, Player killer, bool showGun, bool _cloneKiller){
		anim.Library = animationLibrary;
		
		cloneKiller = _cloneKiller;
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
		
		if (!cloneKiller){
			anim.Play("dying");
		}else{
			anim.Play("disintegrating");
		}
		
		if (showGun){
			gun.gameObject.SetActive(true);
			gun.setup(angleToKiller);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		deathTimer-= Time.deltaTime;
		
		transform.position += vel * Time.deltaTime;
		
		if (!cloneKiller){
			curAngle += angleSpeed * Time.deltaTime;
			transform.localEulerAngles = new Vector3(0,0, curAngle);
		}else{
			transform.localScale += new Vector3(1,1,1) * cloneKillGrow * Time.deltaTime;
		}
		
		//friction
		vel *= Mathf.Pow(friction, Time.deltaTime);
		angleSpeed *= Mathf.Pow(friction, Time.deltaTime);
		
		if (deathTimer<=0 && !cloneKiller){
			Destroy(gameObject);
		}
		if (cloneKiller && !anim.Playing){
			Destroy(gameObject);
		}
	}
}
