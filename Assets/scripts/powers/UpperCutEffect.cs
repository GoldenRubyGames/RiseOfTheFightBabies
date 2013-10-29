using UnityEngine;
using System.Collections;

public class UpperCutEffect : PowerEffect {

	public Vector3 vel;
	public float velFriction;
	
	public Vector3 offset;
	
	public float time;
	private float timer;
	
	
	public tk2dSprite sprite;
	 
	public override void setupCustom(){
		
		timer = time;
		
		offset.x *= Owner.facingDir;
		vel.x    *= Owner.facingDir;
		
		transform.position = Owner.transform.position + offset;
		
		sprite.color = Owner.myColor;
		sprite.FlipX = Owner.facingDir == -1;
		if (Owner.facingDir == -1){
			sprite.gameObject.transform.localPosition = new Vector3(-sprite.gameObject.transform.localPosition.x, sprite.gameObject.transform.localPosition.y, sprite.gameObject.transform.localPosition.z);
		}
		
		//transform.localEulerAngles += new Vector3(0,0, 150 * Owner.facingDir);
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
			Destroy(gameObject);
		}
		
		//blink if player is blinking
		if (Owner.avatar.gameObject.active != sprite.gameObject.active){
			sprite.gameObject.SetActive(Owner.avatar.gameObject.active);
		}
	}
	
	void OnTriggerEnter(Collider other) {
	
		if (other.gameObject.layer == LayerMask.NameToLayer("playerHitBox") ){
			//get the player
			Player thisPlayer = other.gameObject.transform.parent.gameObject.GetComponent<Player>();
			if (thisPlayer != Owner){
				thisPlayer.takeDamage(Owner);
			}
		}
	}
	
}
