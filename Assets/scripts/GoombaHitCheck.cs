using UnityEngine;
using System.Collections;

public class GoombaHitCheck : MonoBehaviour {
	
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("playerHitBox") ){
			//get the player
			Player thisPlayer = other.gameObject.transform.parent.gameObject.GetComponent<Player>();
			
			if (thisPlayer.isPlayerControlled){
				thisPlayer.changeHealth(-1, null);
			}
		}
	}
}
