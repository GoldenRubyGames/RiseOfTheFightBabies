using UnityEngine;
using System.Collections;

public class LightningEffect : PowerEffect {
	
	public Vector3 moveSpeed;
	
	public float killTime;
	private float timer;
	
	public tk2dSprite sprite;
	
	public float flipTime;
	
	public override void setupCustom(bool goUp){
		timer = 0;
		
		if (!goUp){
			moveSpeed.y *= -1;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		timer+=Time.deltaTime;
		if (timer >= killTime){
			Destroy(gameObject);
		}
		
		//move it!
		transform.position += moveSpeed * Time.deltaTime;
		
		//flip the sprite over time
		sprite.FlipX = timer%flipTime > flipTime/2;
		
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
