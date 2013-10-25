using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	
	
	public float explosionTime;
	public float explosionScale;
	//public GameObject trigger;
	
	private float timer;
	
	private Player owner;
	
	public float rotationSpeed;
	
	//sound
	public AudioClip soundEffect;

	// Use this for initialization
	void Start () {
		timer = explosionTime;
		
	}
	
	public void setOwner(Player _owner){
		owner = _owner;
		owner.AudioController.Play(soundEffect);
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer <= 0){
			Destroy(gameObject);
		}
		
		float thisScale = (1-(timer/explosionTime)) * explosionScale;
		transform.localScale = new Vector3(thisScale, thisScale, thisScale);
		
		transform.localEulerAngles += new Vector3(0,0,rotationSpeed)*Time.deltaTime;
	}
	
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("playerHitBox") ){
			//get the player
			//THIS CAN HIT THE OWNER BECAUSE IT'S A GODDAMN EXPLOSION
			Player thisPlayer = other.gameObject.transform.parent.gameObject.GetComponent<Player>();
			
			thisPlayer.changeHealth(-1, owner);
			
		}
		
	}
}
