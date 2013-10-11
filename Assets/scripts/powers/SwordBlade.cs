using UnityEngine;
using System.Collections;

public class SwordBlade : MonoBehaviour {
	
	public SwordEffect parent;

	void OnTriggerEnter(Collider other) {
		//this thing goes through shields!
		
		//hit player
		if (other.gameObject.layer == LayerMask.NameToLayer("playerHitBox") ){
			//get the player
			Player thisPlayer = other.gameObject.transform.parent.gameObject.GetComponent<Player>();
			if (thisPlayer != parent.Owner){
				thisPlayer.changeHealth(-1, parent.Owner);
			}
		}
		
	}
}
