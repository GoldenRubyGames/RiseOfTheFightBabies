using UnityEngine;
using System.Collections;

public class BatEffect : PowerEffect {
	
	public float speed;
	
	public float maxDist;
	
	public Vector3 pushForce;
	
	int dir;
	float curDist;
	
	public tk2dSprite sprite;
	
	private float startDist;
	private bool goingBack;

	public override void setupCustom(){
		
		dir = Owner.facingDir;
		
		sprite.FlipX = Owner.facingDir == -1;
		
		curDist = 1f;
		startDist = curDist;
		
		pushForce.x *= dir;
		
		transform.position = Owner.transform.position + new Vector3(0.5f*Owner.facingDir, 0, 0);
		
		goingBack = false;
	}

	
	// Update is called once per frame
	void Update () {
		int nowDir = goingBack ? -1 : 1;
		curDist += speed * Time.deltaTime * nowDir;
		transform.position = Owner.transform.position + new Vector3( dir*curDist*0.5f, 0,0);
		
		transform.localScale = new Vector3(curDist, transform.localScale.y, transform.localScale.z);
		
		if( curDist >= maxDist){
			goingBack = true;
		}
		if (goingBack && curDist <= startDist){
			Destroy(gameObject);
		}
		
		//if the Owner changes direction, kill it
		if (dir != Owner.facingDir){
			Destroy(gameObject);
		}
	}
	
	void OnTriggerEnter(Collider other) {
		
		if (other.gameObject.layer == LayerMask.NameToLayer("playerHitBox") ){
			//get the player
			Player thisPlayer = other.gameObject.transform.parent.gameObject.GetComponent<Player>();
			if (thisPlayer != Owner){
				thisPlayer.takeDamage(Owner, IsCloneKiller);
			}
		}
	}
	
}
