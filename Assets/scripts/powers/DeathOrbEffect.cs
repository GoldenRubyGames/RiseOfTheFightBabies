using UnityEngine;
using System.Collections;

public class DeathOrbEffect : MonoBehaviour {
	
	Player owner;
	
	public float time;
	private float timer;
	
	public float startSize;
	public float growSpeed;
	
	public tk2dSprite sprite;
	public float rotateSpeed;
	
	public void setup(Player _owner){
		owner = _owner;
		
		timer = time;
		
		transform.localScale = new Vector3(startSize, startSize, startSize);
	}
	
	void Update () {
		timer -= Time.deltaTime;
		if (timer <= 0){
			Destroy(gameObject);
		}
		
		transform.localScale += new Vector3(growSpeed*Time.deltaTime, growSpeed*Time.deltaTime, growSpeed*Time.deltaTime);
		
		sprite.gameObject.transform.localEulerAngles += new Vector3(0,0, rotateSpeed*Time.deltaTime);
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
