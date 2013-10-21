using UnityEngine;
using System.Collections;

public class UpperCutEffect : MonoBehaviour {

	public Vector3 vel;
	public float velFriction;
	
	public Vector3 offset;
	
	public float time;
	private float timer;
	
	Player owner;
	
	public tk2dSprite sprite;
	 
	public void setup(Player _owner){
		owner = _owner;
		
		timer = time;
		
		offset.x *= owner.facingDir;
		vel.x    *= owner.facingDir;
		
		transform.position = owner.transform.position + offset;
		
		sprite.color = owner.myColor;
		sprite.FlipX = owner.facingDir == -1;
		if (owner.facingDir == -1){
			sprite.gameObject.transform.localPosition = new Vector3(-sprite.gameObject.transform.localPosition.x, sprite.gameObject.transform.localPosition.y, sprite.gameObject.transform.localPosition.z);
		}
		
		//transform.localEulerAngles += new Vector3(0,0, 150 * owner.facingDir);
	}

	
	// Update is called once per frame
	void Update () {
		
		if (!owner.gm.Paused){
			owner.push(vel);
			vel *= Mathf.Pow(velFriction, Time.deltaTime);
		}
		
		transform.position = owner.transform.position + offset;
		
		timer -= Time.deltaTime;
		if (timer <= 0){
			Destroy(gameObject);
		}
		
		//blink if player is blinking
		if (owner.avatar.gameObject.active != sprite.gameObject.active){
			sprite.gameObject.SetActive(owner.avatar.gameObject.active);
		}
	}
	
	void OnTriggerEnter(Collider other) {
	
		if (other.gameObject.layer == LayerMask.NameToLayer("playerHitBox") ){
			//get the player
			Player thisPlayer = other.gameObject.transform.parent.gameObject.GetComponent<Player>();
			if (thisPlayer != owner){
				thisPlayer.changeHealth(-1, owner);
			}
		}
	}
	
}
