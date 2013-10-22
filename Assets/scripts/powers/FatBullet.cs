using UnityEngine;
using System.Collections;

public class FatBullet : MonoBehaviour {
	
	public float time;
	float timer;
	
	Player owner;
	
	public float speed;
	Vector3 vel;
	
	public Vector3 pushForce;
	
	public tk2dSprite sprite;
	
	private float curScale;
	public float startScale;
	public float growSpeed;

	public void setup(Player _owner){
		owner = _owner;
		
		timer = time;
		
		pushForce.x *= owner.facingDir;
		
		vel = new Vector3( speed*owner.facingDir, 0, 0);
		
		float spriteAngle = Mathf.Atan2( vel.y, vel.x);
		sprite.gameObject.transform.localEulerAngles = new Vector3(0,0,  spriteAngle*Mathf.Rad2Deg);
		
		transform.position = owner.transform.position + new Vector3(0.5f*owner.facingDir, 0, 0);
		
		curScale = startScale;
		transform.localScale = new Vector3(curScale, curScale, curScale);
		
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
		
		//when it first starts, we need to scale it up
		if (curScale != 1){
			curScale += growSpeed * Time.deltaTime;
			if (curScale > 1)  curScale = 1;
			
			transform.localScale = new Vector3(curScale, curScale, curScale);
		}
		
	}
	
	
	void OnTriggerEnter(Collider other) {
		
		//hit player
		if (other.gameObject.layer == LayerMask.NameToLayer("playerHitBox") ){
			//get the player
			Player thisPlayer = other.gameObject.transform.parent.gameObject.GetComponent<Player>();
			if (thisPlayer != owner){
				thisPlayer.changeHealth(-1, owner);
			}
		}
		
	}
}
