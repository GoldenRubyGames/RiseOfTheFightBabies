using UnityEngine;
using System.Collections;

public class SwordBlade : MonoBehaviour {
	
	public SwordEffect parent;

	void OnTriggerEnter(Collider other) {
		
		//hit player
		if (other.gameObject.layer == LayerMask.NameToLayer("playerHitBox") ){
			//get the player
			Player thisPlayer = other.gameObject.transform.parent.gameObject.GetComponent<Player>();
			if (thisPlayer != parent.Owner){
				thisPlayer.takeDamage(parent.Owner, parent.IsCloneKiller);
			}
		}
		
	}
}
