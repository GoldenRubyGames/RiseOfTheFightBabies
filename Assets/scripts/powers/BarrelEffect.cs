using UnityEngine;
using System.Collections;

public class BarrelEffect : PowerEffect {
	
	public float speed;
	public float dampening;
	
	private int facingDir;
	
	public float time;
	private float timer;
	public float blinkTime;
	public float blinkSpeed;
	
	public tk2dSprite sprite;
	
	public override void setupCustom(){
		timer = time;
		
		facingDir = Owner.facingDir;
		
		transform.position = Owner.transform.position + new Vector3( facingDir, 0,0);
		transform.localEulerAngles = new Vector3(90, 0, 0);
	}
	
	
	// Update is called once per frame
	void Update () {
		if (Time.timeScale != 0){
			rigidbody.AddForce( Vector3.right * speed * facingDir);
		}
		
		//is time time to die?
		timer -= Time.deltaTime;
		if (timer <= 0){
			Destroy(gameObject);
		}
		if (timer <= blinkTime){
			sprite.gameObject.SetActive( timer%blinkSpeed < blinkSpeed/2);
		}
	}
	
	void OnCollisionEnter(Collision collision) {
		rigidbody.velocity *= dampening;
		if (rigidbody.velocity.x > 0 ){
			facingDir = 1;
		}
		if (rigidbody.velocity.x < 0){
			facingDir = -1;
		}
		
		
		//did we hit a player?
		if (collision.gameObject.layer == LayerMask.NameToLayer("playerHitBox")){
			Player thisPlayer = collision.gameObject.transform.parent.gameObject.GetComponent<Player>();
			if (thisPlayer != Owner){
				thisPlayer.takeDamage(Owner);
			}
		}
		
		
	}
}
