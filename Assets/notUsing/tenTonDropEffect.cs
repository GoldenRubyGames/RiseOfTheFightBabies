using UnityEngine;
using System.Collections;

public class tenTonDropEffect : PowerEffect {
	
	public Vector3 startVel;
	public Vector3 pushForce;
	public float dampening;
	
	public float startDist;
	
	public float time;
	private float timer;
	
	public tk2dSprite sprite;
	
	public float blinkTime, blinkSpeed;

	
	public override void setupCustom(){
		
		timer = time;
		
		//start below the player
		transform.position = Owner.transform.position + new Vector3(0, -startDist, 0);
		rigidbody.velocity = startVel;
	}
	
	// Update is called once per frame
	void Update () {
		
		rigidbody.AddForce(pushForce);
	
		time -= Time.deltaTime;
		if (time <= 0){
			Destroy(gameObject);
		}
		if (time <= blinkTime){
			bool isOn = time % blinkSpeed < blinkSpeed/2;
			sprite.renderer.enabled = isOn;
		}
		
	}
	
	void OnCollisionEnter(Collision collision) {
	    
		
		//did we touch a player that is below us?
		if (collision.gameObject.layer == LayerMask.NameToLayer("playerHitBox")){
			if( collision.gameObject.transform.position.y < transform.position.y){
				Player thisPlayer = collision.gameObject.transform.parent.gameObject.GetComponent<Player>();
				if (thisPlayer != Owner){
					thisPlayer.takeDamage(Owner);
				}
			}
		}
		else{
			//reduce bouncing
			rigidbody.velocity *= dampening;
		}
		
    }
}
