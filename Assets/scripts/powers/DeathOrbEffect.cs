using UnityEngine;
using System.Collections;

public class DeathOrbEffect : MonoBehaviour {
	
	Player owner;
	
	public float time;
	private float timer;
	
	public float startSize;
	public float growSpeed;
	
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
	}
	
	
	void OnTriggerEnter(Collider other) {
		//does not give a fuck about shields
		
		
		if (other.gameObject.layer == LayerMask.NameToLayer("playerHitBox") ){
			//get the player
			Player thisPlayer = other.gameObject.transform.parent.gameObject.GetComponent<Player>();
			if (thisPlayer != owner || timer != time){
				thisPlayer.changeHealth(-1, owner);
			}
		}
	}
}
