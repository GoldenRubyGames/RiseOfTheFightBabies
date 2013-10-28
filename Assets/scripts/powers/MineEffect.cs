using UnityEngine;
using System.Collections;

public class MineEffect : PowerEffect {
	
	public Vector3 startVel;
	public Vector3 pushForce;
	public float dampening;
	
	public float startDist;
	
	
	public GameObject explosionPrefab;
	
	public tk2dSpriteAnimator sprite;

	
	public override void setupCustom(){
		
		//start below the player
		transform.position = Owner.transform.position + new Vector3(0, -startDist, 0);
		rigidbody.velocity = startVel;
		
		if (Owner.isGhost){
			sprite.Play("mineGhost");
		}else{
			sprite.Play("mine");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		rigidbody.AddForce(pushForce);
	
		
	}
	
	void OnCollisionEnter(Collision collision) {
	    
		
		//did we touch a player
		if (collision.gameObject.layer == LayerMask.NameToLayer("playerHitBox")){
			Player thisPlayer = collision.gameObject.transform.parent.gameObject.GetComponent<Player>();
			if (thisPlayer != Owner){
				//explode!
				GameObject newExplosion = Instantiate(explosionPrefab, transform.position, new Quaternion(0,0,0,0)) as GameObject;
				newExplosion.GetComponent<Explosion>().setOwner(Owner, IsCloneKiller);
				Destroy(gameObject);
			}
		}
		else{
			//reduce bouncing
			rigidbody.velocity *= dampening;
		}
		
    }
}
