using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	
	public float time;
	float timer;
	
	Player owner;
	
	public float speed;
	Vector3 vel;
	
	public Vector3 pushForce;

	public void setup(Player _owner){
		owner = _owner;
		
		timer = time;
		
		pushForce.x *= owner.facingDir;
		
		vel = new Vector3( speed*owner.facingDir, 0, 0);
		vel.x += owner.CurVel.x;
		transform.position = owner.transform.position + new Vector3(0.5f*owner.facingDir, 0, 0);
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
		
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("shield") ){
			ShieldEffect thisShield = other.gameObject.GetComponent<ShieldEffect>();
			if (thisShield.Owner != owner){
				Destroy(gameObject);
			}
		}
		
		if (other.gameObject.layer == LayerMask.NameToLayer("playerHitBox") ){
			//get the player
			Player thisPlayer = other.gameObject.transform.parent.gameObject.GetComponent<Player>();
			if (thisPlayer != owner){
				hitPlayer(thisPlayer);
				Destroy(gameObject);
			}
		}
		
	}
	
	void hitPlayer(Player targetPlayer){
		targetPlayer.push( pushForce );
		targetPlayer.changeHealth(-1, owner);
		
	}
}
