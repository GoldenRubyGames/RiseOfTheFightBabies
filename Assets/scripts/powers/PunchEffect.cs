using UnityEngine;
using System.Collections;

public class PunchEffect : MonoBehaviour {
	
	public float speed;
	
	public float maxDist;
	
	Player owner;
	
	public Vector3 pushForce;
	
	int dir;
	float curDist;
	
	public void setup(Player _owner){
		owner = _owner;
		
		dir = owner.FacingDir;
		
		curDist = 0.5f;
		
		pushForce.x *= dir;
		
		transform.position = owner.transform.position + new Vector3(0.5f*owner.FacingDir, 0, 0);
		
		renderer.material.color = owner.myColor;
		
	}

	
	// Update is called once per frame
	void Update () {
		curDist += speed * Time.deltaTime;
		transform.position = owner.transform.position + new Vector3(curDist*dir, 0, 0);
		
		if( curDist >= maxDist){
			Destroy(gameObject);
		}
		
		//if the owner changes direction, kill it
		if (dir != owner.FacingDir){
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
			}
		}
	}
	
	void hitPlayer(Player targetPlayer){
		targetPlayer.push( pushForce );
		targetPlayer.changeHealth(-1, owner);
		
	}
}
