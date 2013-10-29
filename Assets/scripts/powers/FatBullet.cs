using UnityEngine;
using System.Collections;

public class FatBullet : PowerEffect {
	
	public float time;
	float timer;
	
	public float speed;
	Vector3 vel;
	
	public Vector3 pushForce;
	
	public tk2dSprite sprite;
	
	private float curScale;
	public float startScale;
	public float growSpeed;

	public override void setupCustom(){
		
		timer = time;
		
		pushForce.x *= Owner.facingDir;
		
		vel = new Vector3( speed*Owner.facingDir, 0, 0);
		
		float spriteAngle = Mathf.Atan2( vel.y, vel.x);
		sprite.gameObject.transform.localEulerAngles = new Vector3(0,0,  spriteAngle*Mathf.Rad2Deg);
		
		transform.position = Owner.transform.position + new Vector3(0.5f*Owner.facingDir, 0, 0);
		
		curScale = startScale;
		transform.localScale = new Vector3(curScale, curScale, curScale);
		
	}
	
	// Update is called once per frame
	void Update () {
		
		//move it
		transform.position += vel * Time.deltaTime;
		
		//is it time to die?
		timer -= Time.deltaTime;
		if (timer <= 0){
			Destroy(gameObject);
		}
		
		//when it first starts, we need to scale it up
		if (curScale != 1){
			curScale += growSpeed * Time.deltaTime;
			if (curScale > 1)  curScale = 1;
			
			transform.localScale = new Vector3(curScale, curScale, curScale);
		}
		
	}
	
	
	void OnTriggerEnter(Collider other) {
		
		//hit player
		if (other.gameObject.layer == LayerMask.NameToLayer("playerHitBox") ){
			//get the player
			Player thisPlayer = other.gameObject.transform.parent.gameObject.GetComponent<Player>();
			if (thisPlayer != Owner){
				thisPlayer.takeDamage(Owner);
			}
		}
		
	}
}
