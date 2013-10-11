using UnityEngine;
using System.Collections;

public class SpikeShoesEffect : MonoBehaviour {
	
	Player owner;
	
	public Vector3 offsetFromPlayer;
	public Vector3 pushForce;
	
	private Vector3 prevPos;
	

	public void setup(Player _owner){
		owner = _owner;
	}
	
	// Update is called once per frame
	void Update () {
		//keep it in place on the bottom of the player
		prevPos = transform.position;
		transform.position = owner.transform.position + offsetFromPlayer;
	}
	
	void OnTriggerEnter(Collider other) {
		//only kill things if the player is moving down
		if (prevPos.y > transform.position.y){
		
			if (other.gameObject.layer == LayerMask.NameToLayer("playerHitBox") ){
				//get the player
				Player thisPlayer = other.gameObject.transform.parent.gameObject.GetComponent<Player>();
				if (thisPlayer != owner){
					hitPlayer(thisPlayer);
				}
			}
		}
	}
	
	void hitPlayer(Player targetPlayer){
		targetPlayer.push( pushForce );
		targetPlayer.changeHealth(-1, owner);
		
	}
}
