using UnityEngine;
using System.Collections;

public class PunchEffect : PowerEffect {
	
	public float speed;
	
	public float maxDist;
	
	public Vector3 pushForce;
	
	int dir;
	float curDist;
	
	public tk2dSprite sprite;
	
	public override void setupCustom(){
		
		dir = Owner.facingDir;
		
		curDist = 0.5f;
		
		pushForce.x *= dir;
		
		transform.position = Owner.transform.position + new Vector3(0.5f*Owner.facingDir, 0, 0);
		
		renderer.material.color = Owner.myColor;
		sprite.color = Owner.myColor;
		sprite.FlipX = Owner.facingDir == -1;
	}

	
	// Update is called once per frame
	void Update () {
		curDist += speed * Time.deltaTime;
		transform.position = Owner.transform.position + new Vector3(curDist*dir, 0, 0);
		
		if( curDist >= maxDist){
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
