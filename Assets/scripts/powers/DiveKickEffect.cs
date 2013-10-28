using UnityEngine;
using System.Collections;

public class DiveKickEffect : PowerEffect {

	public Vector3 vel;
	public float velFriction;
	
	public Vector3 offset;
	
	public float time;
	private float timer;
	
	public float playerRotation;
	
	public override void setupCustom(){
		Owner.startKickAnimation( playerRotation*Owner.facingDir);
		
		timer = time;
		
		offset.x *= Owner.facingDir;
		vel.x    *= Owner.facingDir;
		
		transform.position = Owner.transform.position + offset;
		
		renderer.material.color = Owner.myColor;
		
		transform.localEulerAngles += new Vector3(0,0, 45 * Owner.facingDir);
		
	}

	
	// Update is called once per frame
	void Update () {
		
		if (!Owner.gm.Paused){
			Owner.push(vel);
			vel *= Mathf.Pow(velFriction, Time.deltaTime);
		}
		transform.position = Owner.transform.position + offset;
		
		timer -= Time.deltaTime;
		if (timer <= 0){
			Owner.endKickAnimation();
			Destroy(gameObject);
		}
	}
	
	void OnTriggerEnter(Collider other) {
	
		if (other.gameObject.layer == LayerMask.NameToLayer("playerHitBox") ){
			//get the player
			Player thisPlayer = other.gameObject.transform.parent.gameObject.GetComponent<Player>();
			if (thisPlayer != Owner){
				thisPlayer.takeDamage(Owner, IsCloneKiller);
			}
		}
	}
	
}
