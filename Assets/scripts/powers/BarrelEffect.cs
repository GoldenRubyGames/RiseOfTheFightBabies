using UnityEngine;
using System.Collections;

public class BarrelEffect : MonoBehaviour {
	
	public float speed;
	public float dampening;
	
	private int facingDir;
	
	private Player owner;
	
	public float time;
	private float timer;
	public float blinkTime;
	public float blinkSpeed;
	
	public tk2dSprite sprite;
	
	public void setup(Player _owner){
		owner = _owner;
		timer = time;
		
		facingDir = owner.facingDir;
		
		transform.position = owner.transform.position + new Vector3( facingDir, 0,0);
		transform.localEulerAngles = new Vector3(90, 0, 0);
	}
	
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(Vector3.right * speed - rigidbody.velocity);
		//rigidbody.AddForce ( Vector3.right * speed * facingDir - rigidbody.velocity);
		rigidbody.AddForce ( Vector3.right * speed * facingDir);
		
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
			if (thisPlayer != owner){
				thisPlayer.changeHealth(-1, owner);
			}
		}
		
		
	}
}
