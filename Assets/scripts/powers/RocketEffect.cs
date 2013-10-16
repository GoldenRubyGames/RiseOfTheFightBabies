using UnityEngine;
using System.Collections;

public class RocketEffect : MonoBehaviour {
	
	Player owner;
	
	public Vector3 moveForce;
	
	public tk2dSprite sprite;
	
	//exploding
	public GameObject explosionPrefab;
	
	public void setup(Player _owner){
		owner = _owner;
		
		moveForce.x *= owner.facingDir;
		transform.position = owner.transform.position + new Vector3( owner.facingDir*1, 0, 0);
		
		if (owner.facingDir == -1){
			sprite.FlipX = true;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
		rigidbody.AddForce(moveForce);
		
	}
	
	void explode(){
		GameObject newExplosion = Instantiate(explosionPrefab, transform.position, new Quaternion(0,0,0,0)) as GameObject;
		newExplosion.SendMessage("setOwner", owner);
		
		Destroy(gameObject);
	}
	
	void OnCollisionEnter(Collision collision) {
        
		//ignore player shells
		if (collision.gameObject.layer == LayerMask.NameToLayer("player")){
			explode();;
		}
		
		//did we touch a player?
		if (collision.gameObject.layer == LayerMask.NameToLayer("playerHitBox")){
			Player thisPlayer = collision.gameObject.transform.parent.gameObject.GetComponent<Player>();
			if (thisPlayer != owner){
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
