using UnityEngine;
using System.Collections;

public class RocketEffect : PowerEffect {
	
	public Vector3 moveForce;
	
	public tk2dSprite sprite;
	
	//exploding
	public GameObject explosionPrefab;
	
	public override void setupCustom(){
		
		moveForce.x *= Owner.facingDir;
		transform.position = Owner.transform.position + new Vector3( Owner.facingDir*1, 0, 0);
		
		if (Owner.facingDir == -1){
			sprite.FlipX = true;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.timeScale != 0){
			rigidbody.AddForce(moveForce);
		}
	}
	
	void explode(){
		GameObject newExplosion = Instantiate(explosionPrefab, transform.position, new Quaternion(0,0,0,0)) as GameObject;
		newExplosion.GetComponent<Explosion>().setOwner(Owner);
		
		Destroy(gameObject);
	}
	
	void OnCollisionEnter(Collision collision) {
        
		//ignore player shells
		if (collision.gameObject.layer == LayerMask.NameToLayer("player")){
			explode();
		}
		
		//did we touch a player?
		if (collision.gameObject.layer == LayerMask.NameToLayer("playerHitBox")){
			Player thisPlayer = collision.gameObject.transform.parent.gameObject.GetComponent<Player>();
			if (thisPlayer != Owner){
				explode();
			}else{
				return;
			}
		}
		
		//did we touch a power effect (other than another rocket)? If so, ignore this
		if (collision.gameObject.tag == "powerEffect" && collision.gameObject.layer != LayerMask.NameToLayer("rocket")){
			return;
		}
		
		
		explode();
        
    }
}
