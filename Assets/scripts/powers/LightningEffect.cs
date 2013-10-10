using UnityEngine;
using System.Collections;

public class LightningEffect : MonoBehaviour {
	
	Player owner;
	
	public Vector3 pushForce;
	
	public float growSpeed;
	
	public float killTime;
	private float timer;
	
	public void setup(Player _owner){
		owner = _owner;
		timer = 0;
		
		owner.push( pushForce );
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
