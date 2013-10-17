using UnityEngine;
using System.Collections;

public class BatEffect : MonoBehaviour {
	
	public float speed;
	
	public float maxDist;
	
	Player owner;
	
	public Vector3 pushForce;
	
	int dir;
	float curDist;
	
	public tk2dSprite sprite;
	
	private float startDist;
	private bool goingBack;

	public void setup(Player _owner){
		owner = _owner;
		
		dir = owner.facingDir;
		
		sprite.FlipX = owner.facingDir == -1;
		
		curDist = 1f;
		startDist = curDist;
		
		pushForce.x *= dir;
		
		transform.position = owner.transform.position + new Vector3(0.5f*owner.facingDir, 0, 0);
		
		goingBack = false;
	}

	
	// Update is called once per frame
	void Update () {
		int nowDir = goingBack ? -1 : 1;
		curDist += speed * Time.deltaTime * nowDir;
		transform.position = owner.transform.position + new Vector3( dir*curDist*0.5f, 0,0);
		
		transform.localScale = new Vector3(curDist, transform.localScale.y, transform.localScale.z);
		
		if( curDist >= maxDist){
			goingBack = true;
		}
		if (goingBack && curDist <= startDist){
			Destroy(gameObject);
		}
		
		//if the owner changes direction, kill it
		if (dir != owner.facingDir){
			Destroy(gameObject);
		}
	}
	
	void OnTriggerEnter(Collider other) {
		
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
