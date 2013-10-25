using UnityEngine;
using System.Collections;

public class MineEffect : MonoBehaviour {
	
	private Player owner;
	
	public Vector3 startVel;
	public Vector3 pushForce;
	public float dampening;
	
	public float startDist;
	
	
	public GameObject explosionPrefab;
	
	public tk2dSpriteAnimator sprite;

	
	public void setup(Player _owner){
		owner = _owner;
		
		//start below the player
		transform.position = owner.transform.position + new Vector3(0, -startDist, 0);
		rigidbody.velocity = startVel;
		
		if (owner.isGhost){
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
			if (thisPlayer != owner){
				//explode!
				GameObject newExplosion = Instantiate(explosionPrefab, transform.position, new Quaternion(0,0,0,0)) as GameObject;
				newExplosion.SendMessage("setOwner", owner);
				Destroy(gameObject);
			}
		}
		else{
			//reduce bouncing
			rigidbody.velocity *= dampening;
		}
		
    }
}
