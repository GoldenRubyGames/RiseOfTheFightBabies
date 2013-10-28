using UnityEngine;
using System.Collections;

public class FreezeRayEffect : PowerEffect {
	
	public float growSpeed;
	
	public float killTime;
	private float timer;
	
	public float freezeTime;
	
	public override void setupCustom(){
		timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
		timer+=Time.deltaTime;
		if (timer >= killTime){
			Destroy(gameObject);
		}
		
		//keep making it bigger
		transform.localScale += new Vector3(0, growSpeed*Time.deltaTime, 0);
		
	}
	
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("playerHitBox") ){
			//get the player
			Player thisPlayer = other.gameObject.transform.parent.gameObject.GetComponent<Player>();
			if (thisPlayer != Owner){
				thisPlayer.freeze(freezeTime);
			}
		}
	}
	
}
